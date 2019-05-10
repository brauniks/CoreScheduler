using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Collections.Generic;

namespace Schedule.WebApiCore.Sample.Schedule
{
    public interface IRegisteredJobFactory
    {
        Dictionary<IJobDetail, ITrigger> Get();
    }
}