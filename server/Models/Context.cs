using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
  public class TodoDBContext : DbContext
  {
    public DbSet<Todo> TodoItems { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public TodoDBContext(DbContextOptions<TodoDBContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Todo>()
        .HasOne<User>(t => t.User)
        .WithMany(u => u.todos)
        .IsRequired();
    }

  }
}