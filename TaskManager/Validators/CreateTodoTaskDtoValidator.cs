using FluentValidation;
using TaskManager.DTOs;

namespace TaskManager.Validators
{
    public class CreateTodoTaskDtoValidator : AbstractValidator<CreateTodoTaskDto>
    {
        public CreateTodoTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("DueDate is required.")
                .GreaterThan(DateTime.Now)
                .WithMessage("Due date must be in the future.");



            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.TaskTypeId)
                .NotEmpty().WithMessage("Task Type ID is required.");

            RuleFor(x => x.TaskDifficultyId)
                .NotEmpty().WithMessage("Task Difficulty ID is required.");
        }
    }
}