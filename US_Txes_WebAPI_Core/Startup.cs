using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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
            //Db connection
            var dbConnString = appConfiguration["dbConnectionString"];
            services.AddDbContext<CustomDbContext>(options => options.
                   UseSqlServer(dbConnString));

            //Dependency injections - repositories
            services.AddTransient<IDbEntityRepository<State>, StatesRepository>();
            services.AddTransient<IDbEntityRepository<ZipCode>, ZipCodesRepository>();
            services.AddTransient<IDbEntityRepository<Fee>, FeesRepository>();
            services.AddTransient<IDbEntityRepository<VehicleFee>, VehicleFeesRepository>();
            services.AddTransient<ITaxesRepository, TaxesRepository>();
            services.AddTransient<IHelperRepository, HelperRepository>();

            //To avoid errors with json cycle dependencies
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddLogging();

            //Automapper config
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

            //Request and Response logging
            app.UseMiddleware<RequestResponseMiddleware>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Connection custom logger
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
