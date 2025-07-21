using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Domain.Models;

namespace ToDoApp.Application.Interfaces
{
    public interface ITaskRepository
    {
        public Task AddAsync(TaskItem task, CancellationToken cancellationToken);
        Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<List<TaskItem>> GetAllAsync();
        public void Update(TaskItem task);
        public void Delete(TaskItem task);
        Task<int> SaveChangesAsync(CancellationToken token);
    }
}
