using System.Threading.Tasks;
using Chatnik.ServerApplication.MessageProcessors;
using Chatnik.Shared.Implementations;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chatnik.ServerApplication // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static Task Main(string[] args)
            => CreateHostBuilder(args).RunConsoleAsync();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureAppConfiguration((webHostBuilderContext, configurationbuilder) =>
                {
                    var env = webHostBuilderContext.HostingEnvironment;
                    configurationbuilder.SetBasePath(env.ContentRootPath);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.Warning))
                .ConfigureServices((hostContext, services) =>
                {
                    var applicationConfiguration = new DefaultApplicationConfiguration(hostContext.Configuration);
                    applicationConfiguration.Args = args;
                    
                    services.AddSingleton(applicationConfiguration);
                    services.AddSingleton<IPortTester, PortTester>();
                    services.AddSingleton<IChatnikPublisherSocket, ChatnikPublisher>();
                    services.AddSingleton<IChatnikSubscriberSocket, ChatnikSubscriber>();
                    services.AddSingleton<IMessageListener, MessageListener>();

                    services.AddSingleton<IReceiveAndReturnMessageProcessor, ReceiveAndReturnMessageProcessor>(); 
                    
                    services.AddHostedService<StartupService>();
                });

        
    }
}