using MusicRater.Areas.Identity.Data;
using System;

namespace MusicRater.Models
{
    public class ReleaseEditComment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long ReleaseEditRequestId { get; set; }
        public ReleaseEditRequest ReleaseEditRequest { get; set; }
        public string UserId { get; set; }
        public MusicRaterUser User { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
