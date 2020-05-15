using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using tienda_web.Areas.Identity.Data;
using tienda_web.Data;

[assembly: HostingStartup(typeof(tienda_web.Areas.Identity.IdentityHostingStartup))]
namespace tienda_web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<tienda_webAuthDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("tienda_webAuthDbContextConnection")));

                services.AddDefaultIdentity<tienda_webUsers>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<tienda_webAuthDbContext>();
            });
        }
    }
}