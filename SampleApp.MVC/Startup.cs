using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SampleApp.MVC.Messaging;
using SampleApp.MVC.Messaging.Consumers;
using SampleApp.MVC.Models;
using SampleApp.MVC.Repositories;
using SampleApp.MVC.Services;

namespace SampleApp.MVC
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
            services.AddOptions();

            var section = Configuration.GetSection("BaseSettings");
            services.Configure<BaseSettings>(section);

            var envSettings = new EnvironmentSettings
            {
                Rabbit =
                {
                    UserName = "guest",
                    Password = "guest",
                    VirtualHost = "/",
                    HostName = "localhost"
                }
            };

            services.AddSingleton(envSettings);

            services.AddHostedService<Worker>();

            services.AddSingleton<IConnectionManager, RabbitMqConnectionManager>();
            services.AddSingleton<IMessageHandler, SampleMessageHandler>();

            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            services.AddSingleton<IMongoDbSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddControllersWithViews();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Mock", StringComparison.InvariantCultureIgnoreCase))
                services.AddScoped(typeof(IMongoRepository<>), typeof(MockRepository<>));
            else
                services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            services.AddHttpClient<IWeatherService, WeatherService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["BaseUrl"]);
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
