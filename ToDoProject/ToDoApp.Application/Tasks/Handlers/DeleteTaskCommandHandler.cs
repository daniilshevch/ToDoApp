using MediatR;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Tasks.Commands;
using ToDoApp.Domain.Models;

namespace ToDoApp.Application.Tasks.Handlers
{
    public class DeleteTaskCommandHandler: IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _repository;
        public DeleteTaskCommandHandler(ITaskRepository repository) 
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            TaskItem? task = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (task is null)
            {
                return false;
            }
            _repository.Delete(task);
            await _repository.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
