using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
	public static class DataSeeder
	{
		public static async Task SeedData(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
		{

			if (await context.Users.AnyAsync()) return;

			// Seed Roles
			if (!await roleManager.RoleExistsAsync("User"))
			{
				await roleManager.CreateAsync(new IdentityRole<Guid>("User"));
			}
			if (!await roleManager.RoleExistsAsync("Admin"))
			{
				await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
			}

			//Seed Admin

			if (await userManager.FindByEmailAsync("admin@example.com") == null)
			{
				var adminUser = new User
				{
					UserName = "admin@example.com",
					Email = "admin@example.com",
					Name = "Admin User"
				};

				var result = await userManager.CreateAsync(adminUser, "tandat2106Aa!");
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
			}



			// Seed Users
			var users = new List<User>
			{
				new User { Name = "Alice Johnson", Email = "alice@example.com", UserName = "alice@example.com" },
				new User { Name = "Bob Smith", Email = "bob@example.com", UserName = "bob@example.com" },
				new User { Name = "Charlie Brown", Email = "charlie@example.com", UserName = "charlie@example.com" }
			};

			foreach (var user in users)
			{
				var result = await userManager.CreateAsync(user, "tandat2106Aa!");
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "User");
				}
			}

			// Seed TaskTypes
			var taskTypes = new List<TaskType>
			{
				new TaskType { Name = "Work", Description = "Work related tasks" },
				new TaskType { Name = "Personal", Description = "Personal tasks" },
				new TaskType { Name = "Study", Description = "Study related tasks" }
			};
			await context.TaskTypes.AddRangeAsync(taskTypes);

			// Seed TaskDifficulties
			var taskDifficulties = new List<TaskDifficulty>
			{
				new TaskDifficulty { Name = "Easy", Description = "Simple tasks" },
				new TaskDifficulty { Name = "Medium", Description = "Moderate difficulty tasks" },
				new TaskDifficulty { Name = "Hard", Description = "Challenging tasks" }
			};
			await context.TaskDifficulties.AddRangeAsync(taskDifficulties);

			await context.SaveChangesAsync();

			// Seed TodoTasks
			var todoTasks = new List<TodoTask>
			{
				new TodoTask
				{
					Title = "Complete Project Proposal",
					Description = "Finish the project proposal for the client",
					IsCompleted = false,
					DueDate = DateTime.Now.AddDays(7),
					UserId = users[0].Id,
					TaskTypeId = taskTypes[0].Id,
					TaskDifficultyId = taskDifficulties[2].Id
				},
				new TodoTask
				{
					Title = "Go Grocery Shopping",
					Description = "Buy groceries for the week",
					IsCompleted = false,
					DueDate = DateTime.Now.AddDays(2),
					UserId = users[1].Id,
					TaskTypeId = taskTypes[1].Id,
					TaskDifficultyId = taskDifficulties[0].Id
				},
				new TodoTask
				{
					Title = "Study for Exam",
					Description = "Prepare for upcoming mathematics exam",
					IsCompleted = false,
					DueDate = DateTime.Now.AddDays(14),
					UserId = users[2].Id,
					TaskTypeId = taskTypes[2].Id,
					TaskDifficultyId = taskDifficulties[1].Id
				}
			};
			await context.Tasks.AddRangeAsync(todoTasks);

			await context.SaveChangesAsync();
		}
	}
}