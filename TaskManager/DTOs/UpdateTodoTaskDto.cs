namespace TaskManager.DTOs
{
	public class UpdateTodoTaskDto
	{

		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime DueDate { get; set; }
		public Guid TaskTypeId { get; set; }
		public Guid TaskDifficultyId { get; set; }

	}
}
