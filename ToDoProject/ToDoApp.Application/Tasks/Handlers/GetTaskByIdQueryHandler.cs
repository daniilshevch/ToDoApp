using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Tasks.Queries;
using ToDoApp.Domain.Models;

namespace ToDoApp.Application.Tasks.Handlers
{
    public class GetTaskByIdQueryHandler: IRequestHandler<GetTaskByIdQuery, TaskItem?>
    {
        private readonly IAppDbContext _context;
        public GetTaskByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<TaskItem?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Tasks.FirstOrDefaultAsync(task => task.Id == request.Id, cancellationToken);
        }

    }
}
