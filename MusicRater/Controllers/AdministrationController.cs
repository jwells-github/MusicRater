using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicRater.Areas.Identity.Data;
using MusicRater.Data;
using MusicRater.Models;

namespace MusicRater.Controllers
{
    public class AdministrationController : Controller
    {
        private  IConfiguration _config;
        private UserManager<MusicRaterUser> _userManager;
        public AdministrationController(UserManager<MusicRaterUser> userManager, IConfiguration config)
        {
            _config = config;
            _userManager = userManager;
        }
        
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> New(string password)
        {
            var adminPassword = _config["AdminPassword"];
            if (password == adminPassword)
            {
                MusicRaterUser user = await _userManager.GetUserAsync(User);
                await _userManager.AddToRoleAsync(user, "Administrator");
                return RedirectToAction(nameof(Index));
            }

            return View();

        }
    }
}
