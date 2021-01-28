using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public class Comment
    {
        public long CommentID { get; set; }
        public string Text { get; set; }
        public long ReleaseID { get; set; }
        public Release Release { get; set; }
        public string UserID { get; set; }
        public MusicRaterUser User { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
