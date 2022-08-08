using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
  public class TodoItem
  {
    public string? id { get; set; }
    public string? name { get; set; }
    public bool isComplete { get; set; }
  }
  public class TodoDBContext : DbContext
  {
    public TodoDBContext(DbContextOptions<TodoDBContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;

  }
}