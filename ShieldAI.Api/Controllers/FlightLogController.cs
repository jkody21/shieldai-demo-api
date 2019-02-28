using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ShieldAI.Service;
using ShieldAI.Service.Data.Model;
using System.Threading.Tasks;
using ShieldAI.Core;
using System.Net.Http;
using System.Net;
using System.Text;

namespace ShieldAI.Api.Controllers {
    [Route("flightlog")]
    public class FlightLogController : Controller
    {
        private readonly IFlightEngine _flightEngine;

        public FlightLogController(
            IFlightEngine flightEngine) {

            _flightEngine = flightEngine;
        }


        /// <summary>
        /// Return a list of flight log records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<FlightLog>> Get()
        {
            var results = await  _flightEngine.FindFlights(new FlightLogRequest() { DroneId = 121 });
            return results.ReturnData;

        }

        /// <summary>
        /// Return an individual flight log record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionStatus<FlightLog>> Get(int id)
        {
            var result = await _flightEngine.GetFlight(id);
            return result;
        }


        /// <summary>
        /// insert a new flight log record
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]FlightLog log)
        {
            var result = await _flightEngine.InsertFlightLog(log);

            if (result.Success) {
                return Ok(result);
            } else {
                var builder = new StringBuilder();
                foreach (var m in result.Messages)
                    builder.AppendLine(m);

                return BadRequest(builder.ToString());
            }
        }


        /// <summary>
        /// Bulk insert of flight-logs optimized for performance
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bulk-entry")]
        public async Task BulkEntry([FromBody]IEnumerable<FlightLog> logs) {
            var result = await _flightEngine.BulkInsertFlightLog(logs);
        }

        /*
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
