using MediatR;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Tasks.Commands;
using ToDoApp.Domain.Models;
using ToDoApp.Domain.Enums;
namespace ToDoApp.Application.Tasks.Handlers;

public class CreateTaskCommandHandler: IRequestHandler<CreateTaskCommand, TaskItem>
{
    private readonly IAppDbContext _context;
    public CreateTaskCommandHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<TaskItem> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        TaskItem task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Deadline = request.Deadline,
            Status = request.Status ?? TaskProgressStatus.ToDo,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync(cancellationToken);
        return task;
    }
}
