using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicRater.Areas.Identity.Data;

namespace MusicRater.Models.ViewModels
{
    public class HomeViewModel
    {
        public MusicRaterUser user { get; set; }
        public ICollection<Release> RecentReleases { get; set; }
        public ICollection<Release> MonthlyTopReleases { get; set; }
        public ICollection<ReleaseReview> MonthlyTopReviews { get; set; }

    }
}
