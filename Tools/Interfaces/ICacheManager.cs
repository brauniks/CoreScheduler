using System;

namespace MyScheduler.App.Tools.Tools
{
    public interface ICacheManager
    {
        object GetExistingOrAdd(string key, Func<string, object> func);
    }
}