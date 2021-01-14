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
        [Required]
        public string Title { get; set; }

        [Range(0,31)]
        public int ReleaseDay { get; set; }
        [Range(0, 12)]
        public int ReleaseMonth { get; set; }
        public int ReleaseYear { get; set; }
        public  ReleaseType Type { get; set; }

        public long ArtistID { get; set; }
        [Required]
        public Artist Artist { get; set; }

        public double AverageRating { get; set; }
        public int NumberOfRatings { get; set; }

        public ICollection<ReleaseRating> UserReleaseRatings { get; set; }

        public DateTime FormattedDate()
        {
            if(this.ReleaseDay == 0 && this.ReleaseMonth == 0 && this.ReleaseYear == 0)
            {
                return DateTime.MaxValue;
            }

            int releaseDay = this.ReleaseDay == 0 ? 1 : this.ReleaseDay;
            int releaseMonth = this.ReleaseMonth == 0 ? 1 : this.ReleaseMonth;
            int releaseYear = this.ReleaseYear == 0 ? 1 : this.ReleaseYear;
            return new DateTime(releaseYear, releaseMonth, releaseDay);
        }
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
