using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShieldAI.Service.Data.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShieldAI.Service.Test.Integration
{
    [TestClass]
    public class FlightEngineTests
    {
        /// <summary>
        /// Test retrieval and search of flight-log entries
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task should_find_flight_logs()
        {
            var request = new FlightLogRequest()
            {
                //DroneId = 121,
                //From = new System.DateTime(2019, 2, 1),
                //To = new System.DateTime(2019, 2, 10),
                //DroneGeneration = 6,
                //Latitude = 32.7157,
                //Longitude = 117.1611,
                //Distance = 10.0,
                DurationLow = 10,
                DurationHigh = 30,
            };

            var engine = SetupHelper.GetConfiguredFlightEngine();

            var result = await engine.FindFlights(request);

            result.Should().NotBeNull();
        }


        /// <summary>
        /// test to view the flight-log metrics
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task should_find_flight_log_metrics()
        {
            var engine = SetupHelper.GetConfiguredFlightEngine();

            var result = await engine.GetFlightLogMetrics();

            result.Should().NotBeNull();
        }


        /// <summary>
        /// Test inserting a flight log entry
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task should_create_flight_log_entry()
        {
            var request = new FlightLog()
            {
                FlightLogId = 0,
                DroneId = 321,
                DroneGeneration = 1,
                BeginOn = DateTime.Now,
                EndOn = DateTime.Now.AddDays(-2),
                Longitude = -87.6298,
                Latitude = 41.8781,
                MapPath = Guid.NewGuid().ToString()
            };

            var engine = SetupHelper.GetConfiguredFlightEngine();

            var result = await engine.InsertFlightLog(request);

            result.Should().NotBeNull();
            result.ReturnData.FlightLogId.Should().BeGreaterThan(0);
        }


        /// <summary>
        /// Test bulk adding a flight-log entry
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task should_bulk_insert_to_flight_log()
        {
            var request = new List<FlightLog>();

            request.Add(new FlightLog() {
                FlightLogId = 0,
                DroneId = 321,
                DroneGeneration = 111,
                BeginOn = DateTime.Now,
                EndOn = DateTime.Now.AddDays(2),
                Longitude = 71.0589,
                Latitude = 42.3601,
                MapPath = Guid.NewGuid().ToString()
            });

            request.Add(new FlightLog()
            {
                FlightLogId = 0,
                DroneId = 821,
                DroneGeneration = 10,
                BeginOn = DateTime.Now,
                EndOn = DateTime.Now.AddDays(2),
                Longitude = 87.6298,
                Latitude = 41.8781,
                MapPath = Guid.NewGuid().ToString()
            });

            request.Add(new FlightLog()
            {
                FlightLogId = 0,
                DroneId = 121,
                DroneGeneration = 17,
                BeginOn = DateTime.Now,
                EndOn = DateTime.Now.AddDays(2),
                Longitude = 71.2478,
                Latitude = 42.0654,
                MapPath = Guid.NewGuid().ToString()
            });

            var engine = SetupHelper.GetConfiguredFlightEngine();

            var result = await engine.BulkInsertFlightLog(request);

            result.Should().NotBeNull();
        }
    }
}
