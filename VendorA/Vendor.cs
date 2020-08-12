using System;
using WeatherSensorInterfaces; 

namespace VendorA
{
    /// <summary>
    /// Interface that interacts with sensors from VendorA through its encapsulated proprietary interface.
    /// </summary>
    ///
    public class Vendor : IVendor
    {
        private readonly External.IVendorA _external;

        public Vendor(External.IVendorA external)
        {
            _external = external;
        }

        public bool Validate(ISensorConfiguration sensorConfiguration)
        {
            if (!_external.CanHandleUri(sensorConfiguration.Uri))
            {
                throw new ArgumentException("Invalid argument" + sensorConfiguration.Uri);
            }

            return true;
        }

        public double ReadDoubleValue(ISensorConfiguration sensorConfiguration)
        {
            return _external.ReadDoubleValue(sensorConfiguration.Uri);
        }
    }
}