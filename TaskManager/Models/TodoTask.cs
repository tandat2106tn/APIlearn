namespace TaskManager.Models
{
	public class TodoTask
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime DueDate { get; set; }


		public Guid UserId { get; set; }
		public User User { get; set; }


		public Guid TaskTypeId { get; set; }
		public TaskType TaskType { get; set; }


		public Guid TaskDifficultyId { get; set; }
		public TaskDifficulty TaskDifficulty { get; set; }

	}
}
