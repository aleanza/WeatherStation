using System;
using System.IO;
using WeatherSensorInterfaces;

namespace WeatherSensorTask
{
    public class Sensor
    {

        private readonly IVendor _vendor;
        private readonly ISensorConfiguration _sensorConfiguration;

        public string SensorId => _sensorConfiguration.Id;
        
        public Sensor(IVendor vendor, ISensorConfiguration sensorConfiguration)
        {
            _vendor = vendor;
            _sensorConfiguration = sensorConfiguration;
        }

        public SensorValue GetSensorValue()
        {
            double? value;
            bool valid;
            try
            {
                value = ReadDoubleValue();
                valid = IsValid(value.Value);
            }
            catch (IOException)
            {
                value = null;
                valid = false;
            }

            string unit = GetUnit();
            return new SensorValue(value, valid, unit);
        }

        private double ReadDoubleValue()
        {
            return _vendor.ReadDoubleValue(_sensorConfiguration);
        }

        private bool IsValid(double value)
        {
            return _sensorConfiguration.Type switch
            {
                SensorType.TEMPERATURE => IsTemperatureInRange(value),
                SensorType.WIND_SPEED => IsWindInRange(value),
                SensorType.WIND_DIRECTION => IsWindDirectionInRange(value),
                SensorType.HUMIDITY => IsHumidityInRange(value),
                _ => false
            };
        }

        private bool IsTemperatureInRange(in double value)
        {
            return value >= -50.00 && value <= 150.0;
        }

        private bool IsWindInRange(in double value)
        {
            return value >= 0.0;
        }

        private bool IsWindDirectionInRange(in double value)
        {
            return value >= -Math.PI && value <= Math.PI;
        }

        private bool IsHumidityInRange(in double value)
        {
            return value >= 0.0 && value <= 100.0;
        }

        private string GetUnit()
        {
            return _sensorConfiguration.Type switch
            {
                SensorType.TEMPERATURE => "°C",
                SensorType.WIND_SPEED => "km/h",
                SensorType.WIND_DIRECTION => "",
                SensorType.HUMIDITY => "%",
                _ => throw new ArgumentException("Unknown SensorType: " + _sensorConfiguration.Type),
            };
        }
    }
}
