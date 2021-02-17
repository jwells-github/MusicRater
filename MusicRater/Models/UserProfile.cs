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
        public long Id { get; set; }
        public string UserId { get; set; }
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

        public int Age()
        {
            if(BirthYear <= 0)
            {
                return -1;
            }
            DateTime today = DateTime.Today;
            int age = today.Year - this.BirthYear;
            if(BirthMonth > 0)
            {
                if (BirthMonth > today.Month || (today.Month == BirthMonth && BirthDay > today.Day))
                {
                    age--;
                }
            }
            return age;
        }
    }
}
