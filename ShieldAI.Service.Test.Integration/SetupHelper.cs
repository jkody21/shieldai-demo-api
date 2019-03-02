using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShieldAI.Service.Test.Integration
{
    public static class SetupHelper
    {
        public static IFlightEngine GetConfiguredFlightEngine()
        {
            var config = GetIConfigurationRoot();
            var engine = new FlightEngine(config);

            return engine;
        }


        public static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
