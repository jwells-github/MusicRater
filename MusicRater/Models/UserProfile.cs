using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public class UserProfile
    {
        public long UserProfileID { get; set; }
        public string UserID { get; set; }
        public MusicRaterUser User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        [Range(0, 31)]
        public int BirthDay { get; set; }
        [Range(0, 12)]
        public int BirthMonth { get; set; }
        public int BirthYear { get; set; }
        public Country Country { get; set; }
        public string Biography { get; set; }
    }
}
