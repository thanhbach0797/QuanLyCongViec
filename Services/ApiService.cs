using System.Threading.Tasks;

namespace TaskManagementApp.Services
{
    public class User
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }

    public class ApiService
    {
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            await Task.Delay(500); // Giả lập gọi API

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            // Giả lập: nếu username là "admin", role là "Admin"; "manager" là "Manager"; còn lại là "Employee"
            string role = username.ToLower() switch
            {
                "admin" => "Admin",
                "manager" => "Manager",
                _ => "Employee"
            };
            return new User { Username = username, Role = role };
        }
    }
}