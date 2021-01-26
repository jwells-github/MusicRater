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
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<MusicRaterUser> _userManager;
        private MusicRaterContext context;

        public HomeController(ILogger<HomeController> logger,
            UserManager<MusicRaterUser> userManager,
            MusicRaterContext data)
        {
            _logger = logger;
            _userManager = userManager;
            context = data;
        }

        public async Task <IActionResult> Index()
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);

            DateTime today = DateTime.Today;
            DateTime thisMonth = new DateTime(today.Year, today.Month, 1);

            HomeViewModel homeViewModel = new HomeViewModel
            {
                user = user,
                RecentReleases = await context.Releases.Include(r => r.Artist).OrderByDescending(r => r.FormattedDate).Skip(0).Take(10).ToListAsync(),
                MonthlyTopReleases = await context.Releases.Include(r => r.Artist).Where(r => r.FormattedDate > thisMonth).OrderByDescending(r => r.AverageRating).Skip(0).Take(10).ToListAsync()
            };
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
