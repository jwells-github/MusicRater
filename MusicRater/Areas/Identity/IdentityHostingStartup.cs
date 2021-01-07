using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicRater.Areas.Identity.Data;
using MusicRater.Data;

[assembly: HostingStartup(typeof(MusicRater.Areas.Identity.IdentityHostingStartup))]
namespace MusicRater.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<MusicRaterContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MusicRaterContextConnection")));

                services.AddDefaultIdentity<MusicRaterUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<MusicRaterContext>();

                services.Configure<IdentityOptions>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
                });
            });
        }
    }
}