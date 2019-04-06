using GameHub.Games.BoardGames.ConnectFour;
using GameHub.Web.SignalR.hubs.BoardGames;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Security.Cryptography;

namespace GameHub.Web
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSingleton<IConnectFour, ConnectFour>();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(builder => builder.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin());
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.Use((context, next) =>
            {
                var playerIdExists = context.Request.Cookies.ContainsKey("GHPID");

                var playerId = playerIdExists ? context.Request.Cookies["GHPID"] : Guid.NewGuid().ToString();

                var protector = DataProtectionProvider.Create("Gamehub.Web").CreateProtector("CookieEncryptPlayerId");

                if (playerIdExists)
                {             
                    playerId = protector.Unprotect(playerId);
                }

                // todo: encode datetime expire with cookie to help date tampering.
                // consider signed jwts or something

                context.Items.Add("GHPID", playerId);

                var protectedPlayerId = protector.Protect(playerId);

                context.Response.Cookies.Append("GHPID", protectedPlayerId, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(15),
                    SameSite = SameSiteMode.Strict
                });
                   
                return next();
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<ConnectFourHub>("/connectfourhub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
