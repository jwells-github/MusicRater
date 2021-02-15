using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
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
        public DateTime BirthDate { get; set; }
        public Country Country { get; set; }
        public string About { get; set; }
    }
}
