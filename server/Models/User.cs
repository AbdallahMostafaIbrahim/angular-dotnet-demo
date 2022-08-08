using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class User
    {
        public string? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
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
            modelBuilder.Entity<User>().HasData(new User
            {
                id = "1",
                username = "admin",
                email = "abdallahmostafaibrahim@gmail.com",
                password = "test"
            });

        }

        public DbSet<User> Users { get; set; } = null!;
    }
}