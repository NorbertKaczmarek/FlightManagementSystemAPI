namespace FlightManagementSystem.UnitTests;

public class AuthControllerAuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    public AuthControllerAuthTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<FlightManagementDbContext>));

                    services.Remove(dbContextOptions);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                    services.AddDbContext<FlightManagementDbContext>(options => options.UseInMemoryDatabase("AuthControllerAuthTests"));
                });
            });

        _client = _factory.CreateClient();
    }

    private void SeedUser(User user)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<FlightManagementDbContext>();

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    [Theory]
    [InlineData("/auth/account")]
    // checks with hard coded claimes
    public async Task Get_Endpoints_ReturnSuccessAndCorrectContentType(string url)
    {
        // arrange
        string email = "test1@test.com";
        string fullName = "test1";
        string password = "tDest3@!#!123";

        User newUser = new() { Email = email, FullName = fullName };
        newUser.PasswordHash = password.HashedPassword(newUser);
        SeedUser(newUser);

        // act
        var response = await _client.GetAsync(url);

        // assert
        response.EnsureSuccessStatusCode();
    }
}
