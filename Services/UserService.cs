using Microsoft.EntityFrameworkCore;
using tecnologyApp.Data;

namespace tecnologyApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User? CurrentUser { get; private set; }

        public async Task<AuthResult> LoginAsync(LoginModel model)
        {
            var user = await _context.Users
                .Include(u => u.Cart)
                .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

            if (user is null)
            {
                return new AuthResult { Success = false, Message = "Incorrect credentials." };
            }

            CurrentUser = user;
            return new AuthResult { Success = true, Message = "Login successful." };
        }

        public async Task<AuthResult> RegisterAsync(RegisterModel model)
        {
            var exists = await _context.Users.AnyAsync(u => u.Username == model.Username);
            if (exists)
            {
                return new AuthResult { Success = false, Message = "User already exists." };
            }

            var newUser = new User
            {
                Username = model.Username,
                Password = model.Password,
                Role = model.Role,
                Cart = new List<Product>()
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new AuthResult { Success = true, Message = "Registration successful." };
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            var exists = await _context.Users.AnyAsync(u => u.Username == user.Username);
            if (exists) return false;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public void Logout()
        {
            CurrentUser = null;

            // _context.SaveChanges(); // Optionally persist changes if needed
        }
    }
}
