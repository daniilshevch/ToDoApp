using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Tasks.Commands;
using MediatR;
using ToDoApp.Domain.Models;
using ToDoApp.Application.Tasks.Queries;
using AutoMapper;
using ToDoApp.API.Dto;

namespace TodoApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public TasksController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateTaskCommand command)
        {
            TaskItem task = await _mediator.Send(command);
            TaskItemOutputDto taskDto = _mapper.Map<TaskItemOutputDto>(task);
            return Ok(taskDto);
        }
        [HttpGet]
        public async Task<ActionResult<List<TaskItem>>> GetAll()
        {
            List<TaskItem> tasks = await _mediator.Send(new GetAllTasksQuery());
            List<TaskItemOutputDto> tasksDto = _mapper.Map<List<TaskItemOutputDto>>(tasks);
            return Ok(tasksDto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem?>> GetById(int id)
        {
            TaskItem? task = await _mediator.Send(new GetTaskByIdQuery(id));
            if(task is null)
            {
                return NotFound($"Cant find task with id: {id}");
            }
            TaskItemOutputDto taskDto = _mapper.Map<TaskItemOutputDto>(task);
            return Ok(taskDto);
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<TaskItem?>> UpdateById([FromRoute] int id, [FromBody] UpdateTaskCommandInputDto input)
        {
            UpdateTaskCommand updateTaskCommand = _mapper.Map<UpdateTaskCommand>(input);
            updateTaskCommand.Id = id;
            TaskItem? updatedTask = await _mediator.Send(updateTaskCommand);
            if(updatedTask is null)
            {
                return NotFound($"Cant find task with ID: {id}");
            }
            TaskItemOutputDto updatedTaskDto = _mapper.Map<TaskItemOutputDto>(updatedTask);
            return Ok(updatedTaskDto); 
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            bool successfulDelete = await _mediator.Send(new DeleteTaskCommand(id));
            if(!successfulDelete)
            {
                return NotFound($"Can't find task with id: {id}");
            }
            return NoContent();
        }
    }
}
