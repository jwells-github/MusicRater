﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicRater.Data;

namespace MusicRater.Migrations
{
    [DbContext(typeof(MusicRaterContext))]
    partial class MusicRaterContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MusicRater.Areas.Identity.Data.MusicRaterUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ReleaseGenreGenreID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("ReleaseGenreReleaseID")
                        .HasColumnType("bigint");

                    b.Property<long?>("ReleaseReviewID")
                        .HasColumnType("bigint");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("ReleaseReviewID");

                    b.HasIndex("ReleaseGenreGenreID", "ReleaseGenreReleaseID");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MusicRater.Models.Artist", b =>
                {
                    b.Property<long>("ArtistID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArtistID");

                    b.ToTable("Artists");

                    b.HasData(
                        new
                        {
                            ArtistID = 1L,
                            Name = "The White Stripes"
                        },
                        new
                        {
                            ArtistID = 2L,
                            Name = "Neutral Milk Hotel"
                        });
                });

            modelBuilder.Entity("MusicRater.Models.Genre", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Name");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MusicRater.Models.Release", b =>
                {
                    b.Property<long>("ReleaseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("ArtistID")
                        .HasColumnType("bigint");

                    b.Property<double>("AverageRating")
                        .HasColumnType("float");

                    b.Property<int>("NumberOfRatings")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfReviews")
                        .HasColumnType("int");

                    b.Property<int>("ReleaseDay")
                        .HasColumnType("int");

                    b.Property<int>("ReleaseMonth")
                        .HasColumnType("int");

                    b.Property<int>("ReleaseYear")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ReleaseID");

                    b.HasIndex("ArtistID");

                    b.ToTable("Releases");

                    b.HasData(
                        new
                        {
                            ReleaseID = 1L,
                            ArtistID = 1L,
                            AverageRating = 0.0,
                            NumberOfRatings = 0,
                            NumberOfReviews = 0,
                            ReleaseDay = 15,
                            ReleaseMonth = 6,
                            ReleaseYear = 1999,
                            Title = "The White Stripes",
                            Type = 0
                        },
                        new
                        {
                            ReleaseID = 2L,
                            ArtistID = 1L,
                            AverageRating = 0.0,
                            NumberOfRatings = 0,
                            NumberOfReviews = 0,
                            ReleaseDay = 20,
                            ReleaseMonth = 6,
                            ReleaseYear = 2000,
                            Title = "De Stijl",
                            Type = 0
                        },
                        new
                        {
                            ReleaseID = 3L,
                            ArtistID = 1L,
                            AverageRating = 0.0,
                            NumberOfRatings = 0,
                            NumberOfReviews = 0,
                            ReleaseDay = 3,
                            ReleaseMonth = 7,
                            ReleaseYear = 2001,
                            Title = "White Blood Cells",
                            Type = 0
                        },
                        new
                        {
                            ReleaseID = 4L,
                            ArtistID = 1L,
                            AverageRating = 0.0,
                            NumberOfRatings = 0,
                            NumberOfReviews = 0,
                            ReleaseDay = 1,
                            ReleaseMonth = 4,
                            ReleaseYear = 2003,
                            Title = "Elephant",
                            Type = 0
                        },
                        new
                        {
                            ReleaseID = 5L,
                            ArtistID = 1L,
                            AverageRating = 0.0,
                            NumberOfRatings = 0,
                            NumberOfReviews = 0,
                            ReleaseDay = 7,
                            ReleaseMonth = 6,
                            ReleaseYear = 2005,
                            Title = "Get Behind Me Satan",
                            Type = 0
                        },
                        new
                        {
                            ReleaseID = 6L,
                            ArtistID = 1L,
                            AverageRating = 0.0,
                            NumberOfRatings = 0,
                            NumberOfReviews = 0,
                            ReleaseDay = 19,
                            ReleaseMonth = 6,
                            ReleaseYear = 2007,
                            Title = "Icky Thump",
                            Type = 0
                        },
                        new
                        {
                            ReleaseID = 7L,
                            ArtistID = 2L,
                            AverageRating = 0.0,
                            NumberOfRatings = 0,
                            NumberOfReviews = 0,
                            ReleaseDay = 26,
                            ReleaseMonth = 3,
                            ReleaseYear = 1996,
                            Title = "On Avery Island",
                            Type = 0
                        },
                        new
                        {
                            ReleaseID = 8L,
                            ArtistID = 2L,
                            AverageRating = 0.0,
                            NumberOfRatings = 0,
                            NumberOfReviews = 0,
                            ReleaseDay = 10,
                            ReleaseMonth = 2,
                            ReleaseYear = 1998,
                            Title = "In the Aeroplane Over the Sea",
                            Type = 0
                        });
                });

            modelBuilder.Entity("MusicRater.Models.ReleaseGenre", b =>
                {
                    b.Property<string>("GenreID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("ReleaseID")
                        .HasColumnType("bigint");

                    b.Property<long>("ArtistID")
                        .HasColumnType("bigint");

                    b.Property<int>("GenreVoting")
                        .HasColumnType("int");

                    b.HasKey("GenreID", "ReleaseID");

                    b.HasIndex("ReleaseID");

                    b.ToTable("ReleaseGenres");
                });

            modelBuilder.Entity("MusicRater.Models.ReleaseRating", b =>
                {
                    b.Property<long>("ReleaseRatingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime>("RatingDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("ReleaseID")
                        .HasColumnType("bigint");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReleaseRatingID");

                    b.HasIndex("ReleaseID");

                    b.HasIndex("UserID");

                    b.ToTable("ReleaseRating");
                });

            modelBuilder.Entity("MusicRater.Models.ReleaseReview", b =>
                {
                    b.Property<long>("ReleaseReviewID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("ReleaseID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ReviewDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReviewText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReleaseReviewID");

                    b.HasIndex("ReleaseID");

                    b.HasIndex("UserID");

                    b.ToTable("ReleaseReviews");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MusicRater.Areas.Identity.Data.MusicRaterUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MusicRater.Areas.Identity.Data.MusicRaterUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicRater.Areas.Identity.Data.MusicRaterUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MusicRater.Areas.Identity.Data.MusicRaterUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MusicRater.Areas.Identity.Data.MusicRaterUser", b =>
                {
                    b.HasOne("MusicRater.Models.ReleaseReview", null)
                        .WithMany("UserVotes")
                        .HasForeignKey("ReleaseReviewID");

                    b.HasOne("MusicRater.Models.ReleaseGenre", null)
                        .WithMany("UserVotes")
                        .HasForeignKey("ReleaseGenreGenreID", "ReleaseGenreReleaseID");
                });

            modelBuilder.Entity("MusicRater.Models.Release", b =>
                {
                    b.HasOne("MusicRater.Models.Artist", "Artist")
                        .WithMany("Releases")
                        .HasForeignKey("ArtistID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("MusicRater.Models.ReleaseGenre", b =>
                {
                    b.HasOne("MusicRater.Models.Genre", "Genre")
                        .WithMany("ReleaseGenres")
                        .HasForeignKey("GenreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicRater.Models.Release", "Release")
                        .WithMany("ReleaseGenres")
                        .HasForeignKey("ReleaseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Release");
                });

            modelBuilder.Entity("MusicRater.Models.ReleaseRating", b =>
                {
                    b.HasOne("MusicRater.Models.Release", "Release")
                        .WithMany("UserReleaseRatings")
                        .HasForeignKey("ReleaseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicRater.Areas.Identity.Data.MusicRaterUser", "User")
                        .WithMany("ReleaseRatings")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Release");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MusicRater.Models.ReleaseReview", b =>
                {
                    b.HasOne("MusicRater.Models.Release", "Release")
                        .WithMany("releaseReviews")
                        .HasForeignKey("ReleaseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicRater.Areas.Identity.Data.MusicRaterUser", "User")
                        .WithMany("ReleaseReviews")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Release");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MusicRater.Areas.Identity.Data.MusicRaterUser", b =>
                {
                    b.Navigation("ReleaseRatings");

                    b.Navigation("ReleaseReviews");
                });

            modelBuilder.Entity("MusicRater.Models.Artist", b =>
                {
                    b.Navigation("Releases");
                });

            modelBuilder.Entity("MusicRater.Models.Genre", b =>
                {
                    b.Navigation("ReleaseGenres");
                });

            modelBuilder.Entity("MusicRater.Models.Release", b =>
                {
                    b.Navigation("ReleaseGenres");

                    b.Navigation("releaseReviews");

                    b.Navigation("UserReleaseRatings");
                });

            modelBuilder.Entity("MusicRater.Models.ReleaseGenre", b =>
                {
                    b.Navigation("UserVotes");
                });

            modelBuilder.Entity("MusicRater.Models.ReleaseReview", b =>
                {
                    b.Navigation("UserVotes");
                });
#pragma warning restore 612, 618
        }
    }
}
