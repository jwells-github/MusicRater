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
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task <IActionResult> New(long artistID)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == artistID);
            ReleaseViewModel releaseView = new ReleaseViewModel { Artist = artist};
            return View("ReleaseEditor", releaseView);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> New(long artistID, [FromForm] Release release)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == artistID);
            release.Artist = artist;
            release.ArtistID = artist.ArtistID;
            if (ModelState.IsValid)
            {
                context.Releases.Add(release);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Entry), new { id = release.ReleaseID });
            }
            return View("ReleaseEditor", new ReleaseViewModel(release));
        }

        public async Task <IActionResult> Entry(long id)
        {
            Release release = await context.Releases.Include(r => r.Artist).FirstOrDefaultAsync(r => r.ReleaseID == id);
            ReleaseRating ratings = await context.ReleaseRating.FirstOrDefaultAsync(r => r.ReleaseID == id);

            ReleaseViewModel releaseView = new ReleaseViewModel(release);
            releaseView.NumberOfRatings = release.NumberOfRatings;
            releaseView.AverageRating = release.AverageRating;

            if (User.Identity.IsAuthenticated)
            {
                MusicRaterUser user = await _userManager.GetUserAsync(User);
                ReleaseRating alreadyRated = await context.ReleaseRating.FirstOrDefaultAsync(r => r.ReleaseID == id && r.UserID == user.Id);
                if(alreadyRated != null)
                {
                    releaseView.UserRating = alreadyRated; 
                }
            }

            return View(releaseView);
        }

        [Authorize]
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

        [Authorize]
        public async Task <IActionResult> Delete(long id)
        {
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
            context.RemoveRange(context.ReleaseRating.Where(r => r.ReleaseID == id));
            context.Remove(release);
            await context.SaveChangesAsync();
            return RedirectToAction("Profile", "Artist",new { id = release.ArtistID });
        }
    }
}
