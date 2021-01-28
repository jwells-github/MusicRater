﻿using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MusicRater.Data;
using Microsoft.AspNetCore.Identity;
using MusicRater.Areas.Identity.Data;
using System;

namespace MusicRater.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app, UserManager<MusicRaterUser> userManager)
        {
            MusicRaterContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<MusicRaterContext>();
            DateTime now = DateTime.Now;

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (userManager.FindByNameAsync("TestUser").Result == null)
            {
                MusicRaterUser user = new MusicRaterUser { UserName = "TestUser" };
                userManager.CreateAsync(user, RandomString.Get());
                if (!context.Artists.Any())
                {
                    context.Artists.AddRange(
                        new Artist
                        {
                            Name = "The White Stripes",
                            OriginCountry = Country.US,
                            IsSoloArtist = false,
                            BirthDay = 14,
                            BirthMonth = 7,
                            BirthYear = 1997,
                            FormattedBirthDate = FormattedDateTime.GetFormattedDate(14, 7, 1997),
                            DeathDay = 2,
                            DeathMonth = 2,
                            DeathYear = 2011,
                            FormattedDeathDate = FormattedDateTime.GetFormattedDate(2, 2, 2011)
                        },
                        new Artist
                        {
                            Name = "Neutral Milk Hotel",
                            IsSoloArtist = false,
                            OriginCountry = Country.US,
                            BirthYear = 1989,
                            FormattedBirthDate = FormattedDateTime.GetFormattedDate(0, 0, 1997),
                            DeathYear = 1998,
                            FormattedDeathDate = FormattedDateTime.GetFormattedDate(0, 0, 1998)
                        },
                        new Artist
                        {
                            Name = "Protomartyr",
                            IsSoloArtist = false,
                            OriginCountry = Country.US,
                            BirthYear = 2008,
                            FormattedBirthDate = FormattedDateTime.GetFormattedDate(0, 0, 2008)
                        },
                        new Artist
                        {
                            Name = "Alex Cameron",
                            IsSoloArtist = true,
                            OriginCountry = Country.AU,
                            BirthDay = 11,
                            BirthMonth = 9,
                            BirthYear = 1990,
                            FormattedBirthDate = FormattedDateTime.GetFormattedDate(11, 9, 1990)
                        }
                    );
                }
                context.SaveChanges();
                if (!context.Releases.Any())
                {
                    context.Releases.AddRange(
                        // Artist = The White Stripes
                        new Release
                        {
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
                            Title = "Icky Thump",
                            ReleaseDay = 19,
                            ReleaseMonth = 6,
                            ReleaseYear = 2007,
                            FormattedDate = FormattedDateTime.GetFormattedDate(19, 6, 2007),
                            Type = ReleaseType.Album,
                            ArtistID = 1
                        },
                        // Artist = Neutral Milk Hotel
                        new Release
                        {
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
                            Title = "In the Aeroplane Over the Sea",
                            ReleaseDay = 10,
                            ReleaseMonth = 2,
                            ReleaseYear = 1998,
                            FormattedDate = FormattedDateTime.GetFormattedDate(10, 2, 1998),
                            Type = ReleaseType.Album,
                            ArtistID = 2
                        },
                        // Artist = Protomartyr
                        new Release
                        {
                            Title = "No Passion All Technique",
                            ReleaseDay = 8,
                            ReleaseMonth = 10,
                            ReleaseYear = 2012,
                            FormattedDate = FormattedDateTime.GetFormattedDate(8,10,2012),
                            Type = ReleaseType.Album,
                            ArtistID = 3
                        },
                        new Release
                        {
                            Title = "Under Color of Official Right",
                            ReleaseDay = 8,
                            ReleaseMonth = 4,
                            ReleaseYear = 2014,
                            FormattedDate = FormattedDateTime.GetFormattedDate(8, 4, 2014),
                            Type = ReleaseType.Album,
                            ArtistID = 3
                        },
                        new Release
                        {
                            Title = "The Agent Intellect",
                            ReleaseDay = 9,
                            ReleaseMonth = 10,
                            ReleaseYear = 2015,
                            FormattedDate = FormattedDateTime.GetFormattedDate(9, 10, 2015),
                            Type = ReleaseType.Album,
                            ArtistID = 3
                        },
                        new Release
                        {
                            Title = "Relatives in Descent",
                            ReleaseDay = 29,
                            ReleaseMonth = 9,
                            ReleaseYear = 2017,
                            FormattedDate = FormattedDateTime.GetFormattedDate(29, 9, 2017),
                            Type = ReleaseType.Album,
                            ArtistID = 3
                        },
                        new Release
                        {
                            Title = "Ultimate Success Today",
                            ReleaseDay = 17,
                            ReleaseMonth = 07,
                            ReleaseYear = 2020,
                            FormattedDate = FormattedDateTime.GetFormattedDate(17, 7, 2020),
                            Type = ReleaseType.Album,
                            ArtistID = 3
                        },
                        new Release
                        {
                            Title = "Dredging the Grotto",
                            ReleaseMonth = 4,
                            ReleaseYear = 2015,
                            FormattedDate = FormattedDateTime.GetFormattedDate(0, 4, 2015),
                            Type = ReleaseType.Live,
                            ArtistID = 3
                        },
                        new Release
                        {
                            Title = "Security by Shadow",
                            ReleaseMonth = 12,
                            ReleaseYear = 2020,
                            FormattedDate = FormattedDateTime.GetFormattedDate(0, 12, 2020),
                            Type = ReleaseType.Live,
                            ArtistID = 3
                        },
                        new Release
                        {
                            Title = "Dreads 85 84",
                            ReleaseYear = 2012,
                            FormattedDate = FormattedDateTime.GetFormattedDate(0, 0, 2012),
                            Type = ReleaseType.Ep,
                            ArtistID = 3
                        },
                        new Release
                        {
                            Title = "Colpi Proibiti",
                            ReleaseDay = 3,
                            ReleaseMonth = 9,
                            ReleaseYear = 2012,
                            FormattedDate = FormattedDateTime.GetFormattedDate(3, 9, 2012),
                            Type = ReleaseType.Ep,
                            ArtistID = 3
                        },
                        new Release
                        {
                            Title = "Consolation E.P.",
                            ReleaseDay = 15,
                            ReleaseMonth = 6,
                            ReleaseYear = 2018,
                            FormattedDate = FormattedDateTime.GetFormattedDate(15, 6, 2018),
                            Type = ReleaseType.Ep,
                            ArtistID = 3
                        },
                        new Release
                        {
                            Title = "Scum, Rise!",
                            ReleaseYear = 2014,
                            FormattedDate = FormattedDateTime.GetFormattedDate(0, 0, 2014),
                            Type = ReleaseType.Single,
                            ArtistID = 3
                        },
                        // Artist = Alex Cameron
                        new Release
                        {
                            Title = "Jumping the Shark",
                            ReleaseYear = 2013,
                            FormattedDate = FormattedDateTime.GetFormattedDate(0, 0, 2013),
                            Type = ReleaseType.Album,
                            ArtistID = 4
                        },
                        new Release
                        {
                            Title = "Forced Witness",
                            ReleaseDay = 8,
                            ReleaseMonth = 9,
                            ReleaseYear = 2017,
                            FormattedDate = FormattedDateTime.GetFormattedDate(8, 9, 2017),
                            Type = ReleaseType.Album,
                            ArtistID = 4
                        },
                        new Release
                        {
                            Title = "Miami Memory",
                            ReleaseDay = 13,
                            ReleaseMonth = 9,
                            ReleaseYear = 2019,
                            FormattedDate = FormattedDateTime.GetFormattedDate(13, 9, 2019),
                            Type = ReleaseType.Album,
                            ArtistID = 4
                        }
                    );
                }
                context.SaveChanges();
                if (!context.ReleaseReviews.Any())
                {
                    context.ReleaseReviews.AddRange(
                        new ReleaseReview
                        {
                            Title = "This is a test review",
                            ReviewText = "This is some test text for my test review. " +
                                "This is not a real review. This is just an example of " +
                                "what a review could be. I am going to now list some of " +
                                "my favourite fruits in order to increase the length of " +
                                "this review. 1. Pears 2. Apples 3.Bananas",
                            ReviewDate = now,
                            UserID = user.Id,
                            ReleaseID = 1
                        },
                        new ReleaseReview
                        {
                            Title = "This is a much longer test review with a really long, " +
                            "a criminally long title. I mean what would you even do with a title " +
                            "this long? I'm already bored of this review and I haven't even gotten" +
                            " to the actual content of the review yet",
                            ReviewText = "And now, the moment you have all been waiting for. Radiohead with " +
                            "their breakout single Creep! \n\n" +
                            "When you were here before Couldn't look you in the eye " +
                            "You're just like an angel Your skin makes me cry You float" +
                            " like a feather In a beautiful world I wish I was special " +
                            "You're so special But I'm a creep I'm a weirdo What the" +
                            " hell am I doing here? I don't belong here  I don't care if it hurts" +
                            " I wanna have control I want a perfect body I want a perfect soul I want " +
                            "you to notice When I'm not around You're so special I wish I was special" +
                            "But I'm a creep I'm a weirdo What the hell am I doing " +
                            "here? I don't belong here Oh, oh  She's running out the door" +
                            " She's running out She run, run, run, run Run " +
                            " Whatever makes you happy Whatever you want You're so " +
                            "special I wish I was special But I'm a creep I'm a weirdo What the" +
                            " hell am I doing here? I don't belong here I don't belong here \n\n" +
                            "Encore!!!! \n\n" +
                            "When you were here before Couldn't look you in the eye " +
                            "You're just like an angel Your skin makes me cry You float" +
                            " like a feather In a beautiful world I wish I was special " +
                            "You're so special But I'm a creep I'm a weirdo What the" +
                            " hell am I doing here? I don't belong here  I don't care if it hurts" +
                            " I wanna have control I want a perfect body I want a perfect soul I want " +
                            "you to notice When I'm not around You're so special I wish I was special" +
                            " But I'm a creep I'm a weirdo What the hell am I doing " +
                            "here? I don't belong here Oh, oh  She's running out the door" +
                            " She's running out She run, run, run, run Run " +
                            " Whatever makes you happy Whatever you want You're so " +
                            "special I wish I was special But I'm a creep I'm a weirdo What the" +
                            " hell am I doing here? I don't belong here I don't belong here",
                            ReviewDate = now,
                            UserID = user.Id,
                            ReleaseID = 2
                        }
                     );
                }
                if (!context.Genres.Any()){
                    context.AddRange(
                        new Genre
                        {
                            Name = "Ambient",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Blues",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Classical Music",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Country",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Dance",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Electronic",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Experimental",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Folk",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Field Recordings",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Jazz",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Metal",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Pop",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Punk",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "R&B",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Rock",
                            Description = ""
                        },
                        new Genre
                        {
                            Name = "Ska",
                            Description = ""
                        }
                    );
                }
                context.SaveChanges();
            }
        }
    }
}

