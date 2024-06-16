using FlightManagementSystem.Entities;

namespace FlightManagementSystem.UnitTests;

public class FakeUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrincipal = new ClaimsPrincipal();

        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, "test1@test.com"),
                new Claim("FullName", "test1"),
            }));

        context.HttpContext.User = claimsPrincipal;

        await next();
    }
}
