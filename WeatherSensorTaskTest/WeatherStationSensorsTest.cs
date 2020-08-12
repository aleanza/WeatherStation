using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

using WeatherSensorInterfaces;

namespace WeatherSensorTask
{
    [TestFixture]
    public class WeatherStationSensorsTest
    {
        private VendorAExternalStub _vendorAExternalStub;
        private VendorBExternalStub _vendorBExternalStub;
        private VendorA.Vendor _vendorAStub;
        private VendorB.Vendor _vendorBStub;

        private WeatherStationSensors _weatherStationSensorsVendor;

        [SetUp]
        public void SetUp()
        {
            _vendorAExternalStub = new VendorAExternalStub();
            _vendorBExternalStub = new VendorBExternalStub();
            _vendorAStub = new VendorA.Vendor(_vendorAExternalStub);
            _vendorBStub = new VendorB.Vendor(_vendorBExternalStub);
            _weatherStationSensorsVendor = new WeatherStationSensors();
        }

        [Test]
        public void ValidatesUriWhenAddingSensor()
        {
            _weatherStationSensorsVendor.AddSensor(_vendorAStub, "id1", SensorType.TEMPERATURE, "a:test");
            _weatherStationSensorsVendor.AddSensor(_vendorBStub,"id2", SensorType.TEMPERATURE, "b:test");

            Assert.Throws(typeof(ArgumentException), () => _weatherStationSensorsVendor.AddSensor(_vendorAStub, "id3", SensorType.TEMPERATURE, "c:test"));
            Assert.Throws(typeof(ArgumentException), () => _weatherStationSensorsVendor.AddSensor(_vendorBStub, "id3", SensorType.TEMPERATURE, "c:test"));
        }

        [Test]
        public void TemperatureSensorVendorA()
        {
            CheckTemperatureSensor(_vendorAStub, "a:test", _vendorAExternalStub.Values);
        }

        [Test]
        public void TemperatureSensorVendorB()
        {
            CheckTemperatureSensor(_vendorBStub, "b:test", _vendorBExternalStub.Values);
        }

        private void CheckTemperatureSensor(IVendor vendor, string uri, IDictionary<string, double?> values)
        {
            _weatherStationSensorsVendor.AddSensor(vendor, "id", SensorType.TEMPERATURE, uri);

            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", null, false, "°C");

            values[uri] = 27.3;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", 27.3, true, "°C");

            values[uri] = -274.0;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", -274.0, false, "°C");

            values[uri] = 200.2;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", 200.2, false, "°C");
        }

        [Test]
        public void WindSpeedSensorVendorA()
        {
            CheckWindSpeedSensor(_vendorAStub, "a:test", _vendorAExternalStub.Values);
        }

        [Test]
        public void WindSpeedSensorVendorB()
        {
            CheckWindSpeedSensor(_vendorBStub, "b:test", _vendorBExternalStub.Values);
        }

        private void CheckWindSpeedSensor(IVendor vendor, string uri, IDictionary<string, double?> values)
        {
            _weatherStationSensorsVendor.AddSensor(vendor,"id", SensorType.WIND_SPEED, uri);

            values[uri] = 27.3;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", 27.3, true, "km/h");

            values[uri] = -double.Epsilon;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", -double.Epsilon, false, "km/h");
        }

        [Test]
        public void WindDirectionSensorVendorA()
        {
            CheckWindDirectionSensor(_vendorAStub, "a:test", _vendorAExternalStub.Values);
        }

        [Test]
        public void WindDirectionSensorVendorB()
        {
            CheckWindDirectionSensor(_vendorBStub, "b:test", _vendorBExternalStub.Values);
        }

        private void CheckWindDirectionSensor(IVendor vendor, string uri, IDictionary<string, double?> values)
        {
            _weatherStationSensorsVendor.AddSensor(vendor, "id", SensorType.WIND_DIRECTION, uri);

            values[uri] = -Math.PI;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", -Math.PI, true, "");

            values[uri] = Math.PI;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", Math.PI, true, "");

            double justAbovePi = Math.PI + 0.0000001;
            values[uri] = justAbovePi;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", justAbovePi, false, "");
            values[uri] = -justAbovePi;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", -justAbovePi, false, "");
        }

        [Test, Description("Humidity Sensor")]
        public void HumiditySensorVendorA()
        {
            CheckHumiditySensor(_vendorAStub, "a:test", _vendorAExternalStub.Values);
        }

        [Test, Description("Humidity Sensor")]
        public void HumiditySensorVendorB()
        {
            CheckHumiditySensor(_vendorBStub, "b:test", _vendorBExternalStub.Values);
        }

        private void CheckHumiditySensor(IVendor vendor, string uri, IDictionary<string, double?> values)
        {
            _weatherStationSensorsVendor.AddSensor(vendor, "id", SensorType.HUMIDITY, uri);

            values[uri] = 45.0;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", 45.0, true, "%");

            values[uri] = 100.1;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", 100.1, false, "%");

            values[uri] = -0.1;
            CheckSingleValue(_weatherStationSensorsVendor.ReadSensorValues(), "id", -0.1, false, "%");
        }

        [Test]
        public void MultipleSensors()
        {
            _weatherStationSensorsVendor.AddSensor(_vendorAStub, "A", SensorType.TEMPERATURE, "a:test");
            _weatherStationSensorsVendor.AddSensor(_vendorBStub, "B", SensorType.HUMIDITY, "b:test");

            _vendorAExternalStub.Values["a:test"] = 27.3;
            _vendorBExternalStub.Values["b:test"] = 56.0;
            var values = _weatherStationSensorsVendor.ReadSensorValues();
            Assert.AreEqual(2, values.Count);
            CheckValue(values, "A", 27.3, true, "°C");
            CheckValue(values, "B", 56.0, true, "%");
        }

        [Test]
        public void ErrorHandling()
        {
            _weatherStationSensorsVendor.AddSensor(_vendorAStub, "A", SensorType.TEMPERATURE, "a:test");
            _weatherStationSensorsVendor.AddSensor(_vendorBStub, "B", SensorType.HUMIDITY, "b:test");

            _vendorAExternalStub.Values["a:test"] = 27.3;
            _vendorBExternalStub.Values["b:test"] = 56.0;
            _vendorBExternalStub.ThrowConnectError = true;

            var values = _weatherStationSensorsVendor.ReadSensorValues();
            Assert.AreEqual(2, values.Count);
            CheckValue(values, "A", 27.3, true, "°C");
            CheckValue(values, "B", null, false, "%");
        }

        private void CheckSingleValue(IDictionary<string, SensorValue> values, string id, double? number, bool valid,
            string unit)
        {
            Assert.AreEqual(1, values.Count);
            CheckValue(values, id, number, valid, unit);
        }

        private void CheckValue(IDictionary<string, SensorValue> values, string id, double? number, bool valid,
            string unit)
        {
            SensorValue value = values[id];
            Assert.AreEqual(number, value.Value);
            Assert.AreEqual(valid, value.Valid);
            Assert.AreEqual(unit, value.Unit);
        }

    }

}