﻿namespace FlightManagementSystem.UnitTests;

public class FlightControllerTestsWithoutAuthentication : IClassFixture<WebApplicationFactory<Program>>
{

    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    public FlightControllerTestsWithoutAuthentication(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<FlightManagementDbContext>));

                    services.Remove(dbContextOptions);

                    services.AddDbContext<FlightManagementDbContext>(options => options.UseInMemoryDatabase("FlightManagementDbWithoutAnthentication"));
                });
            });

        _client = _factory.CreateClient();
    }

    private void SeedFlight(Flight flight)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<FlightManagementDbContext>();

        _dbContext.Flights.Add(flight);
        _dbContext.SaveChanges();
        Console.WriteLine(flight.Id);
    }

    [Theory]
    [InlineData("/flight")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // act
        var response = await _client.GetAsync(url);

        // assert
        //response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }

    [Fact]
    public async Task Get_ForNonExistingFlight_ReturnsNotFound()
    {
        // act
        var response = await _client.GetAsync("/flight/55");

        // assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task Delete_Flight_ReturnsUnauthorized()
    {
        // act
        var response = await _client.DeleteAsync("/flight/1");

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }


    [Theory]
    [InlineData(1, "16.04.2024 14:30", "Warszawa", "Gdańsk", PlaneType.Boeing)]
    public async Task Edit_Flight_ReturnsUnauthorized(
        int numerLotu, string dataWylotuString, string miejsceWylotu, string miejscePrzylotu, PlaneType typSamolotu)
    {
        // arrange
        var model = new FlightCreateDto()
        {
            NumerLotu = numerLotu,
            DataWylotu = DateTime.ParseExact(dataWylotuString, "dd.MM.yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture),
            MiejsceWylotu = miejsceWylotu,
            MiejscePrzylotu = miejscePrzylotu,
            TypSamolotu = typSamolotu
        };

        var httpContent = model.ToJsonHttpContent();

        // act
        var response = await _client.PostAsync("/flight/1", httpContent);

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData(1, "16.04.2024 14:30", "Warszawa", "Gdańsk", PlaneType.Boeing)]
    public async Task CreateFlight_WithValidModel_ReturnsUnauthorized(
        int numerLotu, string dataWylotuString, string miejsceWylotu, string miejscePrzylotu, PlaneType typSamolotu)
    {
        // arrange
        var model = new FlightCreateDto()
        {
            NumerLotu = numerLotu,
            DataWylotu = DateTime.ParseExact(dataWylotuString, "dd.MM.yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture),
            MiejsceWylotu = miejsceWylotu,
            MiejscePrzylotu = miejscePrzylotu,
            TypSamolotu = typSamolotu
        };

        var httpContent = model.ToJsonHttpContent();

        // act
        var response = await _client.PostAsync("/flight", httpContent);

        // assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}
