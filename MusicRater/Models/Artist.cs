using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public class Artist
    {
        public long ArtistID { get; set; }
        public string Name { get; set; }
        public ICollection<Release> Releases { get; set; }

    }
}
