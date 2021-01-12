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
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<ReleaseRating> ReleaseRating { get; set; }
    }
}
