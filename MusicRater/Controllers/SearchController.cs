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
    [AllowAnonymous]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private UserManager<MusicRaterUser> _userManager;
        private MusicRaterContext context;

        public SearchController(ILogger<SearchController> logger,
            UserManager<MusicRaterUser> userManager,
            MusicRaterContext data)
        {
            context = data;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task <IActionResult> Index(string searchTerm)
        {
            int resultNumber = 15;
            var artists = await context.Artists.Where(a => a.Name.Contains(searchTerm)).Take(resultNumber).ToListAsync();
            var releases = await context.Releases.Where(r => r.Title.Contains(searchTerm)).Take(resultNumber).ToListAsync();
            var users = await context.Users.Where(u => u.UserName.Contains(searchTerm)).Take(resultNumber).ToListAsync();
            SearchViewModel searchViewModel = new SearchViewModel
            {
                Artists =  artists,
                Releases =  releases,
                Users =  users
            };
            return View("allSearchResults", searchViewModel);
        }
 
        public async Task <IActionResult> Artist (string searchTerm, int? pageNumber)
        {
            int resultNumber = 50;
            return View("artistSearchResults", 
                await PaginatedList<Artist>.CreateAsync(
                    context.Artists.Where(a => a.Name.Contains(searchTerm)),
                    pageNumber ?? 1, resultNumber));
        }

        public async Task<IActionResult> Release (string searchTerm, int? pageNumber)
        {
            int resultNumber = 50;
            return View("releaseSearchResults",
                await PaginatedList<Release>.CreateAsync(
                    context.Releases.Where(r => r.Title.Contains(searchTerm)),
                    pageNumber ?? 1, resultNumber));
        }

        public async Task<IActionResult> User(string searchTerm, int? pageNumber)
        {
            int resultNumber = 50;
            return View("userSearchResults",
                await PaginatedList<MusicRaterUser>.CreateAsync(
                    context.Users.Where(u => u.UserName.Contains(searchTerm)),
                    pageNumber ?? 1, resultNumber));
        }
    }
}
