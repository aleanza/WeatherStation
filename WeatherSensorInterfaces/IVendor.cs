using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WeatherSensorInterfaces
{
    public interface IVendor
    {
        /// <summary>
        /// Validates the sensor configuration.
        /// </summary>
        /// <exception cref="ArgumentException">if the configuration is not valid</exception>
        bool Validate(ISensorConfiguration sensorConfiguration);

        /// <summary>
        /// Reads the current value from the sensor.
        /// </summary>
        /// <exception cref="IOException">if an error occurs</exception>
        double ReadDoubleValue(ISensorConfiguration sensorConfiguration);
    }
}
