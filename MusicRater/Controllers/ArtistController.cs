using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicRater.Models;
using MusicRater.Data;
using Microsoft.AspNetCore.Authorization;
using MusicRater.Models.ViewModels;
using MusicRater.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace MusicRater.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ILogger<ArtistController> _logger;
        private MusicRaterContext context;
        private UserManager<MusicRaterUser> _userManager;
        public ArtistController(ILogger<ArtistController> logger, 
            MusicRaterContext data,
            UserManager<MusicRaterUser> userManager)
        {
            context = data;
            _logger = logger;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
           return View(context.Artists.OrderBy(a => a.Name));
        }

        public IActionResult New()
        {
            return View("ArtistEditor", new Artist());
        }

        [HttpPost]
        public async Task<IActionResult> New([FromForm] Artist artist)
        {
            if (ModelState.IsValid && ValidBirthDate(artist) && ValidDeathDate(artist))
            {
                artist.FormattedBirthDate = FormattedDateTime.GetFormattedDate(artist.BirthDay, artist.BirthMonth, artist.BirthYear);
                artist.FormattedDeathDate = FormattedDateTime.GetFormattedDate(artist.DeathDay, artist.DeathMonth, artist.DeathYear);
                context.Artists.Add(artist);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Profile), new { id = artist.Id });
            }
            return View("ArtistEditor", artist);
        }

        [AllowAnonymous]
        public async Task <IActionResult> Profile(long id)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.Id == id);
            if(artist == null)
            {
                return View("NotFound", "artist");
            }
            var releases = await context.Releases.Where(r => r.ArtistId == id).ToListAsync();
            ArtistProfileViewModel artistViewModel = new ArtistProfileViewModel
            {
                Artist = artist,
                Albums = releases.Where(r => r.Type == ReleaseType.Album).ToList(),
                Compilations = releases.Where(r => r.Type == ReleaseType.Compilation).ToList(),
                Eps = releases.Where(r => r.Type == ReleaseType.Ep).ToList(),
                Mixtapes = releases.Where(r => r.Type == ReleaseType.Mixtape).ToList(),
                Singles = releases.Where(r => r.Type == ReleaseType.Single).ToList(),
                LiveAlbums = releases.Where(r => r.Type == ReleaseType.Live).ToList(),
                Bootlegs = releases.Where(r => r.Type == ReleaseType.Bootleg).ToList(),
                DJMixes = releases.Where(r => r.Type == ReleaseType.DjMix).ToList(),
            };
            if (User.Identity.IsAuthenticated)
            {
                MusicRaterUser user = await _userManager.GetUserAsync(User);
                artistViewModel.IsAdmin = await _userManager.IsInRoleAsync(user, "Administrator");
            }
                return View(artistViewModel);
        }
    
        public async Task<IActionResult> EditRequest(long id)
        {
            ArtistEditRequest editRequest = await context.ArtistEditRequests
                .Include(er => er.Artist)
                .Include(er => er.SubmittingUser)
                .Include(er => er.Comments)
                .ThenInclude(c=>c.User)
                .FirstOrDefaultAsync(er => er.ArtistId == id);
            if (editRequest == null)
            {
                Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.Id == id);
                if (artist == null)
                {
                    return View("NotFound", "artist");
                }
                return View("ArtistEditor", artist);
            }
            else
            {
                MusicRaterUser user = await _userManager.GetUserAsync(User);
                ViewBag.IsOwner = user.Id == editRequest.SubmittingUserId;
                ViewBag.IsAdmin = await _userManager.IsInRoleAsync(user, "Administrator");
                return View(editRequest);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRequest(long id, [FromForm] ArtistEditRequest artistEditRequest)
        {
            ArtistEditRequest editRequest = await context.ArtistEditRequests.FirstOrDefaultAsync(er => er.ArtistId == id);
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.Id == id);
            if (artist == null)
            {
                return View("NotFound", "artist");
            }
            if (ModelState.IsValid && CheckArtistEditRequest(artist, artistEditRequest)){
                if (editRequest == null)
                {
                    artistEditRequest.ArtistId = id;
                    artistEditRequest.SubmittedDate = DateTime.Now;
                    artistEditRequest.SubmittingUser = await _userManager.GetUserAsync(User);
                    context.ArtistEditRequests.Add(artistEditRequest);
                    await context.SaveChangesAsync();
                }
                else
                {
                    MusicRaterUser user = await _userManager.GetUserAsync(User);
                    if (editRequest.SubmittingUserId == user.Id || await _userManager.IsInRoleAsync(user, "Administrator"))
                    {
                        editRequest.Name = artistEditRequest.Name;
                        editRequest.IsSoloArtist = artistEditRequest.IsSoloArtist;
                        editRequest.OriginCountry = artistEditRequest.OriginCountry;
                        editRequest.BirthDay = artistEditRequest.BirthDay;
                        editRequest.BirthMonth = artistEditRequest.BirthMonth;
                        editRequest.BirthYear = artistEditRequest.BirthYear;
                        editRequest.DeathDay = artistEditRequest.DeathDay;
                        editRequest.DeathMonth = artistEditRequest.DeathMonth;
                        editRequest.DeathYear = artistEditRequest.DeathYear;
                        await context.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction(nameof(EditRequest), id);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ApproveEdit(long id)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.Id == id);
            if (artist == null)
            {
                return View("NotFound", "artist");
            }
            ArtistEditRequest editRequest = await context.ArtistEditRequests
                .Include(er => er.SubmittingUser)
                .FirstOrDefaultAsync(er => er.ArtistId == id);
            artist.Name = editRequest.Name;
            artist.IsSoloArtist = editRequest.IsSoloArtist;
            artist.OriginCountry = editRequest.OriginCountry;
            artist.BirthDay = editRequest.BirthDay;
            artist.BirthMonth = editRequest.BirthMonth;
            artist.BirthYear = editRequest.BirthYear;
            artist.DeathDay = editRequest.DeathDay;
            artist.DeathMonth = editRequest.DeathMonth;
            artist.DeathYear = editRequest.DeathYear;

            MusicRaterUser user = await _userManager.GetUserAsync(User);
            editRequest.SubmittingUser.UserNotifications.Add(new UserNotification
            {

                Title = $"Your edit request for <a href='/Artist/Profile/{editRequest.ArtistId}'>{editRequest.Name}</a> has been approved!",
                SiteMessage = "",
                Date = DateTime.Now,
                RecipientUserId = editRequest.SubmittingUserId,
                SendingUser = user
            });
            editRequest.SubmittingUser.UnreadNotificationCount++;
            context.RemoveRange(editRequest.Comments);
            context.Remove(editRequest);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Profile), new { id });
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> DenyEdit(long id, [FromForm] string denyMessage)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.Id == id);
            if (artist == null)
            {
                return View("NotFound", "artist");
            }
            ArtistEditRequest editRequest = await context.ArtistEditRequests
                .Include(er => er.SubmittingUser)
                .FirstOrDefaultAsync(er => er.ArtistId == id);
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            editRequest.SubmittingUser.UserNotifications.Add(new UserNotification
            {

                Title = $"Your edit request for <a href='/Artist/Profile/{editRequest.ArtistId}'>{editRequest.Name}</a> has been denied",
                SiteMessage = denyMessage != null ? denyMessage: "",
                Date = DateTime.Now,
                RecipientUserId = editRequest.SubmittingUserId,
                SendingUser = user
            });
            editRequest.SubmittingUser.UnreadNotificationCount++;
            context.RemoveRange(editRequest.Comments);
            context.Remove(editRequest);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Profile), new { id });
        }


        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(long id)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.Id == id);
            if (artist == null)
            {
                return View("NotFound", "artist");
            }
            return View("ArtistEditor", artist);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [FromForm] Artist artist) 
        {
            Artist oldArtist = await context.Artists.FirstOrDefaultAsync(a => a.Id == id);
            if (oldArtist == null)
            {
                return View("NotFound", "artist");
            }
            if (ModelState.IsValid)
            {
                oldArtist.Name = artist.Name;
                oldArtist.IsSoloArtist = artist.IsSoloArtist;
                oldArtist.OriginCountry = artist.OriginCountry;
                oldArtist.BirthDay = artist.BirthDay;
                oldArtist.BirthMonth = artist.BirthMonth;
                oldArtist.FormattedBirthDate = FormattedDateTime.GetFormattedDate(artist.BirthDay, artist.BirthMonth, artist.BirthYear);
                oldArtist.BirthYear = artist.BirthYear;
                oldArtist.DeathDay = artist.DeathDay;
                oldArtist.DeathMonth = artist.DeathMonth;
                oldArtist.DeathYear = artist.DeathYear;
                oldArtist.FormattedDeathDate = FormattedDateTime.GetFormattedDate(artist.DeathDay, artist.DeathMonth, artist.DeathYear);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Profile), new { id = oldArtist.Id });
            }
            return View("ArtistEditor", artist);
        }

        [Authorize(Roles = "Administrator")]
        public async Task <IActionResult> Delete(long id)
        {
            Artist artist = await context.Artists.Include(a => a.Releases).FirstOrDefaultAsync(a => a.Id == id);
            if (artist == null)
            {
                return View("NotFound", "artist");
            }
            foreach (Release release in artist.Releases)
            {
                context.RemoveRange(release.UserReleaseRatings);
            }
            context.RemoveRange(artist.Releases);
            context.Remove(artist);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Artist");
        }

        private bool ValidBirthDate(Artist artist)
        {
            return FormattedDateTime.DateIsValid(artist.BirthDay, artist.BirthMonth, artist.BirthYear);
        }
        private bool ValidDeathDate(Artist artist)
        {
            return FormattedDateTime.DateIsValid(artist.DeathDay, artist.DeathMonth, artist.DeathYear);
        }
        private bool CheckArtistEditRequest(Artist artist, ArtistEditRequest editRequest)
        {
            if (artist.Name == editRequest.Name &&
                artist.IsSoloArtist == editRequest.IsSoloArtist &&
                artist.OriginCountry == editRequest.OriginCountry &&
                artist.BirthDay == editRequest.BirthDay &&
                artist.BirthMonth == editRequest.BirthMonth &&
                artist.BirthYear == editRequest.BirthYear &&
                artist.DeathDay == editRequest.DeathDay &&
                artist.DeathMonth == editRequest.DeathMonth &&
                artist.DeathYear == editRequest.DeathYear)
            {
                return false;
            }
            Artist dateHolder = new Artist { BirthDay = editRequest.BirthDay, 
                BirthMonth = editRequest.BirthMonth, BirthYear = editRequest.BirthYear,
                DeathDay = editRequest.DeathDay, DeathMonth = editRequest.DeathMonth, 
                DeathYear = editRequest.DeathYear
            };
            if(!ValidBirthDate(dateHolder) || !ValidDeathDate(dateHolder)){ 
                return false;
            }
            return true;
        }
    }
}
