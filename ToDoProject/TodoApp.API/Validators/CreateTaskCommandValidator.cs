using FluentValidation;
using ToDoApp.Application.Tasks.Commands;
namespace ToDoApp.API.Validators
{
    public class CreateTaskCommandValidator: AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty().WithMessage("Task title is compulsory")
                .MaximumLength(50).WithMessage("Maximum length of task title is 50 symbols");

            RuleFor(task => task.Description)
                .MaximumLength(500).WithMessage("Description can't be greater than 500 symbols");


            RuleFor(task => task.Deadline).GreaterThan(DateTime.UtcNow).WithMessage("Deadline must be in the future")
                .When(task => task.Deadline.HasValue);
        }
    }
}
