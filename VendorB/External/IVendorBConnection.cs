using System;
using System.IO;

namespace VendorB.External
{
    public interface IVendorBConnection : IDisposable
    {
        /// <summary>
        /// Reads the current value from the sensor identified with the given URI.
        /// </summary>
        /// <exception cref="IOException">if an error occurs</exception>
        double ReadDoubleValue(string uri);
    }
}