using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	public DbSet<User> Users { get; set; }
	public DbSet<TodoTask> Tasks { get; set; }
	public DbSet<TaskType> TaskTypes { get; set; }
	public DbSet<TaskDifficulty> TaskDifficulties { get; set; }

	public DbSet<Reminder> Reminders { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Cấu hình relationships nếu cần
		modelBuilder.Entity<TodoTask>()
			.HasOne(t => t.User)
			.WithMany(u => u.Tasks)
			.HasForeignKey(t => t.UserId);

		modelBuilder.Entity<TodoTask>()
			.HasOne(t => t.TaskType)
			.WithMany(tt => tt.Tasks)
			.HasForeignKey(t => t.TaskTypeId);

		modelBuilder.Entity<TodoTask>()
			.HasOne(t => t.TaskDifficulty)
			.WithMany(td => td.Tasks)
			.HasForeignKey(t => t.TaskDifficultyId);

		modelBuilder.Entity<Reminder>()
		.HasOne(r => r.Task)
		.WithMany(t => t.Reminders)
		.HasForeignKey(r => r.TodoTaskId)
		.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Reminder>()
		.Property(r => r.TodoTaskId)
		.IsRequired();
	}
}