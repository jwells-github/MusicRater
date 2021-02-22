using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public class UserNotification
    {
        public long Id { get; set;}
        public string Title { get; set; }
        public string SiteMessage { get; set; }
        public string UserMessage { get; set; }
        public DateTime Date { get; set; }
        public MusicRaterUser RecipientUser { get; set; }
        public string RecipientUserId { get; set; }
        public MusicRaterUser SendingUser { get; set; }
       
    }
}

// Your edit request has been denid / approved
// New comment on your profile
// New friend request
// message from user
