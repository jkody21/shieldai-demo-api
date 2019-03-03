using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ShieldAI.Core;
using ShieldAI.Service.Data.Model;

namespace ShieldAI.Service
{
    public class DroneEngine : BaseEngine, IDroneEngine
    {
        private const string DRONE_SQL = @"
            SELECT  DroneId,
                    Name,
                    CurrentGeneration,
                    IsActive
            FROM    [dbo].[Drone]
        ";

        public DroneEngine(IConfiguration configuration) : base(configuration)
        {
        }


        /// <summary>
        /// Return all available drones
        /// </summary>
        /// <returns></returns>
        public async Task<ActionStatus<IEnumerable<Drone>>> FindDrones()
        {
            var status = new ActionStatus<IEnumerable<Drone>>();

            var drones =
                await WithConnection<IEnumerable<Drone>>(
                    async c =>
                        await c.QueryAsync<Drone>(DRONE_SQL)
                );

            status.SetReturnData(drones);

            return status;
        }
    }
}
