using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Tasks.Queries;
using ToDoApp.Domain.Models;

namespace ToDoApp.Application.Tasks.Handlers
{
    public class GetAllTaskQueryHandler: IRequestHandler<GetAllTasksQuery, List<TaskItem>>
    {
        private readonly ITaskRepository _repository;
        public GetAllTaskQueryHandler(ITaskRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<TaskItem>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
