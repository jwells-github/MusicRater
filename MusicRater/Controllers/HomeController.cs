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

namespace MusicRater.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<MusicRaterUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<MusicRaterUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task <IActionResult> Index()
        {
            MusicRaterUser user = await _userManager.GetUserAsync(User);
            MusicRaterUser populatedUser = await _userManager.Users.Include(u => u.ReleaseRatings).FirstOrDefaultAsync(u => u.Id == user.Id); ;
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
