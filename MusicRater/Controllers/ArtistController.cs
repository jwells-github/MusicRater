using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (ModelState.IsValid)
            {
                artist.FormattedBirthDate = FormattedDateTime.GetFormattedDate(artist.BirthDay, artist.BirthMonth, artist.BirthYear);
                artist.FormattedDeathDate = FormattedDateTime.GetFormattedDate(artist.DeathDay, artist.DeathMonth, artist.DeathYear);
                context.Artists.Add(artist);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Profile), new { id = artist.ArtistID });
            }
            return View("ArtistEditor", artist);
        }

        [AllowAnonymous]
        public async Task <IActionResult> Profile(long id)
        {
            var releases = await context.Releases.Where(r => r.ArtistID == id).ToListAsync();
            ArtistProfileViewModel artistViewModel = new ArtistProfileViewModel
            {
                Artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == id),
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

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(long id)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == id);
            return View("ArtistEditor", artist);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [FromForm] Artist artist) 
        {
            Artist oldArtist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == id);
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
                return RedirectToAction(nameof(Profile), new { id = oldArtist.ArtistID });
            }
            return View("ArtistEditor", artist);
        }

        [Authorize(Roles = "Administrator")]
        public async Task <IActionResult> Delete(long id)
        {
            Artist artist = await context.Artists.Include(a => a.Releases).FirstOrDefaultAsync(a => a.ArtistID == id);
            foreach (Release release in artist.Releases)
            {
                context.RemoveRange(release.UserReleaseRatings);
            }
            context.RemoveRange(artist.Releases);
            context.Remove(artist);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Artist");
        }
    }
}
