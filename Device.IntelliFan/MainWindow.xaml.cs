﻿using Newtonsoft.Json;
using Shared.DataDeviceModels;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Device.IntelliFan
{

    public partial class MainWindow : Window
    {
        private readonly DeviceManager _deviceManager;
        public MainWindow(DeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            InitializeComponent();

            Task.WhenAll(SetDeviceTypeAsync(), ToggleFanStateAsync(), CheckConnectivityAsync(), SendTelemetryDataAsync());

        }

        private async Task ToggleFanStateAsync()
        {
            Storyboard fan = (Storyboard)this.FindResource("FanStoryboard");

            while (true)
            {

                if (_deviceManager.AllowSending())
                    fan.Begin();
                else
                    fan.Stop();

                await Task.Delay(1000);
            }
        }

        private async Task CheckConnectivityAsync()
        {
            while (true)
            {
                ConnectivityStatus.Text = await NetworkManager.CheckConnectivityAsync();
                await Task.Delay(1000);
            }
        }


        private async Task SetDeviceTypeAsync()
        {
            var deviceType = "Fan";

            await _deviceManager.SendDeviceTypeAsync(deviceType);
        }

        private async Task SendTelemetryDataAsync()
        {

            while (true)
            {
                if (_deviceManager.Configuration.AllowSending)
                {
                    var dataModel = new FanDataModel()
                    {
                        IsActive = true,
                        Location = "Livingroom",
                        CurrentTime = DateTime.Now
                    };


                    var telemetryDataJson = JsonConvert.SerializeObject(new
                    {
                        DeviceOn = dataModel.IsActive,
                        Location = dataModel.Location,
                        CurrentTime = dataModel.CurrentTime,
                        ContainerName = dataModel.ContainerName,
                    });

                    var latestMessageJson = JsonConvert.SerializeObject(new
                    {
                        IsActive = dataModel.IsActive,
                        CurrentTime = dataModel.CurrentTime,
                    });

                    var operationalStatusJson = JsonConvert.SerializeObject(dataModel.IsActive);

                    var locationString = dataModel.Location;

                    if (await _deviceManager.SendLatestMessageAsync(latestMessageJson) &&
                        await _deviceManager.SendOperationalStatusAsync(operationalStatusJson) &&
                        await _deviceManager.SendLocationAsync(locationString) &&
                        await _deviceManager.SendDataToCosmosDbAsync(telemetryDataJson))
                        CurrentMessageSent.Text = $"Message sent successfully: {latestMessageJson}, DeviceOn: {operationalStatusJson}, Location: {locationString}";

                    var telemetryInterval = _deviceManager.Configuration.TelemetryInterval;

                    await Task.Delay(telemetryInterval);
                }
            }

        }


    }
}

