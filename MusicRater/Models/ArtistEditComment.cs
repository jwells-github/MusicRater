using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public class ArtistEditComment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long ArtistEditRequestId { get; set; }
        public ArtistEditRequest ArtistEditRequest { get; set; }
        public string UserId { get; set; }
        public MusicRaterUser User { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
