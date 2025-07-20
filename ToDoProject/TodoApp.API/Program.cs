using ToDoApp.Inftrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces;
using MediatR;
using System.Text.Json.Serialization;
using ToDoApp.Application.Tasks.Commands;
using FluentValidation;
using ToDoApp.API.Validators;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.Middlewares;
using ToDoApp.API.Mappings;
using Microsoft.AspNetCore.Cors.Infrastructure;
class Server
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IServiceCollection services = builder.Services;
        IConfiguration configuration = builder.Configuration;
        services.AddControllers().AddJsonOptions((JsonOptions options) =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(typeof(TaskMappingProfile).Assembly);
        services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));  
        services.AddDbContext<AppDbContext>((DbContextOptionsBuilder options) => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IAppDbContext>((IServiceProvider provider) => provider.GetRequiredService<AppDbContext>());
        services.AddMediatR((MediatRServiceConfiguration mediatorConfiguration) => mediatorConfiguration.RegisterServicesFromAssembly(typeof(CreateTaskCommand).Assembly));
        services.AddCors((CorsOptions options) =>
        {
            options.AddPolicy("AllowAll", (CorsPolicyBuilder policy) =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });
        WebApplication app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
        }
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}


