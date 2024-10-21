using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace TaskManager.Tests
{
    public class TodoTaskControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;

        public TodoTaskControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllTasks_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = "/api/todotasks";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetTaskById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var taskId = Guid.NewGuid(); // Replace with a known task ID from your test data
            var request = $"/api/todotasks/{taskId}";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetTaskById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidTaskId = Guid.NewGuid();
            var request = $"/api/todotasks/{invalidTaskId}";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}