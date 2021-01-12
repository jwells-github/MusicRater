using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using MusicRater.Areas.Identity.Data;
using MusicRater.Models;
using MusicRater.Data;

namespace MusicRater.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly ILogger<ReleaseController> _logger;
        private readonly UserManager<MusicRaterUser> _userManager;
        private MusicRaterContext context;


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

        [HttpPost]
        [Authorize]
        public async Task <IActionResult> Rate(long id, [FromForm] int rating)
        {
            Debug.WriteLine(rating);
            if (ModelState.IsValid)
            {
                MusicRaterUser user = await _userManager.GetUserAsync(User);
                Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
                ReleaseRating releaseRating = new ReleaseRating();
                releaseRating.Rating = rating;
                releaseRating.User = user;
                releaseRating.UserID = user.Id;
                releaseRating.Release = release;
                releaseRating.RatingDate = DateTime.Now;
                context.ReleaseRating.Add(releaseRating);
                user.ReleaseRatings.Add(releaseRating);
                await _userManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Entry), new { id });
        }
     }
}
