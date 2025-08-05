using Microsoft.EntityFrameworkCore;

namespace TodoJsAspire.ApiService.Db;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }
}