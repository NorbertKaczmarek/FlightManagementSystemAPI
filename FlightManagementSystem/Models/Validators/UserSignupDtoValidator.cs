using FlightManagementSystem.Entities;
using FluentValidation;

namespace FlightManagementSystem.Models.Validators
{
    public class UserSignupDtoValidator : AbstractValidator<UserSignupDto>
    {
        public UserSignupDtoValidator(FlightManagementDbContext dbContext)
        { 
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.FullName)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty();
        }
    }
}
