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
            this.ReleaseReviews = new HashSet<ReleaseReview>();
            this.Comments = new HashSet<ReleaseComment>();
        }
        [PersonalData]
        public ICollection<ReleaseRating> ReleaseRatings { get; set; }
        [PersonalData]
        public ICollection<ReleaseReview> ReleaseReviews { get; set; }
        [PersonalData]
        public ICollection<ReleaseComment> Comments { get; set; }
        [PersonalData]
        public UserProfile UserProfile { get; set; }
    }
}
