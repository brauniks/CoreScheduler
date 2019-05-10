using BLL.WebsiteFilter;
using DAL.Repoitory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using Schedule.WebApiCore.Sample.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.WebApiCore.Sample.Schedule
{
    [DisallowConcurrentExecution]
    public class NewScheduleJob : IScheduleJob
    {
        private readonly ILimangoProcess limangoProcess;
        private readonly IConfiguration configuration;
        private readonly ILogger<NewScheduleJob> logger;
        private readonly IProcessRepository processRepository;

        public NewScheduleJob(ILimangoProcess limangoProcess,IConfiguration configuration, ILogger<NewScheduleJob> logger, IProcessRepository processRepository )
        {
            this.logger = logger;
            this.processRepository = processRepository;
            this.limangoProcess = limangoProcess;
            this.configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        { 
            var websites = await this.processRepository.GetWebsites().ToListAsync();  
            try
            {
                foreach (var item in websites)
                {
                    this.limangoProcess.Process(item.Link, item.Cost);
                }                 
            }
            catch (System.Exception ex)
            {
                this.logger.LogCritical(ex.ToString());
                throw;
            }
            await Task.CompletedTask;
            
        }

        public IJobDetail JobConfiguration()
        {
            return JobBuilder.Create<NewScheduleJob>()
                 .WithIdentity("Sample.jobAAAA", "group2")
                 .Build();
        }

        public ITrigger Trigger()
        {
            return TriggerBuilder.Create()
                  .WithIdentity($"Sample.triggerAAAAA", "group2")
                  .StartNow()
                  .WithSimpleSchedule
                   (s =>
                      s.WithInterval(TimeSpan.FromSeconds(15))
                      .RepeatForever()
                   )
                   .Build();
        }
    }
}
