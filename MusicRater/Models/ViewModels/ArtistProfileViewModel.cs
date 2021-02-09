using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models.ViewModels
{
    public class ArtistProfileViewModel
    {
        public Artist Artist { get; set; }
        public ICollection<Release> Albums { get; set; }
        public ICollection<Release> Compilations { get; set; }
        public ICollection<Release> Eps { get; set; }
        public ICollection<Release> Mixtapes { get; set; }
        public ICollection<Release> Singles { get; set; }
        public ICollection<Release> LiveAlbums { get; set; }
        public ICollection<Release> Bootlegs { get; set; }
        public ICollection<Release> DJMixes { get; set; }
        public bool IsAdmin { get; set; }
    }
}
