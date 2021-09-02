using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laci
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
                        webBuilder.UseStartup<Startup>();
                    else
                        webBuilder.UseStartup<Startup>().UseUrls("http://localhost:5011");
                });
    }
}
