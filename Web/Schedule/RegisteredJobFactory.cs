using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Schedule.WebApiCore.Sample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.WebApiCore.Sample.Schedule
{
    public class RegisteredJobFactory : IRegisteredJobFactory
    {
        private readonly IServiceProvider services;

        public RegisteredJobFactory(IServiceProvider services )
        {
            this.services = services;
        }

        public Dictionary<IJobDetail, ITrigger> Get()
        {
            var jobs = this.services.GetServices<IScheduleJob>().ToDictionary(j => j.JobConfiguration(), j => j.Trigger());
            return jobs;
        }
    }
}