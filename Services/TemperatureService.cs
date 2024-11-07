using System;
using System.Device.Gpio;
using Iot.Device.DHTxx;

namespace hydropon_poc.Services
{
    public class TemperatureService
    {
        private readonly int _pin;
        private Dht22 _sensor;

        public TemperatureService(int pin)
        {
            _pin = pin;
            var gpioController = new GpioController(); // Create the controller separately
            _sensor = new Dht22(_pin, PinNumberingScheme.Logical, gpioController); // Pass the numbering scheme
        }

        public double ReadTemperature()
        {
            if (_sensor.TryReadTemperature(out var temperature))
            {
                return temperature.DegreesCelsius;
            }
            else
            {
                throw new InvalidOperationException("Failed to read temperature.");
            }
        }

        public event Action<double> TemperatureUpdated;

        public void StartReading()
        {
            // Replace with GPIO pin number
            int sensorPin = 4;
            var tempSensor = new TemperatureService(sensorPin);

            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        double temperature = tempSensor.ReadTemperature();
                        TemperatureUpdated?.Invoke(temperature);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading temperature: {ex.Message}");
                    }
                    await Task.Delay(1000); // Adjust interval as needed
                }
            });
        }
    }
}
