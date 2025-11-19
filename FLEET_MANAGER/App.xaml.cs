using System.Configuration;
using System.Data;
using System.Windows;
using FLEET_MANAGER.Config;
using FLEET_MANAGER.Data;

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
                }
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
    }
}
