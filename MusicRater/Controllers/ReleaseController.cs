using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == artistID);
            ReleaseViewModel releaseView = new ReleaseViewModel { Artist = artist};
            return View("ReleaseEditor", releaseView);
        }

        [HttpPost]
        public async Task<IActionResult> New(long artistID, [FromForm] Release release)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == artistID);
            release.Artist = artist;
            release.ArtistID = artist.ArtistID;
            release.FormattedDate = FormattedDateTime.GetFormattedDate(release.ReleaseDay, release.ReleaseMonth, release.ReleaseYear);
            if (ModelState.IsValid)
            {
                context.Releases.Add(release);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Entry), new { id = release.ReleaseID });
            }
            return View("ReleaseEditor", new ReleaseViewModel(release));
        }

        [AllowAnonymous]
        public async Task <IActionResult> Entry(long id)
        {

            Release release = await context.Releases
                .Include(r => r.Artist)
                .Include(r => r.ReleaseGenres)
                .ThenInclude(rg => rg.Genre)
                .FirstOrDefaultAsync(r => r.ReleaseID == id);
            ReleaseRating ratings = await context.ReleaseRating.FirstOrDefaultAsync(r => r.ReleaseID == id);
            
            ReleaseViewModel releaseView = new ReleaseViewModel(release);
            releaseView.ReleaseReviews = await context.ReleaseReviews.Include(r=> r.User).Where(r => r.ReleaseID == release.ReleaseID).ToListAsync();
            releaseView.NumberOfRatings = release.NumberOfRatings;
            releaseView.AverageRating = release.AverageRating;

            if (User.Identity.IsAuthenticated)
            {
                MusicRaterUser user = await _userManager.GetUserAsync(User);
                ViewBag.User = user;
                ReleaseRating releaseRating = await context.ReleaseRating.FirstOrDefaultAsync(r => r.ReleaseID == id && r.UserID == user.Id);
                ReleaseReview releaseReview = await context.ReleaseReviews.FirstOrDefaultAsync(r => r.ReleaseID == id && r.UserID == user.Id);
                if(releaseRating != null)
                {
                    releaseView.UserRating = releaseRating; 
                }
                if(releaseReview != null)
                {
                    releaseView.UserReview = releaseReview;
                }
            }

            return View(releaseView);
        }

        public async Task <IActionResult> Rate(long id, [FromForm] int rating)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            ReleaseRating alreadyRated = await context.ReleaseRating.FirstOrDefaultAsync(r => r.Release.ReleaseID == id && r.UserID == user.Id);
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
            if (alreadyRated == null)
            {
                
                ReleaseRating releaseRating = new ReleaseRating { 
                    Rating = rating,
                    RatingDate = DateTime.Now,
                    UserID = user.Id,
                    User = user,
                    ReleaseID = id,
                    Release = release
                };
                context.ReleaseRating.Add(releaseRating);
            }
            else
            {
                alreadyRated.Rating = rating;
            }

            await context.SaveChangesAsync();

            int numberOfRatings = await context.ReleaseRating.CountAsync(r => r.ReleaseID == id);
            double ratingAverage = await context.ReleaseRating.Where(r => r.ReleaseID == id).AverageAsync(r => r.Rating);

            release.AverageRating = ratingAverage;
            release.NumberOfRatings = numberOfRatings;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Entry), new { id });
        }

        [Authorize(Roles = "Administrator")]
        public async Task <IActionResult> Delete(long id)
        {
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
            context.RemoveRange(context.ReleaseRating.Where(r => r.ReleaseID == id));
            context.Remove(release);
            await context.SaveChangesAsync();
            return RedirectToAction("Profile", "Artist",new { id = release.ArtistID });
        }

        public async Task <IActionResult> Genres(long id)
        {
            Release release = await context.Releases.Include(r => r.ReleaseGenres)
                .ThenInclude(rg => rg.Genre)
                .FirstOrDefaultAsync(r => r.ReleaseID == id);
            ReleaseViewModel releaseView = new ReleaseViewModel(release);
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            ViewBag.User = user;
            ICollection<Genre> genreList = context.Genres.OrderBy(a => a.Name).ToList();
            ViewBag.GenreList = genreList;
            return View(releaseView);
        }

        [HttpPost]
        public async Task <IActionResult> Genres(long id, [FromForm] string suggestedGenre)
        {
            Release release = await context.Releases.Include(r => r.ReleaseGenres)
                .ThenInclude(rg => rg.Genre)
                .FirstOrDefaultAsync(r => r.ReleaseID == id);
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
                if(releaseGenre.Genre.Name == suggestedGenre)
                {
                    genreAlreadySuggsted = true;
                    if (releaseGenre.UserVotes.Contains(currentUser))
                    {
                        releaseGenre.UserVotes.Remove(currentUser);
                        releaseGenre.GenreVoting--;
                    }
                    else
                    {
                        releaseGenre.UserVotes.Add(currentUser);
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
                    GenreID = genre.Name,
                    Release = release,
                    ReleaseID = release.ReleaseID,
                    GenreVoting = 1,
                    ArtistID = release.ArtistID
                };
                newReleaseGenre.UserVotes.Add(currentUser);
                context.ReleaseGenres.Add(newReleaseGenre);
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Genres), new { id });
        }
    }
}
