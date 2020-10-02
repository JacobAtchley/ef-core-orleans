using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Test.Platform.Wms.Cosmo.Contexts;
using Test.Platform.Wms.Cosmo.Implementations;
using Test.Platform.Wms.Services;
using Test.Platform.Wms.Sql.Contexts;
using Test.Platform.Wms.Sql.Implementations;

namespace Test.Platform.Wms.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<InventoryRepository>()
                .AddClasses()
                .AsImplementedInterfaces());
            
            services.Scan(scan => scan.FromAssemblyOf<InventoryIncrementService>()
                .AddClasses()
                .AsImplementedInterfaces());

            services.Scan(scan => scan.FromAssemblyOf<OrderRepository>()
                .AddClasses()
                .AsImplementedInterfaces());
                
            services.AddControllers();
            
            services.AddDbContext<InventoryContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString(nameof(InventoryContext))));

            services.AddDbContext<OrderContext>(opt =>
                opt.UseCosmos(
                    Configuration["Cosmos:Endpoint"],
                    Configuration["Cosmos:Key"],
                    Configuration["Cosmos:Db"]));
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
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
                endpoints.MapControllers();
            });
            
        }
    }
}