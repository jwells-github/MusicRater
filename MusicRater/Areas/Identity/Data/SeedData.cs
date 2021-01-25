﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicRater.Areas.Identity.Data;
using MusicRater.Models;

namespace MusicRater.Areas.Identity.Data
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>().HasData(
                new Artist {ArtistID = 1, Name = "The White Stripes"},
                new Artist { ArtistID = 2, Name = "Neutral Milk Hotel" }
            );
            modelBuilder.Entity<Release>().HasData(
                new Release {
                    ReleaseID = 1,
                    Title = "The White Stripes",
                    ReleaseDay = 15,
                    ReleaseMonth = 6,
                    ReleaseYear = 1999,
                    FormattedDate = FormattedDateTime.GetFormattedDate(15, 6, 1999),
                    Type = ReleaseType.Album,
                    ArtistID = 1
                },
                new Release
                {
                    ReleaseID = 2,
                    Title = "De Stijl",
                    ReleaseDay = 20,
                    ReleaseMonth = 6,
                    ReleaseYear = 2000,
                    FormattedDate = FormattedDateTime.GetFormattedDate(20, 6, 2000),
                    Type = ReleaseType.Album,
                    ArtistID = 1
                },
                new Release
                {
                    ReleaseID = 3,
                    Title = "White Blood Cells",
                    ReleaseDay = 3,
                    ReleaseMonth = 7,
                    ReleaseYear = 2001,
                    FormattedDate = FormattedDateTime.GetFormattedDate(3, 7, 2001),
                    Type = ReleaseType.Album,
                    ArtistID = 1
                },
                new Release
                {
                    ReleaseID = 4,
                    Title = "Elephant",
                    ReleaseDay = 1,
                    ReleaseMonth = 4,
                    ReleaseYear = 2003,
                    FormattedDate = FormattedDateTime.GetFormattedDate(1, 4, 2003),
                    Type = ReleaseType.Album,
                    ArtistID = 1
                },
                new Release
                {
                    ReleaseID = 5,
                    Title = "Get Behind Me Satan",
                    ReleaseDay = 7,
                    ReleaseMonth = 6,
                    ReleaseYear = 2005,
                    FormattedDate = FormattedDateTime.GetFormattedDate(7, 6, 2005),
                    Type = ReleaseType.Album,
                    ArtistID = 1
                },
                new Release
                {
                    ReleaseID = 6,
                    Title = "Icky Thump",
                    ReleaseDay = 19,
                    ReleaseMonth = 6,
                    ReleaseYear = 2007,
                    FormattedDate = FormattedDateTime.GetFormattedDate(19, 6, 2007),
                    Type = ReleaseType.Album,
                    ArtistID = 1
                },
                new Release
                {
                    ReleaseID = 7,
                    Title = "On Avery Island",
                    ReleaseDay = 26,
                    ReleaseMonth = 3,
                    ReleaseYear = 1996,
                    FormattedDate = FormattedDateTime.GetFormattedDate(26, 3, 1996),
                    Type = ReleaseType.Album,
                    ArtistID = 2
                },
                new Release
                {
                    ReleaseID = 8,
                    Title = "In the Aeroplane Over the Sea",
                    ReleaseDay = 10,
                    ReleaseMonth = 2,
                    ReleaseYear = 1998,
                    FormattedDate = FormattedDateTime.GetFormattedDate(10, 2, 1998),
                    Type = ReleaseType.Album,
                    ArtistID = 2
                }
            ) ;
        }
    }
}
