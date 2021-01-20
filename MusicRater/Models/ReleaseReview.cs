﻿using MusicRater.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MusicRater.Models
{
    public class ReleaseReview
    {
        public long ReleaseReviewID { get; set; }
        public string Title { get; set; }
        [Required]
        public string ReviewText { get; set; }
        public MusicRaterUser User { get; set; }
        public string UserID { get; set; }
        public Release Release { get; set; }
        public long ReleaseID { get; set; }

    }
}
