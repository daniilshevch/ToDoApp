using MediatR;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.Models;

namespace ToDoApp.Application.Tasks.Commands
{
    public class UpdateTaskCommand: IRequest<TaskItem?>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; } 
        public DateTime? Deadline { get; set; }
        public TaskProgressStatus? Status { get; set; }
    }
    public class UpdateTaskCommandInputDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public TaskProgressStatus? Status { get; set; }

    }
}
