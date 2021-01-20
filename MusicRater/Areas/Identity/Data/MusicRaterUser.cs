using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MusicRater.Models;


namespace MusicRater.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the MusicRaterUser class
    public class MusicRaterUser : IdentityUser
    {

        public MusicRaterUser()
        {
            this.ReleaseRatings = new HashSet<ReleaseRating>();
        }
        [PersonalData]
        public ICollection<ReleaseRating> ReleaseRatings { get; set; }
        public ICollection<ReleaseReview> ReleaseReviews { get; set; }
    }
}
