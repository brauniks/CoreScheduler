//using CacheManager.Core;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyScheduler.App.Tools.Tools
//{
//    public class CacheManager : ICacheManager
//    {
//        private readonly ICacheManager<object> cache;

//        public CacheManager()
//        {
//            this.cache = CacheFactory.Build("getStartedCache", settings =>
//            {
//                settings.WithSystemRuntimeCacheHandle("handleName").WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(5));
//            });
//        }

//        public object GetExistingOrAdd(string key, Func<string,object> func)
//        {    
//            return cache.GetOrAdd(key, func);  
//        }
//    }
//}
