using Microsoft.Extensions.Configuration;

namespace ShieldAI.Service.Test.Integration
{
    public static class SetupHelper
    {

        /// <summary>
        /// returns a configured flight log engine
        /// </summary>
        /// <returns></returns>
        public static IFlightEngine GetConfiguredFlightEngine()
        {
            var config = GetIConfigurationRoot();
            var engine = new FlightEngine(config);

            return engine;
        }


        /// <summary>
        /// returns a configured drone engine
        /// </summary>
        /// <returns></returns>
        public static IDroneEngine GetConfiguredDroneEngine()
        {
            var config = GetIConfigurationRoot();
            var engine = new DroneEngine(config);

            return engine;
        }


        /// <summary>
        /// returns a configured configuration object
        /// </summary>
        /// <returns></returns>
        private static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
