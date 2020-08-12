using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VendorB.External;

namespace WeatherSensorTask
{
    class VendorBExternalStub : VendorB.External.IVendorB
    {
        public IDictionary<string, double?> Values { get; }
        public bool ThrowConnectError { get; set; }

        public VendorBExternalStub()
        {
            Values = new Dictionary<string, double?>();
        }

        public bool AcceptsUri(string uri)
        {
            return uri.StartsWith("b:");
        }

        public IVendorBConnection Connect()
        {
            if (ThrowConnectError)
            {
                throw new IOException("Failed to connect");
            }

            return new DummyBConnection(Values);
        }
    }

    class DummyBConnection : IVendorBConnection
    {
        private readonly IDictionary<string, double?> _values;

        public DummyBConnection(IDictionary<string, double?> values)
        {
            _values = values;
        }

        public void Dispose()
        {
        }

        public double ReadDoubleValue(string uri)
        {
            double? value = null;
            if (_values.ContainsKey(uri))
            {
                value = _values[uri];
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
