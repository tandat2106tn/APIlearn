using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Repositories;

namespace TaskManager.Tests
{
    public class TodoTaskRepositoryTests
    {
        private readonly Mock<UnitOfWork> mockUnitOfWork;
        private readonly Mock<IMapper> mockMapper;
        private readonly TodoTaskRepository repository;
        private readonly Mock<DbSet<TodoTask>> mockSet;

        public TodoTaskRepositoryTests()
        {
            mockUnitOfWork = new Mock<UnitOfWork>();
            mockMapper = new Mock<IMapper>();
            mockSet = new Mock<DbSet<TodoTask>>();

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Tasks).Returns(mockSet.Object);
            mockUnitOfWork.Setup(uow => uow.dbContext).Returns(mockContext.Object);

            repository = new TodoTaskRepository(mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var tasks = new List<TodoTask>
            {
                new TodoTask { Id = Guid.NewGuid(), Title = "Task 1" },
                new TodoTask { Id = Guid.NewGuid(), Title = "Task 2" }
            };
            var taskDtos = new List<TodoTaskDto>
            {
                new TodoTaskDto { Id = tasks[0].Id, Title = "Task 1" },
                new TodoTaskDto { Id = tasks[1].Id, Title = "Task 2" }
            };

            mockSet.As<IQueryable<TodoTask>>().Setup(m => m.Provider).Returns(tasks.AsQueryable().Provider);
            mockSet.As<IQueryable<TodoTask>>().Setup(m => m.Expression).Returns(tasks.AsQueryable().Expression);
            mockSet.As<IQueryable<TodoTask>>().Setup(m => m.ElementType).Returns(tasks.AsQueryable().ElementType);
            mockSet.As<IQueryable<TodoTask>>().Setup(m => m.GetEnumerator()).Returns(tasks.AsQueryable().GetEnumerator());

            mockUnitOfWork.Setup(uow => uow.mapper.Map<IEnumerable<TodoTaskDto>>(It.IsAny<IEnumerable<TodoTask>>())).Returns(taskDtos);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Task 1", result.First().Title);
            Assert.Equal("Task 2", result.Last().Title);
        }
    }
}