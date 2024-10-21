namespace TaskManager.Models
{
	public class Reminder
	{
		public Guid Id { get; set; }
		public string Message { get; set; }
		public DateTime ReminderTime { get; set; }
		public bool IsTriggered { get; set; }
		public Guid TodoTaskId { get; set; }
		public TodoTask Task { get; set; }
	}
}
