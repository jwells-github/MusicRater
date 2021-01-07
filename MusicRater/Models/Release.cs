using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public class Release
    {
        public long ReleaseID { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public  ReleaseType ReleaseType { get; set; }
    }

    public enum ReleaseType
    {
        Album,
        Compilation,
        Ep,
        Mixtape,
        Single,
        Live,
        DjMix,
        Bootleg,
    }
}
