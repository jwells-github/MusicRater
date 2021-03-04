using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicRater.Areas.Identity.Data;
using MusicRater.Data;
using MusicRater.Models.ViewModels;

namespace MusicRater.Controllers
{
    public class AdministrationController : Controller
    {
        private  IConfiguration _config;
        private MusicRaterContext context;
        private UserManager<MusicRaterUser> _userManager;
        private SignInManager<MusicRaterUser> _signInManager;
        public AdministrationController(UserManager<MusicRaterUser> userManager,
            MusicRaterContext data,
            IConfiguration config,
            SignInManager<MusicRaterUser> signInManager)
        {
            context = data;
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [Authorize(Roles = UserRoleNames.Administrator)]
        public async Task<IActionResult> Index()
        {
            int displayAmount = 5;
            AdministrationViewModel viewModel = new AdministrationViewModel
            {
                RecentArtistRequests = await context.ArtistEditRequests
                    .OrderByDescending(er => er.SubmittedDate)
                    .Skip(0)
                    .Take(displayAmount)
                    .Include(er=>er.Artist)
                    .Include(er=>er.SubmittingUser)
                    .ToListAsync(),
                RecentReleaseRequests = await context.ReleaseEditRequests
                    .OrderByDescending(er => er.SubmittedDate)
                    .Skip(0)
                    .Take(displayAmount)
                    .Include(er=>er.Release)
                        .ThenInclude(r=>r.Artist)
                    .Include(er=>er.SubmittingUser)
                    .ToListAsync(),
                ArtistRequestCount = context.ArtistEditRequests.Count(),
                ReleaseRequestCount = context.ReleaseEditRequests.Count(),
            };
            return View(viewModel);
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
                await _userManager.AddToRoleAsync(user, UserRoleNames.Administrator);
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [Authorize(Roles = UserRoleNames.Administrator)]
        public IActionResult ArtistEditRequests()
        {
            return View(context.ArtistEditRequests
                .Include(er=>er.Artist)
                .Include(er=>er.SubmittingUser)
                .OrderBy(er=>er.SubmittedDate));
        }
        [Authorize(Roles = UserRoleNames.Administrator)]
        public IActionResult ReleaseEditRequests()
        {
            return View(context.ReleaseEditRequests
                .Include(er=>er.Release)
                    .ThenInclude(r=>r.Artist)
                .Include(er => er.SubmittingUser)
                .OrderBy(er => er.SubmittedDate));
        }
    }
}
