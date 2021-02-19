using System;
using Chat_Application.Areas.Identity.Data;
using Chat_Application.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Chat_Application.Areas.Identity.IdentityHostingStartup))]
namespace Chat_Application.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                //services.AddDbContext<IdentityDBContext>(options =>
                //    options.UseSqlite(
                //        context.Configuration.GetConnectionString("IdentityDBContextConnection")));

                services.AddDbContext<IdentityDBContext>();
                services.AddDefaultIdentity<ChatApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<IdentityDBContext>();
            });
        }
    }
}