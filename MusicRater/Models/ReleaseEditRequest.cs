using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicRater.Models
{
    public class ReleaseEditRequest
    {
        public ReleaseEditRequest()
        {
            this.Comments = new HashSet<ReleaseEditComment>();
        }
        public long Id { get; set; }
        public long ReleaseId { get; set; }
        public Release Release { get; set; }
        public string SubmittingUserId { get; set; }
        public MusicRaterUser SubmittingUser { get; set; }
        public string Title { get; set; }
        [Range(0, 31)]
        public int ReleaseDay { get; set; }
        [Range(0, 12)]
        public int ReleaseMonth { get; set; }
        public int ReleaseYear { get; set; }
        public ReleaseType Type { get; set; }
        public DateTime SubmittedDate { get; set; }
        public ICollection<ReleaseEditComment> Comments { get; set; }
    }
}
