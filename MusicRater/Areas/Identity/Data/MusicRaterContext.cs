﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicRater.Areas.Identity.Data;
using MusicRater.Models;

namespace MusicRater.Data
{
    public class MusicRaterContext : IdentityDbContext<MusicRaterUser>
    {
        public MusicRaterContext(DbContextOptions<MusicRaterContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MusicRaterUser>(
                typeBuilder =>
                {
                    typeBuilder.HasMany(user => user.ReleaseRatings)
                    .WithOne(rating => rating.User)
                    .HasForeignKey(rating => rating.UserID)
                    .IsRequired();
                });

            builder.Entity<ReleaseRating>(
                typeBuilder =>
                {
                    typeBuilder.HasOne(rating => rating.User)
                    .WithMany(user => user.ReleaseRatings)
                    .HasForeignKey(rating => rating.UserID)
                    .IsRequired();
                });


            builder.Entity<ReleaseGenre>()
                .HasKey(rg => new { rg.GenreID, rg.ReleaseID });
            builder.Entity<ReleaseGenre>()
                .HasOne(rg => rg.Genre)
                .WithMany(g => g.ReleaseGenres)
                .HasForeignKey(rg => rg.GenreID);
            builder.Entity<ReleaseGenre>()
                .HasOne(rg => rg.Release)
                .WithMany(r => r.ReleaseGenres)
                .HasForeignKey(rg => rg.ReleaseID);
        }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<ReleaseRating> ReleaseRating { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ReleaseGenre> ReleaseGenres{ get; set; }
    }
}
