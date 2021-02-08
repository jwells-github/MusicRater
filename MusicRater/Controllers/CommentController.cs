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
    public class CommentController : Controller
    {
        private readonly ILogger<CommentController> _logger;
        private MusicRaterContext context;
        private UserManager<MusicRaterUser> _userManager;

        public CommentController(ILogger<CommentController> logger,
            MusicRaterContext data,
            UserManager<MusicRaterUser> userManager)
        {
            context = data;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Release(long id, [FromForm] ReleaseComment comment)
        {
            if (ModelState.IsValid)
            {
                MusicRaterUser user = await _userManager.GetUserAsync(User);
                Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
                if(release != null)
                {
                    comment.User = user;
                    comment.Release = release;
                    context.Comments.Add(comment);
                    await context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Entry", "Release", new { id});
        }


    }
}
