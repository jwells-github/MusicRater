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

                services.AddDefaultIdentity<MusicRaterUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<MusicRaterContext>();
            });
        }
    }
}