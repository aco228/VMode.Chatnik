using System.Threading.Tasks;
using Chatnik.ClientApplication.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Chatnik.ClientConsoleApplication
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host =
                Bootstrapper.CreateHostBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<StartupService>();
                    })
                    .Build();
            await host.RunAsync();
        }
    }
}
