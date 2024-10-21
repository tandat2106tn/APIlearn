namespace TaskManagerMVC.ViewModels
{
    public class UpdateReminderViewModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime ReminderTime { get; set; }
        public bool IsTriggered { get; set; }
    }
}