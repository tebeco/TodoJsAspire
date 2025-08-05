namespace TodoJsAspire.ApiService.Db;

public class Todo
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public bool IsComplete { get; set; } = false;

    // The position of the todo in the list, used for ordering.
    // When updating this, make sure to not duplicate values.
    // To move an item up/down, swap the values of the position
    public required int Position { get; set; }
}