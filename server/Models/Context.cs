using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
  public class TodoDBContext : DbContext
  {
    public DbSet<Todo> TodoItems { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    public TodoDBContext(DbContextOptions<TodoDBContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

      modelBuilder.Entity<CartItem>()
        .HasOne<Cart>(t => t.Cart)
        .WithMany(u => u.CartItems)
        .IsRequired();


      modelBuilder.Entity<CartItem>()
        .HasOne<Product>(t => t.Product);

      modelBuilder.Entity<Cart>()
        .HasOne<User>(t => t.User);

      modelBuilder.Entity<Product>()
        .HasOne<Category>(t => t.Category);

      modelBuilder.Entity<Product>()
        .HasOne<Image>(t => t.Image);

      modelBuilder.Entity<Category>()
        .HasOne<Image>(t => t.Image);
    }

  }
}