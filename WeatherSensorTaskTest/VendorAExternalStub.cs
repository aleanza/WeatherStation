using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WeatherSensorInterfaces;

namespace WeatherSensorTask
{
    class VendorAExternalStub : VendorA.External.IVendorA
    {
        public IDictionary<string, double?> Values { get; }

        public VendorAExternalStub()
        {
            Values = new Dictionary<string, double?>();
        }

        public bool CanHandleUri(string uri)
        {
            return uri.StartsWith("a:");
        }

        public double ReadDoubleValue(string uri)
        {
            double? value = null;
            if (Values.ContainsKey(uri))
            {
                value = Values[uri];
            }

            if (value.HasValue)
            {
                return value.Value;
            }
            else
            {
                throw new IOException("error");
            }
        }
    }
}
