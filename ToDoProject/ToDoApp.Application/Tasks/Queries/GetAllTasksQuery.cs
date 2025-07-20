using MediatR;
using ToDoApp.Domain.Models;
namespace ToDoApp.Application.Tasks.Queries
{
    public class GetAllTasksQuery: IRequest<List<TaskItem>>
    {

    }
}
