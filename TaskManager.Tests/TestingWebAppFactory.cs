using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TaskManager.Tests
{
    public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                    db.Database.EnsureCreated();

                    // Seed the database with test data if needed
                    SeedTestData(db);
                }
            });
        }

        private void SeedTestData(ApplicationDbContext context)
        {
            // Add test data to the in-memory database
            var user = new TaskManager.Models.User { Id = Guid.NewGuid(), Name = "Test User", Email = "test@example.com" };
            context.Users.Add(user);

            var taskType = new TaskManager.Models.TaskType { Id = Guid.NewGuid(), Name = "Test Type", Description = "Test Description" };
            context.TaskTypes.Add(taskType);

            var taskDifficulty = new TaskManager.Models.TaskDifficulty { Id = Guid.NewGuid(), Name = "Test Difficulty", Description = "Test Description" };
            context.TaskDifficulties.Add(taskDifficulty);

            var task = new TaskManager.Models.TodoTask
            {
                Id = Guid.NewGuid(),
                Title = "Test Task",
                Description = "Test Description",
                IsCompleted = false,
                DueDate = DateTime.Now.AddDays(1),
                UserId = user.Id,
                TaskTypeId = taskType.Id,
                TaskDifficultyId = taskDifficulty.Id
            };
            context.Tasks.Add(task);

            context.SaveChanges();
        }
    }
}