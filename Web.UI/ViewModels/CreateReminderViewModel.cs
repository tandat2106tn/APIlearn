namespace TaskManagerMVC.ViewModels
{
    public class CreateReminderViewModel
    {
        public string Message { get; set; }
        public DateTime ReminderTime { get; set; }
        public Guid TodoTaskId { get; set; }
    }
}