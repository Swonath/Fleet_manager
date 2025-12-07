using System.Data.SQLite;
using System.IO;

namespace FLEET_MANAGER.Data
{
    /// <summary>
    /// Classe pour gérer la connexion à la base de données SQLite
    /// Version migrée depuis MySQL pour une application standalone
    /// </summary>
    public class DatabaseConnection
    {
        private static string? _connectionString;
        private static string? _databasePath;

        /// <summary>
        /// Initialise la chaîne de connexion SQLite
        /// </summary>
        /// <param name="databasePath">Chemin vers le fichier de base de données SQLite (optionnel, utilise Data/fleet_manager.db par défaut)</param>
        public static void Initialize(string? databasePath = null)
        {
            // Si aucun chemin n'est spécifié, utiliser le dossier Data dans le répertoire de l'application
            if (string.IsNullOrEmpty(databasePath))
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string dataDirectory = Path.Combine(appDirectory, "Data");

                // Créer le dossier Data s'il n'existe pas
                if (!Directory.Exists(dataDirectory))
                {
                    Directory.CreateDirectory(dataDirectory);
                }

                _databasePath = Path.Combine(dataDirectory, "fleet_manager.db");
            }
            else
            {
                _databasePath = databasePath;
            }

            _connectionString = $"Data Source={_databasePath};Version=3;";
        }

        /// <summary>
        /// Retourne le chemin de la base de données
        /// </summary>
        public static string? GetDatabasePath()
        {
            return _databasePath;
        }

        /// <summary>
        /// Vérifie si la base de données existe
        /// </summary>
        public static bool DatabaseExists()
        {
            return !string.IsNullOrEmpty(_databasePath) && File.Exists(_databasePath);
        }

        /// <summary>
        /// Retourne une nouvelle connexion à la base de données
        /// </summary>
        public static SQLiteConnection GetConnection()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée. Appelez Initialize() d'abord.");
            }

            return new SQLiteConnection(_connectionString);
        }

        /// <summary>
        /// Teste la connexion à la base de données
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur de connexion : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Exécute une commande SQL sans paramètres
        /// </summary>
        public static void ExecuteCommand(string query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Exécute une commande SQL avec paramètres
        /// </summary>
        public static void ExecuteCommand(string query, Dictionary<string, object> parameters)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Récupère un tableau de résultats
        /// </summary>
        public static SQLiteDataReader ExecuteQuery(string query)
        {
            var connection = GetConnection();
            connection.Open();
            var command = new SQLiteCommand(query, connection);
            return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Récupère un tableau de résultats avec paramètres
        /// </summary>
        public static SQLiteDataReader ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            var connection = GetConnection();
            connection.Open();
            var command = new SQLiteCommand(query, connection);
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
            return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Récupère un scalaire (nombre, compte, etc.)
        /// </summary>
        public static object? ExecuteScalar(string query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    return command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Récupère un scalaire avec paramètres
        /// </summary>
        public static object? ExecuteScalar(string query, Dictionary<string, object> parameters)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                    return command.ExecuteScalar();
                }
            }
        }
    }
}
