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
        public async Task<IActionResult> Profile(string username)
        {
            int displayRatings = 5;
            int displayReviews = 5;
            MusicRaterUser currentUser = await _userManager.GetUserAsync(User);
            MusicRaterUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if(user == null)
            {
                return View("NotFound", "User");
            }
            UserProfile userProfile = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == user.Id);
            if(userProfile == null)
            {
                userProfile = new UserProfile { UserId = user.Id, User = user };
                context.UserProfiles.Add(userProfile);
                context.SaveChanges();
            }
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel
            {
                User = user,
                UserProfile = userProfile,
                IsProfileOwner = (currentUser.Id == user.Id),
                RecentRatings = await context.ReleaseRating
                    .Where(r => r.UserId == user.Id)
                    .OrderBy(r => r.RatingDate)
                    .Skip(0)
                    .Take(displayRatings)
                    .Include(rating => rating.Release)
                    .ThenInclude(release => release.Artist)
                    .ToListAsync(),
                RecentReviews = await context.ReleaseReviews
                    .Where(r => r.UserId == user.Id)
                    .OrderBy(r=>r.ReviewDate)
                    .Skip(0)
                    .Take(displayReviews)
                    .Include(r => r.Release)
                    .ThenInclude(r=> r.Artist)
                    .ToListAsync(),
            };
            return View(userProfileViewModel);
        }

        public async Task<IActionResult> Notifications(int? pageNumber)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("NotFound", "User");
            }
            var notifications = context.UserNotifications
                .Where(un => un.RecipientUserId == user.Id)
                .Include(un => un.SendingUser);
            int resultNumber = 100;
            user.UnreadNotificationCount = 0;
            await context.SaveChangesAsync();
            return View(await PaginatedList<UserNotification>.CreateAsync(notifications.OrderBy(r => r.Date), pageNumber ?? 1, resultNumber));
        }

        public async Task<IActionResult> Ratings(string username, int? pageNumber)
        {
            MusicRaterUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return View("NotFound", "User");
            }
            var ratings = context.ReleaseRating
                .Where(r => r.UserId == user.Id)
                .Include(rating => rating.Release)
                .ThenInclude(release => release.Artist);
            int resultNumber = 100;
            return View(await PaginatedList<ReleaseRating>.CreateAsync(ratings.OrderBy(r=>r.RatingDate), pageNumber ?? 1, resultNumber));
        }
        public async Task<IActionResult> Reviews(string username, int? pageNumber)
        {
            MusicRaterUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return View("NotFound", "User");
            }
            var reviews = context.ReleaseReviews.Where(r => r.UserId == user.Id)
                    .Include(r => r.Release)
                    .ThenInclude(r => r.Artist);
            int resultNumber = 100;
            return View(await PaginatedList<ReleaseReview>.CreateAsync(reviews.OrderBy(r => r.ReviewDate), pageNumber ?? 1, resultNumber));
        }

        public async Task<IActionResult> EditProfile()
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("NotFound", "User");
            }
            UserProfile userProfile = await context.UserProfiles.Include(u=>u.User).FirstOrDefaultAsync(u=> u.UserId == user.Id);
            if (userProfile == null)
            {
                userProfile = new UserProfile { UserId = user.Id, User = user };
                context.UserProfiles.Add(userProfile);
                context.SaveChanges();
            }
            return View(userProfile);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile([FromForm] UserProfile profile)
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("NotFound", "User");
            }
            UserProfile userProfile = await context.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == user.Id);
            if (ModelState.IsValid)
            {
                userProfile.FirstName = profile.FirstName;
                userProfile.LastName = profile.LastName;
                userProfile.Gender = profile.Gender;
                userProfile.BirthDay = profile.BirthDay;
                userProfile.BirthMonth = profile.BirthMonth;
                userProfile.BirthYear = profile.BirthYear;
                userProfile.Country = profile.Country;
                userProfile.Biography = profile.Biography;
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Profile), new { id = user.UserName });
            }
            return View(userProfile);
        }
    }
}
