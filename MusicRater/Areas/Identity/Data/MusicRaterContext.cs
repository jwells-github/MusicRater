using System;
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
        public MusicRaterContext(DbContextOptions<MusicRaterContext> options) : base(options)
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
                    .HasForeignKey(rating => rating.UserId)
                    .IsRequired();

                    typeBuilder.HasMany(user => user.ReleaseReviews)
                    .WithOne(review => review.User)
                    .HasForeignKey(review => review.UserId)
                    .IsRequired();

                    typeBuilder.HasMany(user => user.UserNotifications)
                    .WithOne(un => un.RecipientUser)
                    .HasForeignKey(un => un.RecipientUserId)
                    .IsRequired();
                });
            builder.Entity<ReleaseReview>(
                typeBuilder =>
                {
                    typeBuilder.HasOne(review => review.User)
                    .WithMany(user => user.ReleaseReviews)
                    .HasForeignKey(rating => rating.UserId)
                    .IsRequired();
                });
            builder.Entity<ReleaseRating>(
                typeBuilder =>
                {
                    typeBuilder.HasOne(rating => rating.User)
                    .WithMany(user => user.ReleaseRatings)
                    .HasForeignKey(rating => rating.UserId)
                    .IsRequired();
                });

            builder.Entity<ReleaseGenre>()
                .HasKey(rg => new { rg.GenreId, rg.ReleaseId });
            builder.Entity<ReleaseGenre>()
                .HasOne(rg => rg.Genre)
                .WithMany(g => g.ReleaseGenres)
                .HasForeignKey(rg => rg.GenreId);
            builder.Entity<ReleaseGenre>()
                .HasOne(rg => rg.Release)
                .WithMany(r => r.ReleaseGenres)
                .HasForeignKey(rg => rg.ReleaseId);
            builder.Entity<UserNotification>(
                typeBuilder =>
                {
                    typeBuilder.HasOne(n => n.RecipientUser)
                    .WithMany(u => u.UserNotifications)
                    .HasForeignKey(nameof => nameof.RecipientUserId)
                    .IsRequired();
                    typeBuilder.HasOne(n => n.SendingUser);

                });
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Administrator", NormalizedName = "Administrator".ToUpper() },
                new IdentityRole { Name = "Moderator", NormalizedName = "Moderator".ToUpper() });
        }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistEditRequest> ArtistEditRequests { get; set; }
        public DbSet<ArtistEditComment> ArtistEditComments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<ReleaseEditRequest> ReleaseEditRequests { get; set; }
        public DbSet<ReleaseEditComment> ReleaseEditComments { get; set; }
        public DbSet<ReleaseRating> ReleaseRating { get; set; }
        public DbSet<ReleaseGenre> ReleaseGenres{ get; set; }
        public DbSet<ReleaseReview> ReleaseReviews { get; set; }
        public DbSet<ReleaseComment> ReleaseComments { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
    }
}
