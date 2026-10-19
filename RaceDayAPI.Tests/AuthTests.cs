using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RaceDay_API.Data;
using RaceDay_API.DTOs;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace RaceDayAPI.Tests
{
    public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthTests(WebApplicationFactory<Program> factory)
        {
            // A completely unique database name for this specific test class instance
            var dbName = $"TestDb_{Guid.NewGuid()}";

            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.UseSolutionRelativeContentRoot("RaceDay_API");

                builder.ConfigureServices(services =>
                {
                    // Remove SQL Server
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RaceDayDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    // Add isolated In-Memory DB
                    services.AddDbContext<RaceDayDbContext>(options =>
                        options.UseInMemoryDatabase(dbName));
                });
            }).CreateClient();
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsSuccess()
        {
            var dto = new RegisterDto { Name = "Test User", Email = "register_test@test.com", Password = "Password123!" };
            var response = await _client.PostAsJsonAsync("/api/auth/register", dto);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var email = "login_test@test.com";
            var dto = new RegisterDto { Name = "Test", Email = email, Password = "Password123!" };
            var regRes = await _client.PostAsJsonAsync("/api/auth/register", dto);
            Assert.True(regRes.IsSuccessStatusCode, "Registration failed.");

            var loginDto = new LoginDto { Email = email, Password = "Password123!" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var errorContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.True(response.IsSuccessStatusCode, $"Failed with status {response.StatusCode}. Response: {errorContent}");

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var loginDto = new LoginDto { Email = "nonexistent@test.com", Password = "WrongPassword!" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateEvent_ParticipantRole_ReturnsForbidden()
        {
            // Arrange
            var email = "forbidden_test@test.com";
            var dto = new RegisterDto { Name = "Test", Email = email, Password = "Password123!" };
            var regRes = await _client.PostAsJsonAsync("/api/auth/register", dto);
            Assert.True(regRes.IsSuccessStatusCode, "Registration failed.");

            var loginDto = new LoginDto { Email = email, Password = "Password123!" };
            var loginRes = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            Assert.True(loginRes.IsSuccessStatusCode, "Login failed.");

            var auth = await loginRes.Content.ReadFromJsonAsync<AuthResponseDto>();
            Assert.NotNull(auth);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.Token);

            // Act
            var eventDto = new EventCreateDto { Name = "Test Event", Location = "JHB", EventDate = DateTime.UtcNow.AddDays(5) };
            var response = await _client.PostAsJsonAsync("/api/events", eventDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}