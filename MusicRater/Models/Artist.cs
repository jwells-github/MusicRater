using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MusicRater.Models
{
    public class Artist
    {
        public Artist()
        {
            this.Releases = new HashSet<Release>();
        }
        public long ArtistID { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Release> Releases { get; set; }

    }
}
