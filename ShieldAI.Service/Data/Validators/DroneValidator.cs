using FluentValidation;
using ShieldAI.Service.Data.Model;

namespace ShieldAI.Service.Data.Validators
{
    public class DroneValidator : AbstractValidator<Drone>
    {
        public DroneValidator() {
            RuleFor(x => x.DroneId).NotEmpty().WithMessage("DroneId is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("This drone needs a name");
            RuleFor(x => x.CurrentGeneration).NotEmpty().WithMessage("What generation is this drone");
        }
    }
}
