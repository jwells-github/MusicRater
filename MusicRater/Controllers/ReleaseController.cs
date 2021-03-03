using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicRater.Areas.Identity.Data;
using MusicRater.Data;
using MusicRater.Models;

namespace MusicRater.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly ILogger<ReleaseController> _logger;
        private MusicRaterContext context;
        private UserManager<MusicRaterUser> _userManager;

        public ReleaseController(ILogger<ReleaseController> logger,
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
            return View();
        }

        public async Task <IActionResult> New(long artistID)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.Id == artistID);
            if(artist == null)
            {
                return View("NotFound", "artist");
            }
            return View("ReleaseEditor", new Release { Artist = artist}); 
        }

        [HttpPost]
        public async Task<IActionResult> New(long artistID, [FromForm] Release release)
        {
            if (ModelState.IsValid)
            {
                Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.Id == artistID);
                if (artist == null)
                {
                    return View("NotFound", "artist");
                }
                release.Artist = artist;
                release.ArtistId = artist.Id;
                release.FormattedDate = FormattedDateTime.GetFormattedDate(release.ReleaseDay, release.ReleaseMonth, release.ReleaseYear);
                context.Releases.Add(release);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Entry), new { id = release.Id });
            }
            return View("ReleaseEditor", release);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Entry(long id)
        {
            ReleaseViewModel releaseView = new ReleaseViewModel
            {
                Release = await context.Releases
                    .Include(r => r.Artist)
                    .Include(r => r.ReleaseGenres)
                    .ThenInclude(rg => rg.Genre)
                    .FirstOrDefaultAsync(r => r.Id == id),
                ReleaseReviews = await context.ReleaseReviews
                    .Include(r => r.User)
                    .Where(r => r.ReleaseId == id)
                    .ToListAsync(),
                ReleaseComments = await context.ReleaseComments
                .Where(r => r.ReleaseId == id)
                .Include(r=> r.User)
                .OrderBy(r => r.PostedDate)
                .Skip(0)
                .Take(100)
                .ToListAsync()
            };
            if(releaseView.Release == null)
            {
                return View("NotFound", "release");
            }
            if (User.Identity.IsAuthenticated)
            {
                MusicRaterUser user = await _userManager.GetUserAsync(User);
                releaseView.User = user;
                releaseView.UserReview = await context.ReleaseReviews.FirstOrDefaultAsync(r => r.ReleaseId == id && r.UserId == user.Id);
                releaseView.UserRating = await context.ReleaseRating.FirstOrDefaultAsync(r => r.ReleaseId == id && r.UserId == user.Id); ;
                releaseView.IsAdmin = await _userManager.IsInRoleAsync(user, "Administrator");
            }
            return View(releaseView);
        }

        public async Task<IActionResult> EditRequest(long id)
        {
            ReleaseEditRequest editRequest = await context.ReleaseEditRequests
                .Include(er => er.Release)
                .Include(er => er.SubmittingUser)
                .Include(er => er.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(er => er.ReleaseId == id);
            if(editRequest == null)
            {
                Release release = await context.Releases
                    .Include(r=>r.Artist).FirstOrDefaultAsync(r => r.Id == id);
                if(release == null)
                {
                    return View("NotFound", "release");
                }
                return View("ReleaseEditor", release);
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
        public async Task<IActionResult> EditRequest(long id, [FromForm] ReleaseEditRequest releaseEditRequest)
        {
            ReleaseEditRequest editRequest = await context.ReleaseEditRequests.FirstOrDefaultAsync(er => er.ReleaseId == id);
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.Id == id);
            if (release == null)
            {
                return View("NotFound", "release");
            }
            if (ModelState.IsValid && CheckReleaseEditRequest(release, releaseEditRequest)){
                if(editRequest == null)
                {
                    releaseEditRequest.ReleaseId = id;
                    releaseEditRequest.SubmittedDate = DateTime.Now;
                    releaseEditRequest.SubmittingUser = await _userManager.GetUserAsync(User);
                    context.ReleaseEditRequests.Add(releaseEditRequest);
                    await context.SaveChangesAsync();
                }
                else
                {
                    MusicRaterUser user = await _userManager.GetUserAsync(User);
                    if (editRequest.SubmittingUserId == user.Id || await _userManager.IsInRoleAsync(user, "Administrator"))
                    {
                        editRequest.Title = releaseEditRequest.Title;
                        editRequest.ReleaseDay = releaseEditRequest.ReleaseDay;
                        editRequest.ReleaseMonth = releaseEditRequest.ReleaseMonth;
                        editRequest.ReleaseYear = releaseEditRequest.ReleaseYear;
                        editRequest.Type = releaseEditRequest.Type;
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
            ReleaseEditRequest editRequest = await context.ReleaseEditRequests
                .Include(er => er.SubmittingUser)
                .FirstOrDefaultAsync(er => er.ReleaseId == id);
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.Id == id);
            release.Title = editRequest.Title;
            release.ReleaseDay = editRequest.ReleaseDay;
            release.ReleaseMonth = editRequest.ReleaseMonth;
            release.ReleaseYear = editRequest.ReleaseYear;
            release.Type = editRequest.Type;

            MusicRaterUser user = await _userManager.GetUserAsync(User);
            editRequest.SubmittingUser.UserNotifications.Add(new UserNotification
            {

                Title = $"Your edit request for <a href='/Release/Entry/{editRequest.ReleaseId}'>{editRequest.Title}</a> has been approved!",
                SiteMessage = "",
                Date = DateTime.Now,
                RecipientUserId = editRequest.SubmittingUserId,
                SendingUser = user
            });
            editRequest.SubmittingUser.UnreadNotificationCount++;
            context.RemoveRange(editRequest.Comments);
            context.Remove(editRequest);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Entry), new { id });
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> DenyEdit(long id, [FromForm] string denyMessage)
        {
            ReleaseEditRequest editRequest = await context.ReleaseEditRequests
                .Include(er => er.SubmittingUser)
                .FirstOrDefaultAsync(er => er.ReleaseId == id);
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            editRequest.SubmittingUser.UserNotifications.Add(new UserNotification
            {

                Title = $"Your edit request for <a href = '/Release/Entry/{editRequest.ReleaseId}' >{ editRequest.Title}</ a > has been denied",
                SiteMessage = denyMessage != null ? denyMessage : "",
                Date = DateTime.Now,
                RecipientUserId = editRequest.SubmittingUserId,
                SendingUser = user
            });
            editRequest.SubmittingUser.UnreadNotificationCount++;
            context.RemoveRange(editRequest.Comments);
            context.Remove(editRequest);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Entry), new { id });
        }
        private bool CheckReleaseEditRequest(Release release, ReleaseEditRequest editRequest)
        {
            if (release.Title == editRequest.Title
                && release.ReleaseDay == editRequest.ReleaseDay
                && release.ReleaseMonth == editRequest.ReleaseMonth
                && release.ReleaseYear == editRequest.ReleaseYear
                && release.Type == editRequest.Type)
            {
                return false;
            }
            return true;
        }
        [Authorize(Roles = "Administrator")]
        public async Task <IActionResult> Edit(long id)
        {
            Release release = await context.Releases.Include(r => r.Artist).FirstOrDefaultAsync(r => r.Id == id);
            if (release == null)
            {
                return View("NotFound", "release");
            }
            return View("ReleaseEditor", new ReleaseViewModel {Release=release, Artist=release.Artist } );
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [FromForm] Release release)
        {
            Release oldRelease = await context.Releases.FirstOrDefaultAsync(r => r.Id == id);
            if (oldRelease == null)
            {
                return View("NotFound", "release");
            }
            if (ModelState.IsValid)
            {
                oldRelease.Title = release.Title;
                oldRelease.ReleaseDay = release.ReleaseDay;
                oldRelease.ReleaseMonth = release.ReleaseMonth;
                oldRelease.ReleaseYear = release.ReleaseYear;
                oldRelease.FormattedDate = FormattedDateTime.GetFormattedDate(release.ReleaseDay, release.ReleaseMonth, release.ReleaseYear);
                oldRelease.Type = release.Type;
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Entry), new { id = oldRelease.Id });
            }
            release.Id = oldRelease.Id;
            return View("ReleaseEditor", new ReleaseViewModel { Release = release });
        }

        [HttpPost]
        public async Task <IActionResult> Rate(long id, [FromForm] int rating)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            ReleaseRating alreadyRated = await context.ReleaseRating.FirstOrDefaultAsync(r => r.Release.Id == id && r.UserId == user.Id);
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.Id == id);
            if (release == null)
            {
                return View("NotFound", "release");
            }
            if (alreadyRated == null)
            {
                if(rating == 0)
                {
                    return RedirectToAction(nameof(Entry), new { id }); ;
                }

                ReleaseRating releaseRating = new ReleaseRating { 
                    Rating = rating,
                    RatingDate = DateTime.Now,
                    UserId = user.Id,
                    User = user,
                    ReleaseId = id,
                    Release = release
                };
                context.ReleaseRating.Add(releaseRating);
            }
            else
            {
                if (rating == 0)
                {
                    context.ReleaseRating.Remove(alreadyRated);
                }
                else
                {
                    alreadyRated.Rating = rating;
                }
            }

            ReleaseReview releaseReview = await context.ReleaseReviews.FirstOrDefaultAsync(r => r.ReleaseId == release.Id && r.UserId == user.Id);
            if (releaseReview != null)
            {
                releaseReview.ReleaseRating = rating;
            }

            await context.SaveChangesAsync();

            int numberOfRatings = await context.ReleaseRating.CountAsync(r => r.ReleaseId == id);
            var releaseRatings = context.ReleaseRating.Where(r => r.ReleaseId == id);
            double ratingAverage = 0;
            if (releaseRatings.Count() > 0)
            {
                ratingAverage = await releaseRatings.AverageAsync(r => r.Rating);
            }

            release.AverageRating = ratingAverage;
            release.NumberOfRatings = numberOfRatings;
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Entry), new { id });
        }

        [Authorize(Roles = "Administrator")]
        public async Task <IActionResult> Delete(long id)
        {
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.Id == id);
            if (release == null)
            {
                return View("NotFound", "release");
            }
            context.RemoveRange(context.ReleaseRating.Where(r => r.ReleaseId == id));
            context.Remove(release);
            await context.SaveChangesAsync();
            return RedirectToAction("Profile", "Artist",new { id = release.ArtistId });
        }

        public async Task <IActionResult> Genres(long id)
        {
            ReleaseViewModel releaseView = new ReleaseViewModel {
                Release = await context.Releases.Include(r => r.ReleaseGenres)
                    .ThenInclude(rg => rg.Genre)
                    .Include(r => r.ReleaseGenres)
                    .ThenInclude(rg => rg.GenreVotes)
                    .FirstOrDefaultAsync(r => r.Id == id),
                User = await _userManager.GetUserAsync(User)
            };
            if(releaseView.Release == null)
            {
                return View("NotFound", "release");
            }
            ICollection<Genre> genreList = context.Genres.OrderBy(a => a.Name).ToList();
            ViewBag.GenreList = genreList;
            return View(releaseView);
        }

        [HttpPost]
        public async Task <IActionResult> Genres(long id, [FromForm] string suggestedGenre)
        {
            Release release = await context.Releases.Include(r => r.ReleaseGenres)
                .ThenInclude(rg => rg.Genre)
                .Include(r => r.ReleaseGenres)
                .ThenInclude(rg => rg.GenreVotes)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (release == null)
            {
                return View("NotFound", "release");
            }
            Genre genre = await context.Genres.FirstOrDefaultAsync(g => g.Name == suggestedGenre);
            if(genre == null)
            {
                // TODO: provide error message to user
                return RedirectToAction(nameof(Genres), new { id }); ;
            }
            MusicRaterUser currentUser = await _userManager.GetUserAsync(User);
            bool genreAlreadySuggsted = false;
            
            foreach(ReleaseGenre releaseGenre in release.ReleaseGenres)
            {
                
                if (releaseGenre.Genre.Name == suggestedGenre)
                {
                    genreAlreadySuggsted = true;
                    GenreVote userVote = releaseGenre.GenreVotes.FirstOrDefault(g => g.User == currentUser);
                    if (userVote != null)
                    {
                        releaseGenre.GenreVotes.Remove(userVote);
                        //releaseGenre.GenreVotes.Remove(currentUser);
                        releaseGenre.GenreVoting--;
                        if(releaseGenre.GenreVoting < 1)
                        {
                            context.Remove(releaseGenre);
                        }
                    }
                    else
                    {
                        releaseGenre.GenreVotes.Add(new GenreVote { 
                            Id = releaseGenre.Id,
                            ReleaseGenre = releaseGenre,
                            UserId = currentUser.Id,
                            User = currentUser
                        });
                        releaseGenre.GenreVoting++;
                    }
                    break;
                }
            }
            if (!genreAlreadySuggsted)
            {
                ReleaseGenre newReleaseGenre = new ReleaseGenre
                {
                    Genre = genre,
                    GenreId = genre.Name,
                    Release = release,
                    ReleaseId = release.Id,
                    GenreVoting = 1,
                    ArtistId = release.ArtistId
                };
                newReleaseGenre.GenreVotes.Add(new GenreVote
                {
                    Id = newReleaseGenre.Id,
                    ReleaseGenre = newReleaseGenre,
                    UserId = currentUser.Id,
                    User = currentUser
                });
                context.ReleaseGenres.Add(newReleaseGenre);
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Genres), new { id });
        }
    }
}
