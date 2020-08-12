using System;
using System.Collections.Generic;
using System.IO;
using WeatherSensorInterfaces;

namespace WeatherSensorTask
{
    public class WeatherStationSensors
    {
        private readonly List<Sensor> _sensors = new List<Sensor>();

        /// <summary>
        /// Registers a sensor.
        /// </summary>
        public void AddSensor(IVendor vendor, string id, SensorType type, string uri) 
        {
            var sensorConfiguration = new SensorConfiguration(id, type, uri);
            if (vendor.Validate(sensorConfiguration))
            {
                _sensors.Add(new Sensor(vendor, sensorConfiguration));
            }
        }

        /// <summary>
        /// Reads the current values for each sensor.
        /// </summary>
        /// <returns>a mapping from sensor IDs to sensor values</returns>
        public IDictionary<string, SensorValue> ReadSensorValues()
        {
            var values = new Dictionary<string, SensorValue>();
            foreach (var sensor in _sensors)
            {
                var sensorValue = sensor.GetSensorValue();
                values.Add(sensor.SensorId, sensorValue);
            }

            return values;
        }
    }
}