using TodoJsAspire.ApiService.Db;

namespace TodoJsAspire.ApiService.Todos;

public record PutTodoDto
{
    public required string Title { get; init; }

    public required bool IsComplete { get; init; }

    public required int Position { get; init; }
}

public record CreateTodoDto
{
    public required string Title { get; init; }

    public required bool IsComplete { get; init; }
}