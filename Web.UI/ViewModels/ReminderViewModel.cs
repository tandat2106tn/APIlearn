namespace TaskManagerMVC.ViewModels
{
    public class ReminderViewModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime ReminderTime { get; set; }
        public bool IsTriggered { get; set; }
        public Guid TodoTaskId { get; set; }
    }
}