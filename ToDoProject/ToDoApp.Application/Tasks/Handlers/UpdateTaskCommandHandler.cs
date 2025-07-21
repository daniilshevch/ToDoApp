using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Tasks.Commands;
using ToDoApp.Domain.Models;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.Tasks.Handlers
{
    public class UpdateTaskCommandHandler: IRequestHandler<UpdateTaskCommand, TaskItem?>
    {
        public static void UpdateTask(TaskItem task, UpdateTaskCommand request)
        {
            if(request.Title is not null)
            {
                task.Title = request.Title;
            }
            if(request.Description is not null)
            {
                task.Description = request.Description;
            }
            if(request.Deadline is not null) 
            {
                task.Deadline = request.Deadline;
            }
            if(request.Status is TaskProgressStatus updatedStatus)
            {
                task.Status = updatedStatus;
            }
            task.UpdatedAt = DateTime.UtcNow;
        }
        private readonly ITaskRepository _repository;
        public UpdateTaskCommandHandler(ITaskRepository repository)
        {
            _repository = repository;
        }
        public async Task<TaskItem?> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            TaskItem? task = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if(task is null)
            {
                return null;
            }
            UpdateTask(task, request);
            await _repository.SaveChangesAsync(cancellationToken);
            return task;
        }
    }
}
