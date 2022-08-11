using TodoApi.Models;
using Isopoh.Cryptography.Argon2;


namespace TodoApi.Services
{
    public interface IAuthService
    {
        User? Authenticate(string email, string password);
        User? Register(User user);
        User? GetUser(int id);

    }
    public class AuthService : IAuthService
    {
        private readonly TodoDBContext _context;

        public AuthService(TodoDBContext context)
        {
            _context = context;
        }

        public User? Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.email == email);
            if (user != null && Argon2.Verify(user.password, password))
            {
                return user;
            }
            return null;
        }

        public User? Register(User user)
        {
            var userExists = _context.Users.Where(u => u.email == user.email).ToList();
            if (userExists.Count > 0)
            {
                return null;
            }
            user.password = Argon2.Hash(user.password);
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User? GetUser(int id)
        {
            var user = _context.Users.Find(id);
            return user;
        }
    }
}