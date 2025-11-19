namespace FLEET_MANAGER.Config
{
    /// <summary>
    /// Configuration de la base de données
    /// </summary>
    public static class DatabaseConfig
    {
        public static string Server { get; set; } = "localhost";
        public static string Database { get; set; } = "fleet_manager";
        public static string UserId { get; set; } = "root";
        public static string Password { get; set; } = "";
        public static int Port { get; set; } = 3306;
    }
}
