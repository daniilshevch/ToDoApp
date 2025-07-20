using ToDoApp.Domain.Enums;

namespace ToDoApp.API.Dto
{
    public class TaskItemOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public TaskProgressStatus Status { get; set; }
    }
}
