using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbModels;
using US_Txes_WebAPI_Core.DbRepositories;
using US_Txes_WebAPI_Core.Extensions;
using US_Txes_WebAPI_Core.Logging;
using US_Txes_WebAPI_Core.Mappings;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core
{
    public class Startup
    {
        private IConfiguration appConfiguration { get; set; }
        public Startup()
        {
            var configBuilder = JsonConfigurationExtensions.AddJsonFile(new ConfigurationBuilder(), $"{Directory.GetCurrentDirectory()}\\Configuration\\envsettings.json");

            appConfiguration = configBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnString = appConfiguration["dbConnectionString"];
            services.AddDbContext<CustomDbContext>(options => options.
                   UseSqlServer(dbConnString));

            services.AddTransient<IDbRepository<State>, StatesRepository>();
            services.AddTransient<IDbRepository<ZipCode>, ZipCodesRepository>();
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddLogging();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }


        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<RequestResponseMiddleware>();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var logFilePath = appConfiguration["logFilePath"];
            if (string.IsNullOrEmpty(logFilePath))
            {
                logFilePath = Directory.GetCurrentDirectory();
            }

            loggerFactory.AddFile(logFilePath + $"/{DateTime.UtcNow.Year}_{DateTime.UtcNow.Month}_{DateTime.UtcNow.Day}_{DateTime.UtcNow.Hour}_{DateTime.UtcNow.Minute}_{DateTime.UtcNow.Second}_log.txt");
            var logger = loggerFactory.CreateLogger(typeof(FileLogger));
        }
    }
}
