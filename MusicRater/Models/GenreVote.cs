using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MusicRater.Models
{
    public class GenreVote
    {
        public long GenreVoteID { get; set; }
        public long ReleaseGenreID { get; set; }
        public ReleaseGenre ReleaseGenre { get; set; }
        public string UserID { get; set; }
        public MusicRaterUser User { get; set; }
    }
}
