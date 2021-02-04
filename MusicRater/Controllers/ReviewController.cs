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

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int resultNumber = 25;
            return View(await PaginatedList<ReleaseReview>.CreateAsync(context.ReleaseReviews.Include(r => r.Release)
                .ThenInclude(r => r.Artist)
                .OrderByDescending(r => r.ReviewDate), pageNumber ?? 1, resultNumber));
        }

        [HttpPost]
        public async Task<IActionResult> Submit(long id, [FromForm] string reviewTitle, [FromForm] string reviewText)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
            ReleaseRating releaseRating = await context.ReleaseRating.FirstOrDefaultAsync(r => r.Release.ReleaseID == id && r.UserID == user.Id);
            ReleaseReview releaseReview = await context.ReleaseReviews.FirstOrDefaultAsync(r => r.Release.ReleaseID == id && r.UserID == user.Id);
            if(release != null)
            {
                if (releaseReview == null)
                {
                    releaseReview = new ReleaseReview
                    {
                        Title = reviewTitle,
                        ReviewText = reviewText,
                        User = user,
                        Release = release,
                        ReleaseID = release.ReleaseID,
                        ReviewDate = DateTime.Now
                    };
                    context.ReleaseReviews.Add(releaseReview);
                    release.NumberOfReviews++;
                }
                else
                {
                    releaseReview.Title = reviewTitle;
                    releaseReview.ReviewText = reviewText;
                }
                if(releaseRating != null)
                {
                    releaseReview.ReleaseRating = releaseRating.Rating;
                }
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Entry","Release", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Vote(long id)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            ReleaseReview releaseReview = await context.ReleaseReviews.FirstOrDefaultAsync(r => r.ReleaseReviewID == id);
            if(releaseReview != null)
            {
                if (releaseReview.UserVotes.Contains(user))
                {
                    releaseReview.UserVotes.Remove(user);
                }
                else
                {
                    releaseReview.UserVotes.Add(user);
                }
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Entry", "Release", new { id = releaseReview.ReleaseID });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            ReleaseReview releaseReview = await context.ReleaseReviews.FirstOrDefaultAsync(r => r.ReleaseReviewID == id);
            if(releaseReview.UserID == user.Id || await _userManager.IsInRoleAsync(user, "Administrator")){
                context.Remove(releaseReview);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Entry", "Release", new { id = releaseReview.ReleaseID });
        }
    }

}
