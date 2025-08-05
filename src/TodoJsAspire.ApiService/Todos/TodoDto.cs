using TodoJsAspire.ApiService.Db;

namespace TodoJsAspire.ApiService.Todos;

public class TodoDto
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public required bool IsComplete { get; init; }

    public required int Position { get; init; }
}

public static class TodoDtoExtensions
{
    public static TodoDto ToDto(this Todo todo)
        => new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            IsComplete = todo.IsComplete,
            Position = todo.Position
        };

    public static IEnumerable<TodoDto> ToDtos(this IEnumerable<Todo> todos)
        => [..todos.Select(todo => todo.ToDto())];
}