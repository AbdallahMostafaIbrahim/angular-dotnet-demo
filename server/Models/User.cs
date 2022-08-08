using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoApi.Models
{
  public class User
  {
    [Key]
    public string? id { get; set; }
    public string? username { get; set; }
    public string? email { get; set; }

    [JsonIgnore]
    public string? password { get; set; }
  }
  public class UserDBContext : DbContext
  {
    public UserDBContext(DbContextOptions<UserDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>().ToTable("User");
    }

    public DbSet<User> Users { get; set; } = null!;
  }
}