using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShieldAI.Service.Test.Integration
{
    [TestClass]
    public class DroneEngineTests
    {
        [TestMethod]
        public async Task should_find_active_drones()
        {
            var engine = SetupHelper.GetConfiguredDroneEngine();

            var result = await engine.FindDrones();

            result.Should().NotBeNull();
        }
    }
}
