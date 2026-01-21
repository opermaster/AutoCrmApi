namespace AutoCrmApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; }

        public ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();
        
    }
    public class UserDto
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; }
        public User ToUser() {
            return new User() {
                Login = Login,
                PasswordHash = HashPassword(Password),
                Role = Role,
            };
        }
        private static string HashPassword(string password) {

            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string storedHash) {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }

}
