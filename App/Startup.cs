using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application.Services;
using Application.Extensions;
using Application.Models;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Application.Options;

namespace Application
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Localization");


            services.AddControllersWithViews();
            services.AddRazorPages();
            services.Configure<EmailServiceOptions>(Configuration.GetSection("Email"));
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IGameInvitationService, GameInvitationService>();
            services.AddRouting();
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
                options => options.ResourcesPath = "Localization").AddDataAnnotationsLocalization();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)  // IHostingEnvironment is obsolete
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("CreateUser", context =>
            {
                var firstName = context.Request.Query["firstName"];
                var lastName = context.Request.Query["lastName"];
                var email = context.Request.Query["email"];
                var password = context.Request.Query["password"];
                var userService = context.RequestServices.
                GetService<IUserService>();
                userService.RegisterUser(new UserModel
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password
                });
                return context.Response.WriteAsync($"User {firstName} {lastName} has been successfully created.");
            });

            app.UseWebSockets();
            app.UseCommunicationMiddleware();
            var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            localizationOptions.RequestCultureProviders.Clear();
            localizationOptions.RequestCultureProviders.Add(new CultureProviderResolverService());

            app.UseRequestLocalization(localizationOptions);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });


            app.UseStatusCodePages("text/plain", "HTTP Error - Status Code: {0}");
        }

    }
}
