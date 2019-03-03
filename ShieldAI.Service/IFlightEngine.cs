using ShieldAI.Core;
using ShieldAI.Service.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShieldAI.Service {
    public interface IFlightEngine
    {
        Task<ActionStatus<IEnumerable<FlightLog>>> FindFlights(FlightLogRequest request);
        Task<ActionStatus<IEnumerable<FlightLog>>> FindFlights();

        Task<ActionStatus<FlightLogMetrics>> GetFlightLogMetrics();

        Task<ActionStatus<FlightLog>> GetFlight(int id);

        Task<ActionStatus<FlightLog>> InsertFlightLog(FlightLog log);

        Task<ActionStatus<bool>> BulkInsertFlightLog(IEnumerable<FlightLog> flights);
    }
}
