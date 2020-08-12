namespace WeatherSensorInterfaces
{
    public interface ISensorConfiguration
    {
        /// <summary>
        /// A unique ID to identify the sensor.
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///The type of the sensor.
        /// </summary>
        public SensorType Type { get; }

        /// <summary>
        /// An URI specifying how to this sensor can be accessed.
        /// </summary>
        public string Uri { get; }
    }
}