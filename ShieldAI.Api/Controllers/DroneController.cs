using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShieldAI.Service;
using ShieldAI.Service.Data.Model;

namespace ShieldAI.Api.Controllers
{
    [Route("drone")]
    [ApiController]
    public class DroneController : ControllerBase
    {

        private readonly IDroneEngine _droneEngine;

        public DroneController(IDroneEngine droneEngine)
        {
            _droneEngine = droneEngine;
        }


        /// <summary>
        /// returns all drones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Drone>> Get()
        {
            var results = await _droneEngine.FindDrones();
            return results.ReturnData;
        }

    }
}