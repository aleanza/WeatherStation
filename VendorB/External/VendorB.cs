using System.IO;

namespace VendorB.External
{
    /// <summary>
    /// Interface to interact with sensors from VendorB.
    /// This is a third-part interface. You cannot change it!
    /// </summary>
    public interface IVendorB
    {
        /// <summary>
        /// Returns true if the uri belongs to a sensor from VendorB.
        /// </summary>
        bool AcceptsUri(string uri);

        /// <summary>
        /// Creates a new Connection which is required to read sensor values.
        /// </summary>
        /// <exception cref="IOException">if an error occurs</exception>
        IVendorBConnection Connect();
    }
}