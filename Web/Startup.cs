using System;
using System.Collections.Specialized;
using System.IO;
using BLL.WebsiteFilter;
using DAL;
using DAL.Repoitory;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyScheduler.App.BLL.WebsiteFilter;
using MyScheduler.App.Tools.Email;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Schedule.WebApiCore.Sample.Interfaces;
using Schedule.WebApiCore.Sample.Schedule;
using Swashbuckle.AspNetCore.Swagger;
using Tools;
using static MyScheduler.App.Tools.Email.Email;

namespace Schedule.WebApiCore.Sample
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.HostingEnvironment = hostingEnvironment;
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {             
            var connectionString = Configuration["ConnectionStrings:PostGre"];
            services.AddDbContext<PostgreDbContext>(
                opts => opts.UseNpgsql(connectionString, m=> m.MigrationsAssembly("Web"))
            ,ServiceLifetime.Singleton);

            services.Configure<AppSettingsModel>(Configuration.GetSection("ApplicationSettings"));
            services.AddOptions();
            services.AddTransient<ISchedSmtpClient, SchedSmtpClient>();
            services.AddTransient<IEmail, Email>();

            services.AddTransient<IWebsiteRepository, WebsiteRepository>();
            services.AddTransient<IProcessRepository, ProcessRepository>();
            
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                   .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                //.AddJsonFile($"appsettings.{this.HostingEnvironment.EnvironmentName.ToLower()}.json")
                .Build());

            services.Scan(scan => scan.FromAssemblyOf<IScheduleJob>()
             .AddClasses(classes => classes.AssignableTo<IScheduleJob>())
             .AsSelfWithInterfaces()
            .WithTransientLifetime());


            services.AddSingleton<IRegisteredJobFactory, RegisteredJobFactory>();
            services.AddSingleton<IJobFactory, ScheduledJobFactory>();
            services.AddSingleton<IScheduler>(provider =>
            {
                NameValueCollection props = new NameValueCollection
        {
            { "quartz.serializer.type", "binary" },
            { "quartz.scheduler.instanceName", "MyScheduler" },
            { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
            { "quartz.threadPool.threadCount", "3" }
        };

                var schedulerFactory = new StdSchedulerFactory(props);

                var scheduler = schedulerFactory.GetScheduler().ConfigureAwait(false).GetAwaiter().GetResult();
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                foreach (var job in provider.GetService<IRegisteredJobFactory>().Get())
                {
                    scheduler.ScheduleJob(job.Key, job.Value).ConfigureAwait(false).GetAwaiter().GetResult();
                }

                scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();
                return scheduler;
            });

            services.AddTransient<ILimangoProcess, LimangoProcess>(); 
            services.AddTransient<HtmlWeb, HtmlWeb>();
            services.AddTransient<ILimangoWebsiteProcessor, LimangoWebsiteProcessor>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }
         
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddLog4Net(); // << Add this line

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseQuartz();

            app.UseMvc();           
        }
    }
}
