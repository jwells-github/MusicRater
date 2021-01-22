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

namespace MusicRater.Controllers
{
    public class UserController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private UserManager<MusicRaterUser> _userManager;

        public UserController(ILogger<UserController> logger, UserManager<MusicRaterUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task <IActionResult> Index(string username)
        {
            MusicRaterUser user =
                await _userManager.Users.Include(user => user.ReleaseRatings)
                .ThenInclude(rating => rating.Release)
                .ThenInclude(release => release.Artist)
                .FirstOrDefaultAsync(u => u.UserName == username);
            return View(user);
        }
    }
}
