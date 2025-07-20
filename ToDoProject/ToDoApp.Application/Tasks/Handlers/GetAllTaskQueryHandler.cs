using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Tasks.Queries;
using ToDoApp.Domain.Models;

namespace ToDoApp.Application.Tasks.Handlers
{
    public class GetAllTaskQueryHandler: IRequestHandler<GetAllTasksQuery, List<TaskItem>>
    {
        private readonly IAppDbContext _context;
        public GetAllTaskQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TaskItem>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            return await _context.Tasks.ToListAsync();
        }
    }
}
