using FakeXiecheng.Database;
using FakeXiecheng.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace FakeXiecheng
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setupAction => {
                setupAction.ReturnHttpNotAcceptable = true;
                //setupAction.OutputFormatters.Add(
                //    new XmlDataContractSerializerOutputFormatter()
                //);
            }).AddXmlDataContractSerializerFormatters();

            // Í¨¹ýEntity Framwork×¢²á²Ö¿â
            services.AddTransient<ITouristRouteRepository, TouristRouteRepository>();
            //services.AddSingleton<ITouristRouteRepository, MockTouristRouteRepository>();
            //services.AddScoped<ITouristRouteRepository, MockTouristRouteRepository>();
            services.AddDbContext<AppDbContext>(option =>
            {
                //option.UseSqlServer("server=localhost; Database=FakeXiechengDb; User Id=sa; Password=Jiang123456"); // docker
                //option.UseSqlServer(@"Data Source=ZC01N02188\QFLOW;Initial Catalog=FakeXiecheng;User ID=sa;Password=Jiang123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"); // docker
                //option.UseSqlServer(Configuration["DbContext:ConnectionString"]);
                option.UseMySql(Configuration["DbContext:MySQLConnectionString"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapGet("/test", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World from test!");
                //});
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });
        }
    }
}
