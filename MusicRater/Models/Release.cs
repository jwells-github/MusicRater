using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MusicRater.Models
{
    public class Release
    {
        public Release()
        {
            this.UserReleaseRatings = new HashSet<ReleaseRating>();
        }

        public long ReleaseID { get; set; }
        public string Title { get; set; }

        [Range(0,31)]
        public int ReleaseDay { get; set; }
        [Range(0, 12)]
        public int ReleaseMonth { get; set; }
        public int ReleaseYear { get; set; }
        public  ReleaseType Type { get; set; }
        public Artist Artist { get; set; }

        public ICollection<ReleaseRating> UserReleaseRatings { get; set; }
    }

    public enum ReleaseType
    {
        Album,
        Compilation,
        Ep,
        Mixtape,
        Single,
        Live,
        [Display(Name = "DJ Mix")]
        DjMix,
        Bootleg,
    }
}
