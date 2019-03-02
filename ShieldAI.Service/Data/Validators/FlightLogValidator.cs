using ShieldAI.Service.Data.Model;
using FluentValidation;

namespace ShieldAI.Service.Data.Validators {
    public class FlightLogValidator : AbstractValidator<FlightLog>
    {
        public FlightLogValidator() {
            RuleFor(x => x.DroneId).NotEmpty().WithMessage("DroneId is required");
            RuleFor(x => x.DroneGeneration).NotEmpty().WithMessage("DroneGeneration is required");
            RuleFor(x => x.BeginOn).NotEmpty();
            RuleFor(x => x.EndOn).NotEmpty();
            RuleFor(x => x.Longitude).InclusiveBetween(-90, 90).WithMessage("Longitude is not in a valid range");
            RuleFor(x => x.Latitude).InclusiveBetween(-180, 180).WithMessage("Latitude is not in a valid range"); ;
        }
    }
}
