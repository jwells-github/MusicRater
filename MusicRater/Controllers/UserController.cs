using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicRater.Models;
using MusicRater.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using MusicRater.Data;
using Microsoft.AspNetCore.Authorization;
using MusicRater.Models.ViewModels;

namespace MusicRater.Controllers
{
    public class UserController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private UserManager<MusicRaterUser> _userManager;
        private MusicRaterContext context;

        public UserController(ILogger<UserController> logger,
            UserManager<MusicRaterUser> userManager,
            MusicRaterContext data)
        {
            context = data;
            _logger = logger;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Profile(string id)
        {
            // TODO: User not found
            int displayRatings = 5;
            int displayReviews = 5;
            MusicRaterUser currentUser = await _userManager.GetUserAsync(User);
            MusicRaterUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == id);
            UserProfile userProfile = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserID == user.Id);
            if(userProfile == null)
            {
                userProfile = new UserProfile { UserID = user.Id, User = user };
                context.UserProfiles.Add(userProfile);
                context.SaveChanges();
            }
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel
            {
                User = user,
                UserProfile = userProfile,
                IsProfileOwner = (currentUser.Id == user.Id),
                RecentRatings = await context.ReleaseRating
                    .Where(r => r.UserID == user.Id)
                    .OrderBy(r => r.RatingDate)
                    .Skip(0)
                    .Take(displayRatings)
                    .Include(rating => rating.Release)
                    .ThenInclude(release => release.Artist)
                    .ToListAsync(),
                RecentReviews = await context.ReleaseReviews
                    .Where(r => r.UserID == user.Id)
                    .OrderBy(r=>r.ReviewDate)
                    .Skip(0)
                    .Take(displayReviews)
                    .Include(r => r.Release)
                    .ThenInclude(r=> r.Artist)
                    .ToListAsync(),
               
            };
            return View(userProfileViewModel);
        }

    }
}
