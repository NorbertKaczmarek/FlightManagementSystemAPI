using FluentValidation;
using FlightManagementSystem.Entities;
using FlightManagementSystem.Models;

namespace FlightManagementSystem.Models.Validators
{
    public class CreateFlightDtoValidator : AbstractValidator<CreateFlightDto>
    {
        public CreateFlightDtoValidator(FlightManagementDbContext dbContext) 
        {
            RuleFor(x => x.NumerLotu)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var NumerLotuInUse = dbContext.Flights.Any(u => u.NumerLotu == value);
                    
                    // cant edit bc it disables it
                    //if (NumerLotuInUse)
                    //{
                    //    context.AddFailure("NumerLotu", "NumerLotu already exists.");
                    //}
                });

            RuleFor(x => x.MiejsceWylotu)
                .NotNull()
                .NotEmpty();
            
            RuleFor(x => x.MiejscePrzylotu)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.TypSamolotu)
                .IsInEnum();
        }
    }
}
