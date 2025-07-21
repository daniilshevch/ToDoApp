using MediatR;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Tasks.Commands;
using ToDoApp.Domain.Models;
using ToDoApp.Domain.Enums;
namespace ToDoApp.Application.Tasks.Handlers;

public class CreateTaskCommandHandler: IRequestHandler<CreateTaskCommand, TaskItem>
{
    private readonly ITaskRepository _repository;
    public CreateTaskCommandHandler(ITaskRepository repository)
    {
        _repository = repository;
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
        await _repository.AddAsync(task, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return task;
    }
}
