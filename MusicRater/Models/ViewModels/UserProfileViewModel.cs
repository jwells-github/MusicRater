using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public MusicRaterUser User { get; set; }
        public ICollection<ReleaseRating> RecentRatings { get; set; }
        public ICollection<ReleaseReview> RecentReviews { get; set; }   
        public UserProfile UserProfile { get; set; }
        public bool IsProfileOwner { get; set; }
    }
}
