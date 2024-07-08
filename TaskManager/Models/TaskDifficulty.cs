namespace TaskManager.Models
{
	public class TaskDifficulty
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<TodoTask> Tasks { get; set; }
	}
}
