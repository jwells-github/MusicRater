using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicRater.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace MusicRater.Models
{
    public class ReleaseRating
    {
        public long ReleaseRatingID { get; set; }
        [Range(1,10)]
        public int Rating { get; set; }
        public DateTime RatingDate { get; set; }
        public string UserID { get; set; }
        public MusicRaterUser User { get; set; }
        public long ReleaseID { get; set; }
        public Release Release { get; set; }
    }
}
