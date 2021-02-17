using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MusicRater.Models
{
    public class GenreVote
    {
        public long Id { get; set; }
        public long ReleaseGenreId { get; set; }
        public ReleaseGenre ReleaseGenre { get; set; }
        public string UserId { get; set; }
        public MusicRaterUser User { get; set; }
    }
}
