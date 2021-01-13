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

        public async Task <IActionResult> New(long artistID)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == artistID);
            ReleaseViewModel releaseView = new ReleaseViewModel();
            releaseView.Artist = artist;
            return View("ReleaseEditor", releaseView);
        }

        [HttpPost]
        public async Task<IActionResult> New(long artistID, [FromForm] Release release)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == artistID);
            release.Artist = artist;
            if (ModelState.IsValid)
            {
                context.Releases.Add(release);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Entry), new { id = release.ReleaseID });
            }
            return View("ReleaseEditor", new Release());
        }

        public async Task <IActionResult> Entry(long id)
        {
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
            return View(release);
        }

        [Authorize]
        public async Task <IActionResult> Rate(long id, [FromForm] int rating)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            ReleaseRating alreadyRated = await context.ReleaseRating.FirstOrDefaultAsync(r => r.Release.ReleaseID == id && r.UserID == user.Id);

            if(alreadyRated == null)
            {
                Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
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
            return RedirectToAction(nameof(Entry), new { id });
        }
    }
}
