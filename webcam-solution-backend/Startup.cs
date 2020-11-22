using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using webcam_solution_backend.hubs;

namespace webcam_solution_backend
{
    public class Startup
    {
        readonly string ConfigOrigins = "AllowOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(ConfigOrigins,
                    builder => builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(ConfigOrigins);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalrWebrtc>("/signalrWebrtc");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("signalr hub start");
            });
        }
    }
}
