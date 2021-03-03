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
using MusicRater.Models.ViewModels;

namespace MusicRater.Controllers
{
    [AllowAnonymous]
    public class TopReleasesController : Controller
    {
        private readonly ILogger<TopReleasesController> _logger;
        private MusicRaterContext context;
        private UserManager<MusicRaterUser> _userManager;

        public TopReleasesController(ILogger<TopReleasesController> logger,
            MusicRaterContext data,
            UserManager<MusicRaterUser> userManager)
        {
            context = data;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? pageNumber, int? searchYear,string searchGenre)
        {
            var releases = context.Releases
                .Include(r => r.Artist)
                .Include(r => r.ReleaseGenres.OrderBy(rg => rg.GenreVotes.Count()).Skip(0).Take(5))
                .ThenInclude(rg => rg.Genre)
                .Where(r => r.AverageRating > 0);
            if (searchYear != null && searchYear > 0)
            {
                releases = releases.Where(r => r.ReleaseYear == searchYear);
            }
            if(searchGenre != null && searchGenre.Length > 0) {
                releases = from r in releases
                           from rg in r.ReleaseGenres
                           where rg.GenreId == searchGenre
                           select r;
            }
            int resultNumber = 100;

            TopReleasesViewModel topReleasesViewModel = new TopReleasesViewModel
            {
                PaginatedList = await PaginatedList<Release>.CreateAsync(releases.OrderByDescending(r => r.AverageRating), pageNumber ?? 1, resultNumber),
                SearchYear = searchYear,
                SearchGenre = searchGenre,
                GenreList = context.Genres.OrderBy(a => a.Name).ToList()
            };
            return View(topReleasesViewModel);
        }
    }
}
