using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public class ReleaseGenre
    {
        public long ReleaseGenreID { get; set; }
        public ReleaseGenre()
        {
            this.GenreVotes = new HashSet<GenreVote>();
        }
        public int GenreVoting { get; set; }
        public string GenreID { get; set; }  
        public Genre Genre { get; set; }
        public long ReleaseID { get; set; }
        public Release Release { get; set; }
        public long ArtistID { get; set; }

        public ICollection<GenreVote> GenreVotes { get; set; }
    }
}
