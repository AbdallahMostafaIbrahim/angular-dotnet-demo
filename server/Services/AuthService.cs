using TodoApi.Models;

namespace TodoApi.Services
{
    public interface IAuthService
    {
        User? Authenticate(string email, string password);
        bool Register(User user);
        User? GetUser(string id);

    }
    public class AuthService : IAuthService
    {
        private readonly UserDBContext _context;

        public AuthService(UserDBContext context)
        {
            _context = context;
        }

        public User? Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.email == email && u.password == password);
            return user;
        }

        public bool Register(User user)
        {
            var userExists = _context.Users.Where(u => u.email == user.email).ToList();
            if (userExists.Count > 0)
            {
                return false;
            }
            user.id = Guid.NewGuid().ToString();
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public User? GetUser(string id)
        {
            var user = _context.Users.Find(id);
            return user;
        }
    }
}