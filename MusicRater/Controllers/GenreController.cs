using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicRater.Models;
using MusicRater.Data;
using Microsoft.AspNetCore.Authorization;

namespace MusicRater.Controllers
{
    public class GenreController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private MusicRaterContext context;

        public GenreController(ILogger<SearchController> logger,MusicRaterContext data)
        {
            context = data;
            _logger = logger;
        }

        public IActionResult Index(string? genreName = null)
        {
            if(genreName == null)
            {
                return View(context.Genres.OrderBy(a => a.Name));
            }
            else
            {
                return View("GenreDetail",context.Genres.FirstOrDefault(g => g.Name == genreName));    
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult New()
        {
            return View("GenreEditor", new Genre());
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> New([FromForm] Genre genre)
        {
            if (ModelState.IsValid)
            {
                context.Genres.Add(genre);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { genreName = genre.Name });
            }
            return View("GenreEditor", genre);
        }

    }
}
