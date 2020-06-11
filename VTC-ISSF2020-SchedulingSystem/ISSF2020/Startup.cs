using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Identity;
using ISSF2020.Models;
using ISSF2020.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json;
using Newtonsoft.Json.Serialization;

namespace ISSF2020
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
            services.AddRazorPages();

            // requires using Microsoft.Extensions.Options
            // USER
            services.Configure<UserDatabaseSettings>(
                Configuration.GetSection(nameof(UserDatabaseSettings)));

            services.AddSingleton<IUserDatabaseSettings>(sp =>
            {
                var value = sp.GetRequiredService<IOptions<UserDatabaseSettings>>().Value;
                value.ConnectionString = $"mongodb+srv://{Configuration["DB_USER"]}:{Configuration["DB_PASS"]}@{Configuration["DB_CLUSTER"]}?retryWrites=true&w=majority";

                return value;
            });
            services.AddSingleton<UserService>();

            // SCHEDULE
            services.Configure<ScheduleDatabaseSettings>(
                Configuration.GetSection(nameof(ScheduleDatabaseSettings)));

            services.AddSingleton<IScheduleDatabaseSettings>(sp =>
            {
                var value = sp.GetRequiredService<IOptions<ScheduleDatabaseSettings>>().Value;
                value.ConnectionString = $"mongodb+srv://{Configuration["DB_USER"]}:{Configuration["DB_PASS"]}@{Configuration["DB_CLUSTER"]}?retryWrites=true&w=majority";

                return value;
            });
            services.AddSingleton<ScheduleService>();

            // REGIONAL WEATHER DATA
            services.Configure<RegionalWeatherDatabaseSettings>(
                Configuration.GetSection(nameof(RegionalWeatherDatabaseSettings)));

            services.AddSingleton<IRegionalWeatherDatabaseSettings>(sp =>
            {
                var value = sp.GetRequiredService<IOptions<RegionalWeatherDatabaseSettings>>().Value;
                value.ConnectionString = $"mongodb+srv://{Configuration["DB_USER"]}:{Configuration["DB_PASS"]}@{Configuration["DB_CLUSTER"]}?retryWrites=true&w=majority";

                return value;
            });
            services.AddSingleton<RegionalWeatherService>();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = System.TimeSpan.FromSeconds(1800);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("JwtSettings", options))
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => Configuration.Bind("CookieSettings", options));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
