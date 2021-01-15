using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicRater.Areas.Identity.Data;
namespace MusicRater.Models
{
    public class SearchViewModel
    {
        public List<Artist> Artists { get; set; }
        public List<Release> Releases { get; set; }
        public List<MusicRaterUser> Users { get; set; }
    }
}
