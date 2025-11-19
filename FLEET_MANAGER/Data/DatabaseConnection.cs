using MySql.Data.MySqlClient;
using System.Configuration;

namespace FLEET_MANAGER.Data
{
    /// <summary>
    /// Classe pour gérer la connexion à la base de données MySQL
    /// </summary>
    public class DatabaseConnection
    {
        private static string? _connectionString;

        /// <summary>
        /// Initialise la chaîne de connexion
        /// </summary>
        public static void Initialize(string server, string database, string uid, string password, int port = 3306)
        {
            _connectionString = $"Server={server};Port={port};Database={database};Uid={uid};Pwd={password};";
        }

        /// <summary>
        /// Retourne une nouvelle connexion à la base de données
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée. Appelez Initialize() d'abord.");
            }

            return new MySqlConnection(_connectionString);
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
                using (var command = new MySqlCommand(query, connection))
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
                using (var command = new MySqlCommand(query, connection))
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
        public static MySqlDataReader ExecuteQuery(string query)
        {
            var connection = GetConnection();
            connection.Open();
            var command = new MySqlCommand(query, connection);
            return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Récupère un tableau de résultats avec paramètres
        /// </summary>
        public static MySqlDataReader ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            var connection = GetConnection();
            connection.Open();
            var command = new MySqlCommand(query, connection);
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
                using (var command = new MySqlCommand(query, connection))
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
                using (var command = new MySqlCommand(query, connection))
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
