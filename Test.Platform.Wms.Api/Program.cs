using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Test.Platform.Wms.Cosmo;
using Test.Platform.Wms.Cosmo.Contexts;
using Test.Platform.Wms.Orleans.Grains.Implementations;
using Test.Platform.Wms.Sql.Contexts;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace Test.Platform.Wms.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateEfDb<InventoryContext>(host);
            CreateEfDb<OrderContext>(host);
            host.Run();
        }

        public static void CreateEfDb<TContext>(IHost host)
            where TContext: DbContext
        {
            using var scope = host.Services.CreateScope();
            
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<TContext>();
                    
                context.Database.EnsureCreated();

                if (context is InventoryContext inventoryContext)
                {
                    InventoryContextSeeder.Init(inventoryContext);
                }
                else if (context is OrderContext orderContext)
                {
                    OrderContextSeeder.Init(orderContext, 10);
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                    
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration( (context, builder) => {
                    if(context.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddUserSecrets("Test.Platform.Wms", true);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                //.UseOrleans(ConfigureOrleans)
        ;

        private static void ConfigureOrleans(HostBuilderContext context, ISiloBuilder siloBuilder)
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                siloBuilder.UseLocalhostClustering();
            }

            siloBuilder.Configure<ClusterOptions>(opt =>
            {
                opt.ClusterId = context.HostingEnvironment.IsDevelopment() ? "dev" : "prod";
                opt.ServiceId = "Test.Platform.Wms.Inventory";
            });

            siloBuilder.Configure<SiloMessagingOptions>(opt =>
            {
                opt.ResponseTimeout = TimeSpan.FromMinutes(10);
                opt.ResponseTimeoutWithDebugger = TimeSpan.FromHours(1);
            });

            siloBuilder.Configure<ClientMessagingOptions>(opt =>
            {
                opt.ResponseTimeout = TimeSpan.FromMinutes(10);
                opt.ResponseTimeoutWithDebugger = TimeSpan.FromHours(1);
            });

            siloBuilder.AddAzureBlobGrainStorage("inventoryStorage", opt =>
            {
                opt.ContainerName = "inventory";
                opt.UseJson = true;
                opt.ConnectionString = context.Configuration.GetConnectionString("Blob");
            });

            siloBuilder.ConfigureApplicationParts(manager => { manager.AddApplicationPart(typeof(InventoryGrain).Assembly).WithReferences(); });
        }
    }
}