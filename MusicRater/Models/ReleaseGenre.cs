using System.Collections.Generic;

namespace MusicRater.Models
{
    public class ReleaseGenre
    {
        public long Id { get; set; }
        public ReleaseGenre()
        {
            this.GenreVotes = new HashSet<GenreVote>();
        }
        public int GenreVoting { get; set; }
        public string GenreId { get; set; }  
        public Genre Genre { get; set; }
        public long ReleaseId { get; set; }
        public Release Release { get; set; }
        public long ArtistId { get; set; }

        public ICollection<GenreVote> GenreVotes { get; set; }
    }
}
