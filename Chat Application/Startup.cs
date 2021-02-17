using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Models;
using Microsoft.Extensions.Options;
using Chat_Application.Services;
using Chat_Application.SocketsManger;
using Chat_Application.Handlers;
using Chat_Application.Hubs;

namespace Chat_Application
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
          
                // requires using Microsoft.Extensions.Options
                services.Configure<DatabaseSettings>(
                    Configuration.GetSection(nameof(DatabaseSettings)));

                services.AddSingleton<IDatabaseSettings>(sp =>
                    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddSingleton<ConversationService>();
            services.AddSingleton<UserService>();

            services.AddSignalR();

            services.AddControllersWithViews();
            services.AddMvc();

            services.AddWebSocketManger();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseWebSockets();

            app.MapSockets("/ws", serviceProvider.GetService<WebSocketMessageHandler>());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "DashboardRoute",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/chathub");


            });

           

        }
    }
}
