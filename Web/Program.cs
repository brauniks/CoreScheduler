using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Schedule.WebApiCore.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting program");
            var cultures = System.Globalization.CultureInfo.CurrentCulture;
            var culture = new System.Globalization.CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5000")
                .UseStartup<Startup>();
    }
}
