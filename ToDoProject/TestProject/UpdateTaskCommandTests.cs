using Moq;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToDoApp.Application.Tasks.Handlers;
using ToDoApp.Application.Tasks.Commands;
using ToDoApp.Domain.Enums;
using Microsoft.Identity.Client;

namespace TestProject
{
    public class UpdateTaskCommandTests
    {
        private readonly Mock<ITaskRepository> mockRepository;
        private readonly UpdateTaskCommandHandler handler;
        public UpdateTaskCommandTests()
        {
            mockRepository = new Mock<ITaskRepository>();
            mockRepository.Setup(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            handler = new UpdateTaskCommandHandler(mockRepository.Object);
        }
        [Fact]
        public async Task Handle_ShouldUpdateTask_WhenTaskExists()
        {
            TaskItem existingTask = new TaskItem
            {
                Id = 1,
                Title = "Old Title",
                Description = "Old Description",
                Deadline = DateTime.UtcNow.AddDays(1),
                Status = TaskProgressStatus.ToDo,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            };

            mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingTask);

            UpdateTaskCommand updateCommand = new UpdateTaskCommand
            {
                Id = 1,
                Title = "New Title",
                Description = "New Description",
                Deadline = DateTime.UtcNow.AddDays(5),
                Status = TaskProgressStatus.Done
            };

            TaskItem? result = await handler.Handle(updateCommand, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
            Assert.Equal("New Description", result.Description);
            Assert.Equal(TaskProgressStatus.Done, result.Status);
            Assert.Equal(updateCommand.Deadline, result.Deadline);
            Assert.True(result.UpdatedAt > result.CreatedAt);

            mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
            mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldReturnNull_WhenTaskNotFound()
        {
            mockRepository.Setup(r => r.GetByIdAsync(12, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);
            
            UpdateTaskCommand updateCommand = new UpdateTaskCommand
            {
                Id = 2,
            };

            TaskItem? result = await handler.Handle(updateCommand, CancellationToken.None);
            Assert.Null(result);


            mockRepository.Verify(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>()), Times.Once);
            mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
