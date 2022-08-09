using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
  public class TodoItem
  {
    [Key]
    public int? Id { get; set; }
    public string? name { get; set; }
    public bool isComplete { get; set; }

    public int? userId { get; set; }
    public User? User { get; set; }
  } 
  public class TodoDBContext : DbContext
  {
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public TodoDBContext(DbContextOptions<TodoDBContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
          modelBuilder.Entity<TodoItem>()
            .HasOne<User>(t => t.User)
            .WithMany(u => u.todos)
            .IsRequired();
    }

  }
}