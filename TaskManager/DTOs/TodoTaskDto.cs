namespace TaskManager.DTOs
{
	public class TodoTaskDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime DueDate { get; set; }

		public Guid UserId { get; set; }
		public Guid TaskTypeId { get; set; }
		public Guid TaskDifficultyId { get; set; }

		public string UserName { get; set; }
		public string TaskTypeName { get; set; }
		public string TaskDifficultyName { get; set; }

		public List<ReminderDto> Reminders { get; set; }

	}
}
