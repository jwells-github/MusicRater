using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models.ViewModels
{
    public class TopReleasesViewModel
    {
        public PaginatedList<Release> PaginatedList { get; set; }
        public string SearchGenre { get; set; }
        public int? SearchYear { get; set; }
        public ICollection<Genre> GenreList { get; set; }
    }
}
