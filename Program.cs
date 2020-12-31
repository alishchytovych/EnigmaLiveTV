using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EnigmaLiveTV
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var configurationProviders = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: false)
				.AddEnvironmentVariables();

			if (args != null)
				configurationProviders = configurationProviders.AddCommandLine(args);

			var configuration = configurationProviders.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.Enrich.WithProperty("ServiceName", System.Reflection.Assembly.GetEntryAssembly().GetName().Name)
				.CreateLogger();

			CreateHostBuilder(args, configuration).Build().Run();
			Log.CloseAndFlush();

        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration = null) =>
            Host.CreateDefaultBuilder(args)
				.UseSystemd()
                .ConfigureAppConfiguration(config => {
					config.AddConfiguration(configuration);
				})
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.UseUrls(typeof(Startup).Namespace).UseSerilog().UseStartup<Startup>().UseKestrel((configuration, serverOptions) => {
						serverOptions.Configure(configuration.Configuration.GetSection("Kestrel:" + typeof(Startup).Namespace));
					});
				});

    }
}
