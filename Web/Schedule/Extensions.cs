using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.WebApiCore.Sample.Schedule
{
    public static class Extensions
    {
        public static void UseQuartz(this IServiceCollection services, params Type[] jobs)
        {
            services.AddSingleton<IJobFactory, ScheduledJobFactory>();
            //services.Add(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Singleton)));
            services.AddSingleton(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType)));

            services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler();
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start();
                return scheduler;
            });
        }

    }
}
