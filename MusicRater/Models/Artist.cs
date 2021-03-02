using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicRater.Models
{
    public class Artist
    {
        public Artist()
        {
            this.Releases = new HashSet<Release>();
        }
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsSoloArtist { get; set; }
        public Country OriginCountry { get; set; }
        [Range(0, 31)]
        public int BirthDay { get; set; }
        [Range(0, 12)]
        public int BirthMonth { get; set; }
        public int BirthYear { get; set; }
        public DateTime FormattedBirthDate { get; set; }
        [Range(0, 31)]
        public int DeathDay { get; set; }
        [Range(0, 12)]
        public int DeathMonth { get; set; }
        public int DeathYear { get; set; }
        public DateTime FormattedDeathDate { get; set; }
        public ICollection<Release> Releases { get; set; }
        public ArtistEditRequest ArtistEditRequest { get; set; }
    }
}
