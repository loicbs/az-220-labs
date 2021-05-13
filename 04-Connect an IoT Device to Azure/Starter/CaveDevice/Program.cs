// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// This application uses the Azure IoT Hub device SDK for .NET
// For samples see: https://github.com/Azure/azure-iot-sdk-csharp/tree/master/iothub/device/samples

// INSERT using statements below here

using Microsoft.Azure.Devices.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CaveDevice
{
    public class Program
    {
        // INSERT variables below here
        private static DeviceClient deviceClient;

        // INSERT Main method below here
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting simulator.");
            var connectionString = args[0];
            deviceClient = DeviceClient.CreateFromConnectionString(connectionString);
            await SendDeviceToCloudMessagesAsync();
            Console.ReadKey();
        }

        // INSERT SendDeviceToCloudMessagesAsync method below here
        public static async Task SendDeviceToCloudMessagesAsync()
        {
            var sensor = new EnvironmentSensor();

            while (true)
            {
                var temperature = sensor.GetTemperature();
                var humidity = sensor.GetHumidity();

                var message = new Message(Encoding.UTF8.GetBytes(CreateMessageString(temperature, humidity)));
                
                Console.WriteLine("Sending telemetry.");
                await deviceClient.SendEventAsync(message);

                await Task.Delay(1000);
            }
        }


        // INSERT CreateMessageString method below here
        public static string CreateMessageString(int temperature, int humidity)
        {
            var telemetry = new
            {
                temperature,
                humidity
            };

            return JsonSerializer.Serialize(telemetry);
        }
    }

    // INSERT EnvironmentSensor class below here
    public class EnvironmentSensor
    {
        readonly int averageTemperature = 10;
        readonly int averageHumidity = 78;
        readonly Random random = new Random();

        public int GetTemperature() => averageTemperature + random.Next(0, 20);

        public int GetHumidity() => averageHumidity + random.Next(0, 22);
    }
}