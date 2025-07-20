using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Domain.Models;

namespace ToDoApp.Application.Tasks.Queries
{
    public class GetTaskByIdQuery: IRequest<TaskItem?>
    {
        public int Id { get; set; }
        public GetTaskByIdQuery(int id)
        {
            Id = id;
        }
    }
}
