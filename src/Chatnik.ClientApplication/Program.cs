using System;
using System.Windows.Forms;
using Chatnik.ClientApplication.Core;
using Chatnik.ClientApplication.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Chatnik.ClientApplication
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = Bootstrapper.CreateHostBuilder(null)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<LoginForm>();
                })
                .Build();

            ServiceProvider = host.Services;
            Application.Run(Extensions.GetView<LoginForm>());
        }
    }
}
