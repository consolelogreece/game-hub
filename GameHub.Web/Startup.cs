using GameHub.Web.SignalR.hubs.BoardGames;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Caching;
using GameHub.Games.BoardGames.ConnectFour;
using GameHub.Games.BoardGames.Chess;
using GameHub.Web.Services.Games.ConnectFourServices;
using GameHub.Web.Middleware;
using GameHub.Games.BoardGames.Battleships;

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
            
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSingleton<ICache<ConnectFour>, ConnectFourCache>();

            services.AddSingleton<ICache<Chess>, ChessCache>();

            services.AddSingleton<ICache<Battleships>, BattleshipsCache>();

            services.AddSingleton<UserCache>();

            services.AddTransient<IConnectFourServiceFactory, ConnectFourServiceFactory>();

            services.AddTransient<IChessServiceFactory, ChessServiceFactory>();

            services.AddTransient<IBattleshipsServiceFactory, BattleshipsServiceFactory>();

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

            app.UseAuthMiddleware();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ConnectFourHub>("/connectfourhub");

                routes.MapHub<ChessHub>("/chesshub");

                routes.MapHub<BattleshipsHub>("/battleshipshub");
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
