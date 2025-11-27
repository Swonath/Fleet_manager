using System.Configuration;

namespace FLEET_MANAGER.Config
{
    /// <summary>
    /// Configuration de la base de données
    /// Lit les valeurs depuis App.config pour plus de sécurité
    /// </summary>
    public static class DatabaseConfig
    {
        public static string Server => ConfigurationManager.AppSettings["DB_Server"] ?? "localhost";
        public static string Database => ConfigurationManager.AppSettings["DB_Database"] ?? "fleet_manager";
        public static string UserId => ConfigurationManager.AppSettings["DB_UserId"] ?? "root";
        public static string Password => ConfigurationManager.AppSettings["DB_Password"] ?? "";
        public static int Port => int.TryParse(ConfigurationManager.AppSettings["DB_Port"], out int port) ? port : 3306;
    }
}
