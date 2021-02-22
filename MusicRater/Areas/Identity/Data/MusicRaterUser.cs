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
            this.ReleaseComments = new HashSet<ReleaseComment>();
            this.ArtistEditComments = new HashSet<ArtistEditComment>();
            this.UserNotifications = new HashSet<UserNotification>();
        }
        [PersonalData]
        public ICollection<ReleaseRating> ReleaseRatings { get; set; }
        [PersonalData]
        public ICollection<ReleaseReview> ReleaseReviews { get; set; }
        [PersonalData]
        public ICollection<ReleaseComment> ReleaseComments { get; set; }
        public ICollection<ArtistEditComment> ArtistEditComments { get; set; }
        [PersonalData]
        public UserProfile UserProfile { get; set; }
        [PersonalData]
        public ICollection<UserNotification> UserNotifications { get; set; }
        public int UnreadNotificationCount { get; set; }
    }
}
