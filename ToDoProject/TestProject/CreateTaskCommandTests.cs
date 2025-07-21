using Moq;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToDoApp.Application.Tasks.Handlers;
using ToDoApp.Application.Tasks.Commands;
using ToDoApp.Domain.Enums;

namespace TestProject
{
    public class CreateTaskCommandTests
    {
        private readonly Mock<ITaskRepository> mockRepository;
        private readonly CreateTaskCommandHandler handler;
        public CreateTaskCommandTests()
        {
            mockRepository = new Mock<ITaskRepository>();
            mockRepository.Setup(repository => repository.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            mockRepository.Setup(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            handler = new CreateTaskCommandHandler(mockRepository.Object);
        }
        [Fact]

        public async Task Handle_ShouldCreateTask_WithDefaultStatus()
        {
            CreateTaskCommand command = new CreateTaskCommand()
            {
                Title = "Test Task",
                Description = "This is a test task",
                Deadline = DateTime.UtcNow.AddDays(3),
            };
            TaskItem result = await handler.Handle(command, CancellationToken.None);
            
            Assert.NotNull(result);
            Assert.Equal("Test Task", result.Title);
            Assert.Equal("This is a test task", result.Description);
            Assert.Equal(TaskProgressStatus.ToDo, result.Status);

            mockRepository.Verify(repository => repository.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
            mockRepository.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }
        [Fact]
        public async Task Handle_ShouldUseProvidedStatus_WhenGiven()
        {
            CreateTaskCommand command = new CreateTaskCommand
            {
                Title = "Task with status",
                Status = TaskProgressStatus.InProgress
            };
            TaskItem result = await handler.Handle(command, CancellationToken.None);
            Assert.Equal(TaskProgressStatus.InProgress, result.Status);
            mockRepository.Verify(repository => repository.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
            mockRepository.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldSetTimeCorrectly()
        {
            DateTime before = DateTime.UtcNow;
            CreateTaskCommand command = new CreateTaskCommand
            {
                Title = "Task for time checking",
            };
            TaskItem result = await handler.Handle(command, CancellationToken.None);
            DateTime after = DateTime.UtcNow;
            Assert.True(result.CreatedAt >= before && result.CreatedAt <= after);
            Assert.True(result.UpdatedAt >= before && result.CreatedAt <= after);
            mockRepository.Verify(repository => repository.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
            mockRepository.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
