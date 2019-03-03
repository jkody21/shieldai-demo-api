using ShieldAI.Service.Data.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShieldAI.Service.Data.Validators {
    public class FlightLogValidator : AbstractValidator<FlightLog>
    {
        private readonly List<Drone> _validDrones;

        public FlightLogValidator(List<Drone> validDrones) {
            _validDrones = validDrones;

            RuleFor(x => x.DroneId).NotEmpty().WithMessage("DroneId is required");
            RuleFor(x => x.DroneId).Custom((id, context) => {
                if (!_validDrones.Any(d => d.DroneId.Equals(id)))
                    context.AddFailure("The drone ID must be for a valid and registered drone");
            });

            RuleFor(x => x.DroneGeneration).NotEmpty().WithMessage("DroneGeneration is required");
            RuleFor(x => x.BeginOn).NotEmpty();
            RuleFor(x => x.EndOn).NotEmpty();
            RuleFor(x => x.BeginOn).LessThan(DateTime.Now).WithMessage("Drone flights cannot begin in the future");
            RuleFor(x => x.EndOn).LessThan(DateTime.Now).WithMessage("Drone flights cannot end in the future");
            RuleFor(m => m.EndOn).Must((model, field) => field >= model.BeginOn && field <= model.EndOn).WithMessage("EndOn date/time must be after the BeginOn date/time");
            RuleFor(x => x.Longitude).InclusiveBetween(-180.0, 180.0).WithMessage("Longitude is not in a valid range");
            RuleFor(x => x.Latitude).InclusiveBetween(-90.0, 90.0).WithMessage("Latitude is not in a valid range"); ;
        }
    }
}
