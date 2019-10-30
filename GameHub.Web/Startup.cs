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
using Caching;
using GameHub.Games.BoardGames.ConnectFour;
using GameHub.Games.BoardGames.Chess;
using GameHub.Web.Services.Games.ConnectFourServices;

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

            services.AddSingleton<ICache<ConnectFour>, ConnectFourCache>();

            services.AddSingleton<ICache<Chess>, ChessCache>();

            services.AddTransient<IConnectFourServiceFactory, ConnectFourServiceFactory>();

            services.AddTransient<IChessServiceFactory, ChessServiceFactory>();

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
                //currently ids are "protected" when sent to front end, so cant just paste in another players id as it hasn't been protected.
                //that is what this middleware is doing.
                var playerIdExists = context.Request.Cookies.ContainsKey("GHPID");

                var playerId = playerIdExists ? context.Request.Cookies["GHPID"] : Guid.NewGuid().ToString();

                var protector = DataProtectionProvider.Create("Gamehub.Web").CreateProtector("CookieEncryptPlayerId");

                if (playerIdExists)
                {             
                    playerId = protector.Unprotect(playerId);
                }

                // todo: encode datetime expire with cookie to help date tampering.
                // todo: perhaps remove ids and just use unique temporary names chosen on page load. check if in use like any other username.
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

                routes.MapHub<ChessHub>("/chesshub");
            });

            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute(
                name: "spa-fallback",
                defaults: new { controller = "Home", action = "Index" });
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
