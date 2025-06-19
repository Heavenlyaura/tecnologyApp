using tecnologyApp.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace tecnologyApp.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        Task<bool> RegisterUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();
        Task<AuthResult> LoginAsync(LoginModel model);
        Task<AuthResult> RegisterAsync(RegisterModel model);
        User? CurrentUser { get; }
        void Logout(); 
    }
}