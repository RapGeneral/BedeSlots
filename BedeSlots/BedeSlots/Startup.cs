using AutoMapper;
using BedeSlots.DataContext;
using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.MappingProvider;
using BedeSlots.GlobalData.Providers;
using BedeSlots.Services;
using BedeSlots.Services.Contracts;
using BedeSlots.Services.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using BedeSlots.Utilities.Extentions;

namespace BedeSlots
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<BedeDBContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("MyDbConnection")));

            services.BuildServiceProvider().GetService<BedeDBContext>().Database.Migrate();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<BedeDBContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUserManager<>), typeof(UserManagerWrapper<>));
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ITransactionServices, TransactionServices>();
            services.AddScoped<IMappingProvider, MappingProvider>();
            services.AddScoped<ICurrencyServices, CurrencyServices>();
            services.AddScoped<IBankDetailsServices, BankDetailsServices>();
            services.AddScoped<ISlotGamesServices, SlotGamesServices>();
            services.AddScoped<IUserBankDetailsServices, UserBankDetailsServices>();
            services.AddScoped<IDateTimeWrapper, DateTimeWrapper>();

            services.AddMemoryCache();
            services.AddAutoMapper();
        
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Index");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseNotFoundExceptionHandler();

            app.UseMvc(routes =>
            {
				routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
