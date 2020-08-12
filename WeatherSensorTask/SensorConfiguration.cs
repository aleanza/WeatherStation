using WeatherSensorInterfaces;

namespace WeatherSensorTask
{
    internal class SensorConfiguration : ISensorConfiguration
    {
        public string Id { get; private set; }
        public SensorType Type { get; private set; }
        public string Uri { get; private set; }

        public SensorConfiguration(string id, SensorType type, string uri)
        {
            Id = id;
            Type = type;
            Uri = uri;
        }
    }
}