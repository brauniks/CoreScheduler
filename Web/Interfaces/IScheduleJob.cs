using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.WebApiCore.Sample.Interfaces
{
    public interface IScheduleJob : IJob
    {
        ITrigger Trigger();

        IJobDetail JobConfiguration(); 
    }
}
