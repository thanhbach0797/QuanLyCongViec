namespace TaskManagerApp.Helpers
{
    public static class AuthManager
    {
        public static string Token { get; private set; }
        public static string Role { get; set; }

        public static void SetToken(string token)
        {
            Token = token;
        }

        public static bool IsAdmin => Role == "Admin";
        public static bool IsManager => Role == "Manager";
        public static bool IsEmployee => Role == "Employee";
    }
}