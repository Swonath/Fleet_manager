using System.Configuration;
using System.Data;
using System.Windows;
using FLEET_MANAGER.Config;
using FLEET_MANAGER.Data;
using FLEET_MANAGER.Services;

namespace FLEET_MANAGER
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialiser la connexion à la base de données
            try
            {
                DatabaseConnection.Initialize(
                    DatabaseConfig.Server,
                    DatabaseConfig.Database,
                    DatabaseConfig.UserId,
                    DatabaseConfig.Password,
                    DatabaseConfig.Port
                );

                // Tester la connexion
                if (!DatabaseConnection.TestConnection())
                {
                    MessageBox.Show(
                        "Impossible de se connecter à la base de données.\n\n" +
                        "Assurez-vous que :\n" +
                        "1. MySQL est en cours d'exécution\n" +
                        "2. La base de données 'fleet_manager' existe\n" +
                        "3. Les paramètres de connexion sont corrects",
                        "Erreur de connexion",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    this.Shutdown();
                    return;
                }

                // Migrer les mots de passe non hashés (exécuté une seule fois)
                MigrerMotsDePasse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur lors de l'initialisation : {ex.Message}",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                this.Shutdown();
            }
        }

        /// <summary>
        /// Migre automatiquement les mots de passe en clair vers BCrypt
        /// Cette méthode s'exécute au démarrage et convertit les anciens mots de passe
        /// </summary>
        private void MigrerMotsDePasse()
        {
            try
            {
                // Vérifier si une migration est nécessaire
                if (PasswordMigrationService.MigrationNecessaire())
                {
                    int nombreMigres = PasswordMigrationService.MigrerMotsDePasse();

                    if (nombreMigres > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Migration BCrypt : {nombreMigres} mot(s) de passe converti(s)");
                    }
                }
            }
            catch (Exception ex)
            {
                // Ne pas bloquer l'application en cas d'erreur de migration
                System.Diagnostics.Debug.WriteLine($"Avertissement migration : {ex.Message}");
            }
        }
    }
}
