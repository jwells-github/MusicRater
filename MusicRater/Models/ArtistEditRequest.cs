using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public class ArtistEditRequest
    {
        public ArtistEditRequest()
        {
            this.Comments = new HashSet<ArtistEditComment>();
        }
        public long Id { get; set; }
        public long ArtistId { get; set; }
        public Artist Artist { get; set; }
        public string SubmittingUserId { get; set; }
        public MusicRaterUser SubmittingUser { get; set; }
        public string Name { get; set; }
        public bool IsSoloArtist { get; set; }
        public Country OriginCountry { get; set; }
        [Range(0, 31)]
        public int BirthDay { get; set; }
        [Range(0, 12)]
        public int BirthMonth { get; set; }
        public int BirthYear { get; set; }
        [Range(0, 31)]
        public int DeathDay { get; set; }
        [Range(0, 12)]
        public int DeathMonth { get; set; }
        public int DeathYear { get; set; }
        public DateTime SubmittedDate { get; set; }
        public ICollection<ArtistEditComment> Comments { get; set; }
    }
}
