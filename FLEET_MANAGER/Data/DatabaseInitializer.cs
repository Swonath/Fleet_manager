using FLEET_MANAGER.Helpers;
using System.Data.SQLite;

namespace FLEET_MANAGER.Data
{
    /// <summary>
    /// Classe pour initialiser automatiquement la base de données SQLite
    /// Crée les tables et les données par défaut au premier lancement
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Initialise la base de données si elle n'existe pas
        /// </summary>
        public static void InitializeIfNeeded()
        {
            if (!DatabaseConnection.DatabaseExists())
            {
                CreateDatabase();
                CreateTables();
                InsertDefaultData();
            }
            else
            {
                // Exécuter les migrations pour les bases existantes
                ApplyMigrations();
            }
        }

        /// <summary>
        /// Crée le fichier de base de données
        /// </summary>
        private static void CreateDatabase()
        {
            string? dbPath = DatabaseConnection.GetDatabasePath();
            if (dbPath != null)
            {
                SQLiteConnection.CreateFile(dbPath);
                System.Diagnostics.Debug.WriteLine($"Base de données créée : {dbPath}");
            }
        }

        /// <summary>
        /// Crée toutes les tables nécessaires
        /// </summary>
        private static void CreateTables()
        {
            // Table des utilisateurs
            DatabaseConnection.ExecuteCommand(@"
                CREATE TABLE utilisateurs (
                    id_utilisateur INTEGER PRIMARY KEY AUTOINCREMENT,
                    nom_utilisateur TEXT UNIQUE NOT NULL,
                    email TEXT UNIQUE NOT NULL,
                    mot_de_passe TEXT NOT NULL,
                    nom TEXT NOT NULL,
                    prenom TEXT NOT NULL,
                    role TEXT NOT NULL CHECK(role IN ('super_admin', 'admin', 'utilisateur')) DEFAULT 'utilisateur',
                    date_creation TEXT DEFAULT CURRENT_TIMESTAMP,
                    actif INTEGER DEFAULT 1
                )");

            // Table des véhicules
            DatabaseConnection.ExecuteCommand(@"
                CREATE TABLE vehicules (
                    id_vehicule INTEGER PRIMARY KEY AUTOINCREMENT,
                    marque TEXT NOT NULL,
                    modele TEXT NOT NULL,
                    immatriculation TEXT UNIQUE NOT NULL,
                    annee_fabrication INTEGER NOT NULL,
                    type_carburant TEXT NOT NULL CHECK(type_carburant IN ('Essence', 'Diesel', 'Hybride', 'Électrique', 'GPL')),
                    kilometrage_initial INTEGER DEFAULT 0,
                    kilometrage_actuel INTEGER DEFAULT 0,
                    date_acquisition TEXT NOT NULL,
                    etat TEXT NOT NULL CHECK(etat IN ('En service', 'En maintenance', 'Retiré du service', 'Disponible')) DEFAULT 'Disponible',
                    capacite_reservoir REAL,
                    capacite_batterie REAL,
                    date_creation TEXT DEFAULT CURRENT_TIMESTAMP,
                    date_modification TEXT DEFAULT CURRENT_TIMESTAMP
                )");

            // Index pour les véhicules
            DatabaseConnection.ExecuteCommand("CREATE INDEX idx_immatriculation ON vehicules(immatriculation)");
            DatabaseConnection.ExecuteCommand("CREATE INDEX idx_etat ON vehicules(etat)");

            // Table des carburants
            DatabaseConnection.ExecuteCommand(@"
                CREATE TABLE carburants (
                    id_carburant INTEGER PRIMARY KEY AUTOINCREMENT,
                    id_vehicule INTEGER NOT NULL,
                    date_saisie TEXT NOT NULL,
                    heure_saisie TEXT NOT NULL,
                    quantite_litres REAL NOT NULL,
                    cout_total REAL NOT NULL,
                    cout_par_litre REAL NOT NULL,
                    kilometrage INTEGER NOT NULL,
                    notes TEXT,
                    id_utilisateur INTEGER NOT NULL,
                    date_creation TEXT DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (id_vehicule) REFERENCES vehicules(id_vehicule) ON DELETE CASCADE,
                    FOREIGN KEY (id_utilisateur) REFERENCES utilisateurs(id_utilisateur) ON DELETE RESTRICT
                )");

            // Index pour les carburants
            DatabaseConnection.ExecuteCommand("CREATE INDEX idx_carburant_vehicule ON carburants(id_vehicule)");
            DatabaseConnection.ExecuteCommand("CREATE INDEX idx_carburant_date ON carburants(date_saisie)");

            // Table des trajets
            DatabaseConnection.ExecuteCommand(@"
                CREATE TABLE trajets (
                    id_trajet INTEGER PRIMARY KEY AUTOINCREMENT,
                    id_vehicule INTEGER NOT NULL,
                    date_trajet TEXT NOT NULL,
                    heure_depart TEXT NOT NULL,
                    heure_arrivee TEXT NOT NULL,
                    kilometrage_depart INTEGER NOT NULL,
                    kilometrage_arrivee INTEGER NOT NULL,
                    distance_parcourue INTEGER NOT NULL,
                    lieu_depart TEXT NOT NULL,
                    lieu_arrivee TEXT NOT NULL,
                    type_trajet TEXT NOT NULL CHECK(type_trajet IN ('Personnel', 'Professionnel', 'Service')) DEFAULT 'Professionnel',
                    notes TEXT,
                    id_utilisateur INTEGER NOT NULL,
                    date_creation TEXT DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (id_vehicule) REFERENCES vehicules(id_vehicule) ON DELETE CASCADE,
                    FOREIGN KEY (id_utilisateur) REFERENCES utilisateurs(id_utilisateur) ON DELETE RESTRICT
                )");

            // Index pour les trajets
            DatabaseConnection.ExecuteCommand("CREATE INDEX idx_trajet_vehicule ON trajets(id_vehicule)");
            DatabaseConnection.ExecuteCommand("CREATE INDEX idx_trajet_date ON trajets(date_trajet)");

            // Table des maintenances
            DatabaseConnection.ExecuteCommand(@"
                CREATE TABLE maintenances (
                    id_maintenance INTEGER PRIMARY KEY AUTOINCREMENT,
                    id_vehicule INTEGER NOT NULL,
                    date_maintenance TEXT NOT NULL,
                    type_maintenance TEXT NOT NULL CHECK(type_maintenance IN ('Révision', 'Réparation', 'Révision contrôle technique', 'Changement pneus', 'Autre')),
                    description TEXT NOT NULL,
                    cout_maintenance REAL NOT NULL,
                    kilometrage INTEGER NOT NULL,
                    statut TEXT NOT NULL CHECK(statut IN ('Planifiée', 'En cours', 'Terminée', 'Annulée')) DEFAULT 'Planifiée',
                    date_creation TEXT DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (id_vehicule) REFERENCES vehicules(id_vehicule) ON DELETE CASCADE
                )");

            System.Diagnostics.Debug.WriteLine("Tables créées avec succès");
        }

        /// <summary>
        /// Insère les données par défaut (utilisateurs admin)
        /// </summary>
        private static void InsertDefaultData()
        {
            // Hasher les mots de passe avec BCrypt
            string superadminPassword = PasswordHelper.HasherMotDePasse("Superadmin123");
            string adminPassword = PasswordHelper.HasherMotDePasse("Admin123");

            // Insérer le super administrateur
            DatabaseConnection.ExecuteCommand(
                "INSERT INTO utilisateurs (nom_utilisateur, email, mot_de_passe, nom, prenom, role) VALUES (@username, @email, @password, @nom, @prenom, @role)",
                new Dictionary<string, object>
                {
                    { "@username", "superadmin" },
                    { "@email", "superadmin@fleet-manager.com" },
                    { "@password", superadminPassword },
                    { "@nom", "Super" },
                    { "@prenom", "Administrateur" },
                    { "@role", "super_admin" }
                });

            // Insérer l'administrateur
            DatabaseConnection.ExecuteCommand(
                "INSERT INTO utilisateurs (nom_utilisateur, email, mot_de_passe, nom, prenom, role) VALUES (@username, @email, @password, @nom, @prenom, @role)",
                new Dictionary<string, object>
                {
                    { "@username", "admin" },
                    { "@email", "admin@fleet-manager.com" },
                    { "@password", adminPassword },
                    { "@nom", "Administrateur" },
                    { "@prenom", "Système" },
                    { "@role", "admin" }
                });

            System.Diagnostics.Debug.WriteLine("Données par défaut insérées avec succès");
            System.Diagnostics.Debug.WriteLine("Comptes créés :");
            System.Diagnostics.Debug.WriteLine("  - Super Admin: superadmin / Superadmin123");
            System.Diagnostics.Debug.WriteLine("  - Admin: admin / Admin123");
        }

        /// <summary>
        /// Applique les migrations de base de données pour les nouvelles fonctionnalités
        /// </summary>
        private static void ApplyMigrations()
        {
            try
            {
                // Migration : Ajouter capacite_reservoir et capacite_batterie si elles n'existent pas
                if (!ColumnExists("vehicules", "capacite_reservoir"))
                {
                    DatabaseConnection.ExecuteCommand("ALTER TABLE vehicules ADD COLUMN capacite_reservoir REAL");
                    System.Diagnostics.Debug.WriteLine("Colonne 'capacite_reservoir' ajoutée à la table vehicules");
                }

                if (!ColumnExists("vehicules", "capacite_batterie"))
                {
                    DatabaseConnection.ExecuteCommand("ALTER TABLE vehicules ADD COLUMN capacite_batterie REAL");
                    System.Diagnostics.Debug.WriteLine("Colonne 'capacite_batterie' ajoutée à la table vehicules");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de l'application des migrations : {ex.Message}");
            }
        }

        /// <summary>
        /// Vérifie si une colonne existe dans une table
        /// </summary>
        private static bool ColumnExists(string tableName, string columnName)
        {
            try
            {
                string query = $"PRAGMA table_info({tableName})";
                using (var reader = DatabaseConnection.ExecuteQuery(query))
                {
                    while (reader.Read())
                    {
                        string name = reader["name"].ToString() ?? string.Empty;
                        if (name.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
