using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MusicRater.Models;
using MusicRater.Data;
using Microsoft.AspNetCore.Authorization;

namespace MusicRater.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ILogger<ArtistController> _logger;
        private MusicRaterContext context;
        public ArtistController(ILogger<ArtistController> logger, MusicRaterContext data)
        {
            context = data;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
           return View(context.Artists.OrderBy(a => a.Name));
        }

        public IActionResult New()
        {
            return View("ArtistEditor", new Artist());
        }

        [HttpPost]
        public async Task<IActionResult> New([FromForm] Artist artist)
        {
            if (ModelState.IsValid)
            {
                context.Artists.Add(artist);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Profile), new { id = artist.ArtistID });
            }
            return View("ArtistEditor", artist);
        }

        [AllowAnonymous]
        public async Task <IActionResult> Profile(long id)
        {
            Artist artist = await context.Artists.Include(a => a.Releases).FirstOrDefaultAsync(a => a.ArtistID == id);
            return View(artist);
        }

        [Authorize(Roles = "Administrator")]
        public async Task <IActionResult> Delete(long id)
        {
            Artist artist = await context.Artists.Include(a => a.Releases).FirstOrDefaultAsync(a => a.ArtistID == id);
            foreach (Release release in artist.Releases)
            {
                context.RemoveRange(release.UserReleaseRatings);
            }
            context.RemoveRange(artist.Releases);
            context.Remove(artist);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Artist");
        }
    }
}
