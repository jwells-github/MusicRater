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
    public class ArtistController : Controller
    {
        private readonly ILogger<ArtistController> _logger;
        private MusicRaterDbContext context;
        public ArtistController(ILogger<ArtistController> logger, MusicRaterDbContext data)
        {
            context = data;
            _logger = logger;
        }
        public  IActionResult Index()
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
            return View("ArtistEditor", new Artist());
        }

        public async Task <IActionResult> Profile(long id)
        {
            Artist artist = await context.Artists.FirstOrDefaultAsync(a => a.ArtistID == id);
            return View(artist);
        }


    }
}
