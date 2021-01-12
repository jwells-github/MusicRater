using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task <IActionResult> Rate(long id, [FromForm] int rating)
        {
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            ReleaseRating releaseRating = new ReleaseRating();
            releaseRating.Rating = rating;
            releaseRating.RatingDate = DateTime.Now;
            releaseRating.UserID = user.Id;
            releaseRating.User = user;
            releaseRating.Release = release;
            context.ReleaseRating.Add(releaseRating);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Entry), new { id });
        }
    }
}
