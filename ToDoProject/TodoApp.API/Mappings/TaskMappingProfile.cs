using AutoMapper;
using ToDoApp.API.Dto;
using ToDoApp.Application.Tasks.Commands;
using ToDoApp.Domain.Models;
namespace ToDoApp.API.Mappings
{
    public class TaskMappingProfile: Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<TaskItem, TaskItemOutputDto>();
            CreateMap<UpdateTaskCommandInputDto, UpdateTaskCommand>();
        }
    }
}
