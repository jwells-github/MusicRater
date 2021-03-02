using MusicRater.Areas.Identity.Data;

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
