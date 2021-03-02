using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicRater.Models
{
    public class ReleaseReview
    {
        public ReleaseReview()
        {
            this.UserVotes = new HashSet<MusicRaterUser>();
        }
        public long Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public MusicRaterUser User { get; set; }
        public string UserId { get; set; }
        public ICollection<MusicRaterUser> UserVotes { get; set; }
        public Release Release { get; set; }
        public long ReleaseId { get; set; }
        [Range(1, 10)]
        public int ReleaseRating { get; set; }
    }
}
