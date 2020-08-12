using System;
using WeatherSensorInterfaces;

namespace VendorB
{
    /// <summary>
    /// Interface that interacts with sensors from VendorB through its encapsulated proprietary interface.
    /// </summary>
    ///
    public class Vendor : IVendor
    {
        private readonly External.IVendorB _external;

        public Vendor(External.IVendorB external)
        {
            _external = external;
        }

        public bool Validate(ISensorConfiguration sensorConfiguration)
        {
            if (!_external.AcceptsUri(sensorConfiguration.Uri))
            {
                throw new ArgumentException("Invalid argument" + sensorConfiguration.Uri);
            }

            return true;
        }

        public double ReadDoubleValue(ISensorConfiguration sensorConfiguration)
        {
            using (var connection = _external.Connect())
            {
                return connection.ReadDoubleValue(sensorConfiguration.Uri);
            }
        }
    }
}