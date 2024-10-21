namespace TaskManagerMVC.ViewModels
{
    public class CreateTodoTaskViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid TaskTypeId { get; set; }
        public Guid TaskDifficultyId { get; set; }
    }
}