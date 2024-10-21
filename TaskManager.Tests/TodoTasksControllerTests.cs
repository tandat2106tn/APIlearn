using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManager.DTOs;
using TaskManager.Repositories;

namespace TaskManager.Tests
{
    public class TodoTasksControllerTests
    {
        private readonly Mock<ITodoTaskRepository> mockRepo;
        private readonly TodoTasksController controller;

        public TodoTasksControllerTests()
        {
            mockRepo = new Mock<ITodoTaskRepository>();
            controller = new TodoTasksController(mockRepo.Object, null);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOkResult_WithListOfTasks()
        {
            // Arrange
            var tasks = new List<TodoTaskDto>
            {
                new TodoTaskDto { Id = Guid.NewGuid(), Title = "Task 1" },
                new TodoTaskDto { Id = Guid.NewGuid(), Title = "Task 2" }
            };
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await controller.GetAllTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<TodoTaskDto>>(okResult.Value);
            Assert.Equal(2, returnedTasks.Count());
        }
    }
}