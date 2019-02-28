using Dapper;
using Microsoft.Extensions.Configuration;
using ShieldAI.Core;
using ShieldAI.Service.Data.Model;
using ShieldAI.Service.Data.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldAI.Service {
    public class FlightEngine : BaseEngine, IFlightEngine {

        private const string FLIGHTLOG_SQL_BASE = @"
                    SELECT [FlightLogId]
                          ,[DroneId]
                          ,[DroneGeneration]
                          ,[BeginOn]
                          ,[EndOn]
                          ,[Latitude]
                          ,[Longitude]
                          ,[MapPath]
                      FROM [dbo].[FlightLog]
                      WHERE 1 = 1";


        public FlightEngine(IConfiguration configuration) : base(configuration) {

        }


        /// <summary>
        /// Retrieve a list of flights based on input parameters
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionStatus<IEnumerable<FlightLog>>> FindFlights(FlightLogRequest request) {
            var status = new ActionStatus<IEnumerable<FlightLog>>();

            var sql = GetSqlBuilder();
            sql.AppendLine("AND DroneId = ISNULL(@DroneId, DroneId)");
                
            var flightLogs =
                await WithConnection<IEnumerable<FlightLog>>(
                    async c => 
                        await c.QueryAsync<FlightLog>(sql.ToString(), new { DroneId = request.DroneId })
                );

            var flightLogList = flightLogs.ToList();

            status.SetReturnData(flightLogList);
            return status;
        }


        /// <summary>
        /// Return all flight log entries matching the entered criteria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionStatus<FlightLog>> GetFlight(int id) {
            var status = new ActionStatus<FlightLog>();

            var sql = GetSqlBuilder();
            sql.AppendLine("AND FlightLogId = @FlightLogId");

            var flight =
                await WithConnection<IEnumerable<FlightLog>>(
                    async c =>
                        await c.QueryAsync<FlightLog>(sql.ToString(), new { FlightLogId = id })
                );

            var flightLogList = flight.FirstOrDefault();

            status.SetReturnData(flightLogList);
            return status;
        }


        /// <summary>
        /// Insert a single flight log entry to the DB
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<ActionStatus<FlightLog>> InsertFlightLog(FlightLog log) {
            var status = GetActionStatus<FlightLog>();
            var validator = new FlightLogValidator();
            var result = validator.Validate(log);

            if(!result.IsValid) {
                status.Success = false;
                foreach (var v in result.Errors)
                    status.AddMessage(v.ErrorMessage);

                return status;
            }

            const string sql = @"
                INSERT INTO [dbo].[FlightLog]
                       ([DroneId]
                       ,[DroneGeneration]
                       ,[BeginOn]
                       ,[EndOn]
                       ,[Latitude]
                       ,[Longitude]
                       ,[MapPath])
                 VALUES (
                        @DroneId
                       ,@DroneGeneration
                       ,@BeginOn
                       ,@EndOn
                       ,@Latitude
                       ,@Longitude
                       ,@MapPath)
            ";

            var flightId = await this.WithConnection<int>(c =>
                c.ExecuteAsync(sql, log)
                );

            log.FlightLogId = flightId;
            return GetActionStatus<FlightLog>().SetReturnData(log);
        }


        /// <summary>
        /// High-performance bulk-insert operation for the flight log
        /// table
        /// </summary>
        /// <param name="flights"></param>
        /// <returns></returns>
        public async Task<ActionStatus<bool>> BulkInsertFlightLog(IEnumerable<FlightLog> flights) {
            var status = GetActionStatus<bool>();

            var result = await this.WithConnection<bool>(async c => {
                var conn = (SqlConnection)c;

                using (var copy = new SqlBulkCopy(conn)) {
                    copy.DestinationTableName = "FlightLog";
                    DataTable table = BuildBulkFlightLogUpdateTable();

                    foreach (var f in flights) {
                        table.Rows.Add(
                            f.DroneId,
                            f.DroneGeneration,
                            f.BeginOn,
                            f.EndOn,
                            f.Latitude,
                            f.Longitude,
                            f.MapPath
                            );
                    }

                    await copy.WriteToServerAsync(table);

                    return true;
                }
            });

            return status.SetReturnData(result);
        }


        /// <summary>
        /// Construct a bulk upload table used for high-performance
        /// bulk inserts
        /// </summary>
        /// <returns></returns>
        private static DataTable BuildBulkFlightLogUpdateTable() {
            var table = new DataTable("FlightLog");

            table.Columns.Add("DroneId", typeof(int));
            table.Columns.Add("DroneGeneration", typeof(int));
            table.Columns.Add("BeginOn", typeof(DateTime));
            table.Columns.Add("EndOn", typeof(DateTime));
            table.Columns.Add("Latitude", typeof(double));
            table.Columns.Add("Longitude", typeof(double));
            table.Columns.Add("MapPath", typeof(string));

            return table;
        }


        /// <summary>
        /// Return the basics of a query that selects from the FlightLog table
        /// </summary>
        /// <returns></returns>
        private static StringBuilder GetSqlBuilder() {
            var builder = new StringBuilder();
            builder.AppendLine(FLIGHTLOG_SQL_BASE);

            return builder;
        }
    }
}
