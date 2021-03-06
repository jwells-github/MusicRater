﻿using System;
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
            return View(await PaginatedList<ReleaseReview>.CreateAsync(context.ReleaseReviews
                .Include(r => r.Release)
                .ThenInclude(r => r.Artist)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReviewDate), pageNumber ?? 1, resultNumber));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Release(int id,int? pageNumber)
        {
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.Id == id);
            if (release == null)
            {
                return View("NotFound", "release");
            }
            int resultnumber = 25;
            return View("Index", await PaginatedList<ReleaseReview>.CreateAsync(context.ReleaseReviews
                .Where(r => r.ReleaseId == id)
                .Include(r => r.Release)
                .ThenInclude(r => r.Artist)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReviewDate), pageNumber ?? 1, resultnumber));
        }

        [HttpPost]
        public async Task<IActionResult> Submit(long id, [FromForm] string reviewTitle, [FromForm] string reviewText)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.Id == id);
            ReleaseRating releaseRating = await context.ReleaseRating.FirstOrDefaultAsync(r => r.Release.Id == id && r.UserId == user.Id);
            ReleaseReview releaseReview = await context.ReleaseReviews.FirstOrDefaultAsync(r => r.Release.Id == id && r.UserId == user.Id);
            if(release != null && reviewText != null)
            {
                if (releaseReview == null)
                {
                    releaseReview = new ReleaseReview
                    {
                        Title = reviewTitle,
                        ReviewText = reviewText,
                        User = user,
                        Release = release,
                        ReleaseId = release.Id,
                        ReviewDate = DateTime.Now
                    };
                    context.ReleaseReviews.Add(releaseReview);
                    release.NumberOfReviews = release.releaseReviews.Count();
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
            ReleaseReview releaseReview = await context.ReleaseReviews.FirstOrDefaultAsync(r => r.Id == id);
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
            return RedirectToAction("Entry", "Release", new { id = releaseReview.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, [FromForm] long releaseID)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.Id == releaseID);
            ReleaseReview releaseReview = await context.ReleaseReviews.FirstOrDefaultAsync(r => r.Id == id);
            if (releaseReview == null)
            {
                return View("NotFound", "review");
            }
            if (releaseReview.UserId == user.Id || await _userManager.IsInRoleAsync(user, UserRoleNames.
                )){
                context.Remove(releaseReview);
                release.NumberOfReviews--;
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Entry", "Release", new { id = releaseReview.ReleaseId });
        }
    }

}
