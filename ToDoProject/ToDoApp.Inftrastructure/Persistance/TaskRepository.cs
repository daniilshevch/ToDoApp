using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Models;

namespace ToDoApp.Inftrastructure.Persistance
{
    public class TaskRepository: ITaskRepository
    {
        private readonly IAppDbContext _context;
        public TaskRepository(IAppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TaskItem task, CancellationToken cancellationToken)
        {
            await _context.Tasks.AddAsync(task);
        }
        public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Tasks.FirstOrDefaultAsync(task => task.Id == id);
        }
        public async Task<List<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }
        public void Update(TaskItem task)
        {
            _context.Tasks.Update(task);
        }
        public void Delete(TaskItem task)
        {
            _context.Tasks.Remove(task);
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
