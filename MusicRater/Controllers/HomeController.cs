using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicRater.Models;
using Microsoft.AspNetCore.Identity;

using MusicRater.Areas.Identity.Data;
using MusicRater.Data;
using Microsoft.EntityFrameworkCore;

namespace MusicRater.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<MusicRaterUser> _userManager;
        private MusicRaterContext context;

        public HomeController(ILogger<HomeController> logger,
            UserManager<MusicRaterUser> userManager,
            MusicRaterContext data)
        {
            _logger = logger;
            _userManager = userManager;
            context = data;
        }

        public async  Task<IActionResult> Index()
        {
            MusicRaterUser currentUser = await _userManager.GetUserAsync(User);
            MusicRaterUser populatedUser = await _userManager.Users.Include(u => u.ReleaseRatings).SingleAsync(u => u.Id == currentUser.Id);
            return View(populatedUser);
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
