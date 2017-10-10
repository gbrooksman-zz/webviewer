using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;


namespace webviewer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)                
                .WriteTo.RollingFile("Logs/log-{Date}.txt", fileSizeLimitBytes: 1048576)
                .Enrich.FromLogContext()
                .CreateLogger();


            Log.Information("Starting web host");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseSerilog() 
                .Build();

            host.Run();
        }
    }
}
