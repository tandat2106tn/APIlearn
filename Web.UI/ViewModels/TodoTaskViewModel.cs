namespace TaskManagerMVC.ViewModels
{
    public class TodoTaskViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid TaskTypeId { get; set; }
        public string TaskTypeName { get; set; }
        public Guid TaskDifficultyId { get; set; }
        public string TaskDifficultyName { get; set; }
    }
}