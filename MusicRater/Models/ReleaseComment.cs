using MusicRater.Areas.Identity.Data;
using System;

namespace MusicRater.Models
{
    public class ReleaseComment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long ReleaseId { get; set; }
        public Release Release { get; set; }
        public string UserId { get; set; }
        public MusicRaterUser User { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
