using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shared.Services;
using Shared.Models.Devices;
using Newtonsoft.Json.Linq;
using Shared;

namespace IntelliLamp
{
    public partial class App : Application
    {
        public static IHost? host { get; set; }

        public App()
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((config, services) =>
                {
                    var conn = config.Configuration.GetConnectionString("Device")!;


                    services.AddSingleton<MainWindow>();
                    services.AddSingleton(new DeviceConfiguration(conn));
                    services.AddSingleton<DeviceManager>();
                    services.AddSingleton<NetworkManager>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host!.StartAsync();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();

            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
