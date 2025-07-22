using FluentValidation;
using ToDoApp.Application.Tasks.Commands;

namespace ToDoApp.API.Validators
{
    public class UpdateTaskCommandValidator: AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            When(task => task.Title is not null, () =>
            {
                RuleFor(task => task.Title)
                    .NotEmpty().WithMessage("Title can't be empty")
                    .MaximumLength(50).WithMessage("Max length of title is 50 symbols");
            });
            When(task => task.Description is not null, () =>
            {
                RuleFor(task => task.Description).MaximumLength(500).WithMessage("Maximum length of description is 500 symbols");
            });
            When(task => task.Deadline.HasValue, () =>
            {
                RuleFor(task => task.Deadline!.Value).GreaterThanOrEqualTo(DateTime.UtcNow.Date.AddDays(-1)).WithMessage("Deadline must be in the future");
            });
            RuleFor(task => task.Status)
                .IsInEnum().When(task => task.Status.HasValue)
                .WithMessage("Invalid status value");
        }
    }
}
