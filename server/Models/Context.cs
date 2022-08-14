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

      modelBuilder.Entity<Student>()
        .HasOne<Class>(t => t.Class)
        .WithMany(u => u.Students)
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


      modelBuilder.Entity<User>().HasData(
        new User
        {
          Id = 1,
          username = "admin",
          password = "$argon2id$v=19$m=1024,t=1,p=1$c29tZXNhbHQ$ZmFtaGxvd2xlZGdl",
          email = "admin@gmail.com"
        },
        new User
        {
          Id = 2,
          username = "user",
          password = "$argon2id$v=19$m=1024,t=1,p=1$c29tZXNhbHQ$ZmFtaGxvd2xlZGdl",
          email = "user@gmail.com"
        }
        );
      modelBuilder.Entity<Category>().HasData(
        new Category
        {
          imageId = 1,
          name = "Electronics",
          Id = 1,
          description = "nice",
        }
        );
      modelBuilder.Entity<Product>().HasData(
          new Product { Id = 1, name = "Cool", imageId = 1, price = 100, categoryId = 1, description = "niasdfce" },
          new Product { Id = 2, name = "Very", imageId = 2, price = 123, categoryId = 1, description = "dsd" },
          new Product { Id = 3, name = "Dope", imageId = 1, price = 10120, categoryId = 1, description = "nisadfasdfce" },
          new Product { Id = 4, name = "Prod", imageId = 3, price = 10120, categoryId = 1, description = "asdf" }
      );
      modelBuilder.Entity<Image>().HasData(
        new Image { Id = 1, name = "Image 1", imageUrl = "yeet.png", },
        new Image { Id = 2, name = "Image 2", imageUrl = "ok.png", },
        new Image { Id = 3, name = "Image 3", imageUrl = "adsf.png", }
      );
      modelBuilder.Entity<Cart>().HasData(
        new Cart { Id = 1, userId = 1, totalPrice = 200 },
        new Cart { Id = 2, userId = 2, totalPrice = 200 }
      );
      modelBuilder.Entity<CartItem>().HasData(
        new CartItem { Id = 1, productId = 1, cartId = 1 },
        new CartItem { Id = 2, productId = 2, cartId = 1 },
        new CartItem { Id = 3, productId = 2, cartId = 2 }
      );

      modelBuilder.Entity<Student>().HasData(
  new Student
  {
    Id = 1,
    username = "Abdallah",
    classId = 1,
    email = "abdo@gmail.com"
  },
  new Student
  {
    Id = 2,
    username = "Salma",
    classId = 1,
    email = "salma@gmail.com"
  },
    new Student
    {
      Id = 3,
      username = "Hamza",
      classId = 2,
      email = "hamza@gmail.com"
    }
  );

      modelBuilder.Entity<Class>().HasData(
new Class
{
  Id = 1,
  name = "Big"
},
new Class
{
  Id = 2,
  name = "Small"
}
);

    }

  }
}