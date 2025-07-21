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
        private readonly Mock<DbSet<TaskItem>> mockDbSet;
        private readonly Mock<IAppDbContext> mockDbContext;
        private readonly CreateTaskCommandHandler handler;
        public CreateTaskCommandTests()
        {
            mockDbSet = new Mock<DbSet<TaskItem>>();
            mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(context => context.Tasks).Returns(mockDbSet.Object);
            mockDbContext.Setup(context => context.Tasks.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>())).Returns(null!);
            mockDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            handler = new CreateTaskCommandHandler(mockDbContext.Object);
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

            mockDbContext.Verify(c => c.Tasks.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
            mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

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
            mockDbContext.Verify(c => c.Tasks.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
            mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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
            mockDbContext.Verify(c => c.Tasks.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
            mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
