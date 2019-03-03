using ShieldAI.Core;
using ShieldAI.Service.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShieldAI.Service
{
    public interface IDroneEngine
    {
        Task<ActionStatus<IEnumerable<Drone>>> FindDrones();
    }
}
