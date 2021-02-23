using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
                Release release = await context.Releases.FirstOrDefaultAsync(r => r.Id == id);
                if(release != null && comment.Text != null)
                {
                    comment.User = user;
                    comment.Release = release;
                    comment.PostedDate = DateTime.Now;
                    context.ReleaseComments.Add(comment);
                    await context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Entry", "Release", new { id});
        }

        [HttpPost]
        public async Task<IActionResult> ArtistEditRequest(long id, [FromForm] ArtistEditComment comment)
        {
            ArtistEditRequest editRequest = await context.ArtistEditRequests
                .Include(er => er.SubmittingUser)
                .FirstOrDefaultAsync(er => er.Id == id);
            if (ModelState.IsValid)
            {
                MusicRaterUser user = await _userManager.GetUserAsync(User);
                
                if(editRequest != null && comment.Text != null)
                {
                    comment.User = user;
                    comment.ArtistEditRequest = editRequest;
                    comment.PostedDate = DateTime.Now;
                    context.ArtistEditComments.Add(comment);
                    if(user.Id != editRequest.SubmittingUserId)
                    {
                        editRequest.SubmittingUser.UserNotifications.Add(new UserNotification
                        {

                            Title = $"<a href='/User/Profile/{user.UserName}'>{user.UserName}</a> " +
                                $"posted a comment on your edit request for the profile of " +
                                $"<a href='/Artist/Profile/{editRequest.ArtistId}'>{editRequest.Name}</a>",
                            SiteMessage = "",
                            Date = DateTime.Now,
                            RecipientUserId = editRequest.SubmittingUserId,
                            SendingUser = user
                        });
                        editRequest.SubmittingUser.UnreadNotificationCount++;
                    }
                    await context.SaveChangesAsync();
                }
            }
            return RedirectToAction("EditRequest", "Artist", new { id = editRequest.ArtistId });
        }
    }
}
