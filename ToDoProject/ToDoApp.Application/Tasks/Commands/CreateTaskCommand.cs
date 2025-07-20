using MediatR;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.Models;

namespace ToDoApp.Application.Tasks.Commands;

public class CreateTaskCommand: IRequest<TaskItem>
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public TaskProgressStatus? Status { get; set; }
    public DateTime? Deadline { get; set; }
}
