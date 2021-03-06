﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicRater.Models
{
    public class Release
    {
        public Release()
        {
            this.releaseReviews = new HashSet<ReleaseReview>();
            this.UserReleaseRatings = new HashSet<ReleaseRating>();
            this.ReleaseGenres = new HashSet<ReleaseGenre>();
            this.Comments = new HashSet<ReleaseComment>();
            Random random = new Random();
            this.RGBAlbumCoverBlue = random.Next(256);
            this.RGBAlbumCoverRed = random.Next(256);
            this.RGBAlbumCoverGreen = random.Next(256);
        }
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Range(0,31)]
        public int ReleaseDay { get; set; }
        [Range(0, 12)]
        public int ReleaseMonth { get; set; }
        public int ReleaseYear { get; set; }
        public DateTime FormattedDate { get; set; }
        public  ReleaseType Type { get; set; }
        public long ArtistId { get; set; }
        public Artist Artist { get; set; }
        public double AverageRating { get; set; }
        public int NumberOfRatings { get; set; }
        public int NumberOfReviews { get; set; }
        public ICollection<ReleaseReview> releaseReviews { get; set; }
        public ICollection<ReleaseRating> UserReleaseRatings { get; set; }
        public ICollection<ReleaseGenre> ReleaseGenres { get; set; }
        public ICollection<ReleaseComment> Comments { get; set; }
        public int RGBAlbumCoverRed { get; set; }
        public int RGBAlbumCoverGreen { get; set; }
        public int RGBAlbumCoverBlue { get; set; }
        public string GetBackgroundCSS()
        {
            return $"background-color: rgb({this.RGBAlbumCoverRed},{this.RGBAlbumCoverGreen},{this.RGBAlbumCoverBlue});";
        }
    }

    public enum ReleaseType
    {
        Album,
        Compilation,
        Ep,
        Mixtape,
        Single,
        [Display(Name = "Live Album")]
        Live,
        [Display(Name = "DJ Mix")] DjMix,
        Bootleg,
    } 

}

