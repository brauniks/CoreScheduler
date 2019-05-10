using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Reflection;

namespace Schedule.WebApiCore.Sample.Schedule
{
    public static class QuartzExtensions
    { 
        public static void UseQuartz (this IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IScheduler>();
        }
    }
}
