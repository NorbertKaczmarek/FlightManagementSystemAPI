using FlightManagementSystem.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FlightManagementSystem.Entities;
using FlightManagementSystem.Models;
using FlightManagementSystem.Services;

namespace FlightManagementSystem.IntegrationTests
{
    public class FlightControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        public FlightControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<FlightManagementDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddDbContext<FlightManagementDbContext>(options => options.UseInMemoryDatabase("FlightManagementDb"));
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
        }

        [Fact]
        public async Task Delete_Flight_ReturnsNoContent()
        {
            // arrange
            var flight = new Flight()
            {
                NumerLotu = 1,
                DataWylotu = new DateTime(2004, 12, 5),
                MiejsceWylotu = "Warszawa",
                MiejscePrzylotu = "Gdańsk",
                TypSamolotu = PlaneType.Boeing
            };

            // seed
            SeedFlight(flight);

            // act
            var response = await _client.DeleteAsync("/flight/" + flight.Id);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ForNonExistingFlight_ReturnsNotFound()
        {
            // act
            var response = await _client.DeleteAsync("/flight/115");

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(11, "16.04.2024 14:30", "Warszawa", "Gdańsk", PlaneType.Boeing)]
        [InlineData(22, "23.05.2021 07:10", "Poznań", "Gdańsk", PlaneType.Embraer)]
        [InlineData(33, "", "Wrocław", "Toruń", PlaneType.Airbus)]
        public async Task CreateFlight_WithValidModel_ReturnsCreatedStatus(
            int numerLotu, string dataWylotuString, string miejsceWylotu, string miejscePrzylotu, PlaneType typSamolotu)
        {
            // arrange
            var model = new CreateFlightDto()
            {
                NumerLotu = numerLotu,
                DataWylotuString = dataWylotuString,
                MiejsceWylotu = miejsceWylotu,
                MiejscePrzylotu = miejscePrzylotu,
                TypSamolotu = typSamolotu
            };

            var httpContent = model.ToJsonHttpContent();

            // act
            var response = await _client.PostAsync("/flight", httpContent);

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1, "16.04.2024 14:30", "", "Gdańsk", PlaneType.Boeing)]
        [InlineData(2, "23.05.2021 07:10", "Poznań", "", PlaneType.Embraer)]
        [InlineData(3, "nodate", "Poznań", "Gdańsk", PlaneType.Airbus)]
        public async Task CreateFlight_WithInValidModel_ReturnsCreatedStatus(
            int numerLotu, string dataWylotuString, string miejsceWylotu, string miejscePrzylotu, PlaneType typSamolotu)
        {
            // arrange
            var flight = new CreateFlightDto()
            {
                NumerLotu = numerLotu,
                DataWylotuString = dataWylotuString,
                MiejsceWylotu = miejsceWylotu,
                MiejscePrzylotu = miejscePrzylotu,
                TypSamolotu = typSamolotu
            };

            var httpContent = flight.ToJsonHttpContent();

            // act
            var response = await _client.PostAsync("/flight", httpContent);

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateFlight_WithExistingNumber_ReturnsBadRequest()
        {
            // arrange
            var flight = new Flight()
            {
                NumerLotu = 111,
                DataWylotu = new DateTime(2004, 12, 5),
                MiejsceWylotu = "Warszawa",
                MiejscePrzylotu = "Gdańsk",
                TypSamolotu = PlaneType.Boeing
            };

            var newFlight = new CreateFlightDto()
            {
                NumerLotu = 111,
                DataWylotuString = "23.05.2021 09:10",
                MiejsceWylotu = "Kraków",
                MiejscePrzylotu = "Lublin",
                TypSamolotu = PlaneType.Airbus
            };

            var httpContent = newFlight.ToJsonHttpContent();

            // seed
            SeedFlight(flight);

            // act
            var response = await _client.PostAsync("/flight", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

    }
}
