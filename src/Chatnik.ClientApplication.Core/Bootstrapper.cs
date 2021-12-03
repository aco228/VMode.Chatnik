using Chatnik.ClientApplication.Core.MessageProcessors;
using Chatnik.ClientApplication.Core.Services;
using Chatnik.Shared.Implementations;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chatnik.ClientApplication.Core
{
    public static class Bootstrapper
    {
        public static IHostBuilder CreateHostBuilder(string[]? args) =>
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
                .ConfigureServices((hostContext, services) =>
                { 
                    var applicationConfiguration = new DefaultApplicationConfiguration(hostContext.Configuration);
                    services.AddSingleton(applicationConfiguration);
                    
                    services.AddSingleton(applicationConfiguration);
                    services.AddSingleton<IPortTester, PortTester>();
                    services.AddSingleton<IChatnikPublisherSocket, ChatnikPublisher>();
                    services.AddSingleton<IChatnikSubscriberSocket, ChatnikSubscriber>();
                    services.AddSingleton<IMessageListener, MessageListener>();

                    services.AddSingleton<IHearthbeatMessageProcessor, HearthbeatMessageProcessor>();
                    services.AddSingleton<IHearthbeatService, HearthbeatService>();
                    services.AddSingleton<IChatMessageProcessor, ChatMessageProcessor>();

                });
    }
}