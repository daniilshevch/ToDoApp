using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Tasks.Commands;
using ToDoApp.Domain.Models;

namespace ToDoApp.Application.Tasks.Handlers
{
    public class DeleteTaskCommandHandler: IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly IAppDbContext _context;
        public DeleteTaskCommandHandler(IAppDbContext context) 
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            TaskItem? task = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == request.Id);
            if (task is null)
            {
                return false;
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
