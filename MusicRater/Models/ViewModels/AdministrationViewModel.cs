using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models.ViewModels
{
    public class AdministrationViewModel
    {
        public ICollection<ArtistEditRequest> RecentArtistRequests { get; set; }
        public int ArtistRequestCount { get; set; }

        public ICollection<ReleaseEditRequest> RecentReleaseRequests { get; set; }
        public int ReleaseRequestCount { get; set; }
    }
}
