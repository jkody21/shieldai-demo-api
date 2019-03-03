using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShieldAI.Core;
using ShieldAI.Service.Data.Model;
using ShieldAI.Service.Data.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldAI.Service {
    public class FlightEngine : BaseEngine, IFlightEngine {

        private readonly IDroneEngine _droneEngine;
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


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public FlightEngine(
            IConfiguration configuration,
            IDroneEngine droneEngine) : base(configuration) {

            _droneEngine = droneEngine;
        }


        /// <summary>
        /// Retrieve a list of flights based on input parameters
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionStatus<IEnumerable<FlightLog>>> FindFlights(FlightLogRequest request)
        {
            var status = new ActionStatus<IEnumerable<FlightLog>>();

            var sql = GetSqlBuilder();
            ApplyFiltersToSql(request, sql);

            var flightLogs =
                await WithConnection<IEnumerable<FlightLog>>(
                    async c =>
                        await c.QueryAsync<FlightLog>(sql.ToString(), request)
                );

            var flightLogList = flightLogs.ToList();

            status.SetReturnData(flightLogList);
            return status;
        }
        public async Task<ActionStatus<IEnumerable<FlightLog>>> FindFlights()
        {
            return await FindFlights(null);
        }


        /// <summary>
        /// return flight-log metrics
        /// </summary>
        /// <returns></returns>
        public async Task<ActionStatus<FlightLogMetrics>> GetFlightLogMetrics()
        {
            var status = GetActionStatus<FlightLogMetrics>();

            var metrics =
               await WithConnection<IEnumerable<FlightLogMetrics>>(
                   async c =>
                       await c.QueryAsync<FlightLogMetrics>("usp_GetFlightLogMetrics", null, null, null, CommandType.StoredProcedure)
               );

            status.SetReturnData(metrics.FirstOrDefault());

            return status;
        }


        /// <summary>
        /// Build the SQL statement based on the values contained in the 
        /// request object
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sql"></param>
        private static void ApplyFiltersToSql(FlightLogRequest request, StringBuilder sql)
        {
            if (request.IsNotNull())
            {
                if (request.DroneId.HasValue)
                    sql.AppendLine("AND DroneId = @DroneId");

                if (request.From.HasValue)
                    sql.AppendLine("AND BeginOn >= @From");

                if (request.To.HasValue)
                    sql.AppendLine("AND EndOn <= @To");

                if (request.DroneGeneration.HasValue)
                    sql.AppendLine("AND DroneGeneration = @DroneGeneration");

                if (request.DurationLow.HasValue)
                    sql.AppendLine("AND DATEDIFF(mi, BeginOn, EndOn) > @DurationLow");

                if(request.DurationHigh.HasValue)
                    sql.AppendLine("AND DATEDIFF(mi, BeginOn, EndOn) < @DurationHigh");

                var box = GetBoundingBox(request);

                if(box.IsNotNull())
                {
                    request.MinLatitude = box.MinimumLatitude;
                    request.MaxLatitude = box.MaximumLatitude;
                    request.MinLongitude = box.MinimumLongitude;
                    request.MaxLongitude = box.MaximumLongitude;

                    sql.AppendLine("AND Latitude BETWEEN @MinLatitude AND @MaxLatitude");
                    sql.AppendLine("AND Longitude BETWEEN @MinLongitude AND @MaxLongitude");
                }
            }
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
            var drones = await _droneEngine.FindDrones();

            var validator = new FlightLogValidator(drones.ReturnData.ToList());
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
                       ,@MapPath);
                SELECT SCOPE_IDENTITY();
            ";

            var flightId = await this.WithConnection<int>(async c => {
                var id =  await c.QueryAsync<int>(sql, log);
                return id.FirstOrDefault();
                });

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
            var drones = await _droneEngine.FindDrones();
            var hasErrors = false;
            var successCount = 0;
            var failCount = 0;

            var validator = new FlightLogValidator(drones.ReturnData.ToList());

            try { 
                var result = await this.WithConnection<bool>(async c => {
                    var conn = (SqlConnection)c;

                    using (var copy = new SqlBulkCopy(conn)) {
                        copy.DestinationTableName = "FlightLog";
                        var table = BuildBulkFlightLogUpdateTable();

                        foreach (var f in flights) {
                            var isOk = validator.Validate(f);
                            if(!isOk.IsValid)
                            {
                                failCount++;
                                hasErrors = true;
                                status.AddMessage($"Flight not valid.  Skipped in bulk operation. {JsonConvert.SerializeObject(f)}.  {JsonConvert.SerializeObject(isOk)}");
                                continue;
                            }

                            successCount++;
                            table.Rows.Add(
                                0,
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

                if(hasErrors) {
                    status.StatusCode = 400;
                    status.AddMessage($"One or more bulk entries could not be added due to validation issues. {successCount} succeeded / {failCount} failed.");
                 }

                return status.SetReturnData(result);
            } catch(Exception ex) {
                var msg = ex.Message;
                throw;
            }
        }


        /// <summary>
        /// Construct a bulk upload table used for high-performance
        /// bulk inserts
        /// </summary>
        /// <returns></returns>
        private static DataTable BuildBulkFlightLogUpdateTable() {
            var table = new DataTable("FlightLog");

            table.Columns.Add("FlightLogId", typeof(long));
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


        /// <summary>
        /// Checks to see if a bounding box can be created.
        /// If possible, return the result
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static BoundingBox GetBoundingBox(FlightLogRequest request)
        {
            if (!request.CanCreateBoundingBox)
                return null;

            var box = GeoHelper.GetBoundingBox(
                request.Longitude.Value,
                request.Latitude.Value,
                request.Distance.Value);

            return box;

        }
    }
}
