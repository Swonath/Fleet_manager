using FLEET_MANAGER.Data;
using FLEET_MANAGER.Helpers;

namespace FLEET_MANAGER.Services
{
    /// <summary>
    /// Service de migration pour convertir les mots de passe en clair vers BCrypt
    /// À exécuter une seule fois lors de la mise à jour de l'application
    /// </summary>
    public static class PasswordMigrationService
    {
        /// <summary>
        /// Migre tous les mots de passe non hashés vers BCrypt
        /// Un mot de passe est considéré comme "non hashé" s'il ne commence pas par "$2"
        /// </summary>
        public static int MigrerMotsDePasse()
        {
            int compteur = 0;

            try
            {
                // Récupérer tous les utilisateurs
                string selectQuery = "SELECT id_utilisateur, mot_de_passe FROM utilisateurs";
                var utilisateurs = new List<(int id, string motDePasse)>();

                using (var reader = DatabaseConnection.ExecuteQuery(selectQuery))
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["id_utilisateur"];
                        string mdp = reader["mot_de_passe"]?.ToString() ?? string.Empty;
                        utilisateurs.Add((id, mdp));
                    }
                }

                // Pour chaque utilisateur, vérifier si le mot de passe est déjà hashé
                foreach (var (id, motDePasse) in utilisateurs)
                {
                    // Si le mot de passe ne commence pas par "$2", c'est qu'il n'est pas hashé
                    if (!string.IsNullOrEmpty(motDePasse) && !motDePasse.StartsWith("$2"))
                    {
                        // Hasher le mot de passe
                        string hash = PasswordHelper.HasherMotDePasse(motDePasse);

                        // Mettre à jour en base
                        string updateQuery = "UPDATE utilisateurs SET mot_de_passe = @hash WHERE id_utilisateur = @id";
                        var parameters = new Dictionary<string, object>
                        {
                            { "@hash", hash },
                            { "@id", id }
                        };

                        DatabaseConnection.ExecuteCommand(updateQuery, parameters);
                        compteur++;

                        System.Diagnostics.Debug.WriteLine($"Mot de passe migré pour l'utilisateur ID {id}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Migration terminée : {compteur} mot(s) de passe migré(s)");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la migration : {ex.Message}");
                throw;
            }

            return compteur;
        }

        /// <summary>
        /// Vérifie si la migration est nécessaire (s'il existe des mots de passe non hashés)
        /// </summary>
        public static bool MigrationNecessaire()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM utilisateurs WHERE mot_de_passe NOT LIKE '$2%' AND mot_de_passe IS NOT NULL AND mot_de_passe != ''";
                using (var reader = DatabaseConnection.ExecuteQuery(query))
                {
                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader[0]) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la vérification de migration : {ex.Message}");
            }

            return false;
        }
    }
}
