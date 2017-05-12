using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Demo.Repository;
using Demo.Core;
using Demo.Repository.Entities;

namespace Demo.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddScoped<IAsyncDataRepository<Person>, AsyncDapperRepository<Person>>();
            services.AddScoped<IAsyncDataRepository<User>, AsyncDapperRepository<User>>();
            services.AddScoped<IAsyncDataRepository<Address>, AsyncDapperRepository<Address>>();
            services.AddScoped<IAsyncDataRepository<Email>, AsyncDapperRepository<Email>>();
            services.AddScoped<IChangeLog, ChangeLog>();
            services.AddScoped<IConnectionString>(provider => new ConnectionString(Configuration.GetConnectionString("DemoBase")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }


    }
}
