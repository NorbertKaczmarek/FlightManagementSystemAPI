using FlightManagementSystem.Entities;
using FluentValidation;

namespace FlightManagementSystem.Models.Validators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator(FlightManagementDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
