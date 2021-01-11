using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MusicRater.Models;

namespace MusicRater.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly ILogger<ReleaseController> _logger;
        private MusicRaterDbContext context;

        public ReleaseController(ILogger<ReleaseController> logger, MusicRaterDbContext data)
        {
            context = data;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task <IActionResult> New(long artistID)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == artistID);
            ReleaseViewModel releaseView = new ReleaseViewModel();
            releaseView.Artist = artist;
            return View("ReleaseEditor", releaseView);
        }

        [HttpPost]
        public async Task<IActionResult> New(long artistID, [FromForm] Release release)
        {
            Debug.WriteLine("???");
            Debug.WriteLine(Json(release));
            Debug.WriteLine(release.Title);
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == artistID);
            release.Artist = artist;
            if (ModelState.IsValid)
            {
                context.Releases.Add(release);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Entry), new { id = release.ReleaseID });
            }
            return View("ReleaseEditor", new Release());
        }

        public async Task <IActionResult> Entry(long id)
        {
            Release release = await context.Releases.FirstOrDefaultAsync(r => r.ReleaseID == id);
            return View(release);
        }
    }
}
