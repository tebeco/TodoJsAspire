using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoJsAspire.ApiService;
using TodoJsAspire.ApiService.Db;
using TodoJsAspire.ApiService.Todos;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/todo").WithTags(nameof(Todo));

        group.MapGet("/", GetAllTodos)
            .WithName(nameof(GetAllTodos))
            .WithOpenApi();

        group.MapGet("/{id}", GetTodoById)
            .WithName(nameof(GetTodoById))
            .WithOpenApi();

        group.MapPut("/{id}", UpdateTodo)
            .WithName(nameof(UpdateTodo))
            .WithOpenApi();

        group.MapPost("/", CreateTodo)
            .WithName(nameof(CreateTodo))
            .WithOpenApi();

        group.MapDelete("/{id}", DeleteTodo)
            .WithName(nameof(DeleteTodo))
            .WithOpenApi();
    }

    public static async Task<Ok<IEnumerable<TodoDto>>> GetAllTodos(TodoDbContext db)
    {
        var todos = await db.Todos.ToListAsync();
        return TypedResults.Ok(todos.ToDtos());
    }

    private static async Task<Results<Ok<TodoDto>, NotFound>> GetTodoById(int id, TodoDbContext db)
    {
        var todo = await db.Todos
            .AsNoTracking()
            .FirstOrDefaultAsync(todo => todo.Id == id);

        return TypedResults.Extensions.ToOkOrNotFound(todo?.ToDto());
    }

    private static async Task<Results<Ok, NotFound>> UpdateTodo(int id, PutTodoDto putTodoDto, TodoDbContext db)
    {
        var affected = await db.Todos
            .Where(todo => todo.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.Title, putTodoDto.Title)
                .SetProperty(m => m.IsComplete, putTodoDto.IsComplete)
                .SetProperty(m => m.Position, putTodoDto.Position));

        return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static async Task<Created<TodoDto>> CreateTodo(CreateTodoDto createTodoDto, TodoDbContext db)
    {
        var todo = new Todo
        {
            IsComplete = createTodoDto.IsComplete,
            Position = createTodoDto.Position,
            Title = createTodoDto.Title,
        };
        
        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        
        return TypedResults.Created($"/api/todo/{todo.Id}", todo.ToDto());
    }
    
    private static async Task<Results<Ok, NotFound>> DeleteTodo (int id, TodoDbContext db)
    {
        var affected = await db.Todos
            .Where(model => model.Id == id)
            .ExecuteDeleteAsync();

        return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
    }
}