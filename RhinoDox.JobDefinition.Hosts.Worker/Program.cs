using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RhinoDox.Core.V2.Configuration;
using System.Threading.Tasks;
using RhinoDox.JobDefinition.Domain.Adapters;

namespace RhinoDox.JobDefinition.Hosts.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("config.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"config.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<JobDefinitionStagingPathMonitoringServiceConfig>(hostContext.Configuration.GetSection("DaemonConfig"));

                    services.AddTransient<IJobDefinitionRepositoryAdapter>(s =>
                        new JobDefinitionRepositoryAdapter(
                            hostContext.Configuration["DaemonConfig:DatabaseConnectionString"]));
                    services.AddSingleton<IHostedService, JobDefinitionStagingPathMonitoringService>();
                })
                .ConfigureLogging((hostingContext, logging) => {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                });

            await builder.RunConsoleAsync();
        }
    }
}
