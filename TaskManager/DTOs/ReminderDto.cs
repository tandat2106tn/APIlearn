namespace TaskManager.DTOs
{
	public class ReminderDto
	{
		public Guid Id { get; set; }
		public string Message { get; set; }
		public DateTime ReminderTime { get; set; }
		public bool IsTriggered { get; set; }
		public Guid TodoTaskId { get; set; }
	}

	public class CreateReminderDto
	{
		public string Message { get; set; }
		public DateTime ReminderTime { get; set; }
		public Guid TodoTaskId { get; set; }
	}

	public class UpdateReminderDto
	{
		public DateTime ReminderTime { get; set; }
		public bool IsTriggered { get; set; }
		public string Message { get; set; }
	}
}