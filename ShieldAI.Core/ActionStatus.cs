using System;
using System.Collections.Generic;


namespace ShieldAI.Core {
    public class ActionStatus<T>
    {
        public ActionStatus() {
            this.Messages = new List<string>();
            this.StatusCode = 200;
            this.BeginTime = DateTime.Now;
        }


        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public List<String> Messages { get; set; }
        public T ReturnData { get; set; }
        public TimeSpan ProcessTime { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }


        /// <summary>
        /// Merges the specified status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public ActionStatus<T> Merge(ActionStatus<T> status) {
            if (!status.Success)
                this.Success = status.Success;

            foreach (var m in status.Messages)
                this.Messages.Add(m);

            return this;
        }


        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public ActionStatus<T> AddMessage(string message) {
            this.Messages.Add(message);

            return this;
        }


        /// <summary>
        /// Sets the return data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public ActionStatus<T> SetReturnData(T data) {
            this.ReturnData = data;

            return this;
        }


        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public ActionStatus<T> HandleException(Exception ex) {
            this.Success = false;

            this.Messages.Add(ex.Message);

            var e = ex;

            while (e.InnerException != null) {
                e = e.InnerException;
                this.Messages.Add(e.Message);
            }

            return this;
        }


        public TimeSpan GetProcessTime() {
            if (EndTime == DateTime.MinValue)
                EndTime = DateTime.Now;

            return EndTime.Subtract(BeginTime);
        }
    }
}
