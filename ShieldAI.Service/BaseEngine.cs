using ShieldAI.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace ShieldAI.Service
{
    public abstract class BaseEngine
    {
        private IConfiguration _config;

        public BaseEngine(IConfiguration configuration) {
            _config = configuration;
        }

        /// <summary>
        /// Gen up a connection and do a thing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getData"></param>
        /// <returns></returns>
        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData) {
            var connectionString = _config["AppSettings:ConnectionString"];

            try {
                using (var connection = new SqlConnection(connectionString)) {
                    await connection.OpenAsync(); // Asynchronously open a connection to the database
                    return await getData(connection); // Asynchronously execute getData, which has been passed in as a Func<IDBConnection, Task<T>>
                }
            } catch (TimeoutException ex) {
                throw new Exception(
                    $"{GetType().FullName}.WithConnection() experienced a SQL timeout",
                    ex);
            } catch (SqlException ex) {
                throw new Exception(
                    $"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)",
                    ex);
            }
        }


        /// <summary>
        /// Gets the action status.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        protected ActionStatus<T> GetActionStatus<T>(bool success, int statusCode = 100, params string[] messages) {
            var status = new ActionStatus<T> { Messages = new List<string>(), Success = success, StatusCode = statusCode };

            foreach (var m in messages)
                status.Messages.Add(m);

            return status;
        }
        protected ActionStatus<T> GetActionStatus<T>(Exception ex) {
            return GetActionStatus<T>(false).HandleException(ex);
        }
        protected ActionStatus<T> GetActionStatus<T>(ActionStatus<T> status) {
            return GetActionStatus<T>(true).Merge(status);
        }
        protected ActionStatus<T> GetActionStatus<T>() {
            return GetActionStatus<T>(true);
        }


        /// <summary>
        /// Gets the action status.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        /// <param name="statusCode">The status code.</param>
        /// <returns></returns>
        protected ActionStatus<T> GetActionStatus<T>(bool success, string message, int statusCode = 100) {
            var status = new ActionStatus<T> { Messages = new List<string>(), Success = success, StatusCode = statusCode };

            status.Messages.Add(message);

            return status;
        }

    }
}
