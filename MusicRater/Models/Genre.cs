using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicRater.Models
{
    public class Genre
    {
        public Genre()
        {
            this.ReleaseGenres = new HashSet<ReleaseGenre>();
        }
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ReleaseGenre> ReleaseGenres { get; set; }
    }
}
