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
    public class ReviewController : Controller
    {

        private readonly ILogger<ReviewController> _logger;
        private MusicRaterContext context;
        private UserManager<MusicRaterUser> _userManager;

        public ReviewController(ILogger<ReviewController> logger,
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
        [HttpPost]
        public async Task<IActionResult> Submit(long id, [FromForm] string reviewTitle, [FromForm] string reviewText)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
            ReleaseReview releaseReview = await context.ReleaseReviews.FirstOrDefaultAsync(r => r.Release.ReleaseID == id && r.UserID == user.Id);
            if (releaseReview == null)
            {
                releaseReview = new ReleaseReview
                {
                    Title = reviewTitle,
                    ReviewText = reviewText,
                    User = user,
                    Release = release,
                    ReleaseID = release.ReleaseID

                };
                context.ReleaseReviews.Add(releaseReview);
                release.NumberOfReviews++;
            }
            else
            {
                releaseReview.Title = reviewTitle;
                releaseReview.ReviewText = reviewText;
            }

            await context.SaveChangesAsync();
            return RedirectToAction("Entry","Release", new { id });
        }
    }
}
