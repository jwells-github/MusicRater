using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicRater.Areas.Identity.Data;

namespace MusicRater.Models
{
    public class ReleaseViewModel
    {
        public Release Release { get; set; }
        public Artist Artist { get;set; }
        public ReleaseRating UserRating { get; set; }
        public ReleaseReview UserReview { get; set; }
        public ICollection<ReleaseReview> ReleaseReviews { get; set; }
        public ICollection<ReleaseGenre> ReleaseGenres { get; set; }
        public MusicRaterUser User { get; set; }
    }
}
