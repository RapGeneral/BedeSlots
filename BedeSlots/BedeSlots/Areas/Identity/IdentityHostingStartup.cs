using System;
using BedeSlots.Areas.Identity.Data;
using BedeSlots.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(BedeSlots.Areas.Identity.IdentityHostingStartup))]
namespace BedeSlots.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<BedeSlotsContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("BedeSlotsContextConnection")));

                services.AddDefaultIdentity<BedeSlotsUser>()
                    .AddEntityFrameworkStores<BedeSlotsContext>();
            });
        }
    }
}