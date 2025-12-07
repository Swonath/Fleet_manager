using System.Windows;
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

            // Initialiser la connexion à la base de données SQLite
            try
            {
                // Initialiser la connexion (crée le dossier Data/ et le fichier fleet_manager.db)
                DatabaseConnection.Initialize();

                // Initialiser la base de données si nécessaire (première utilisation)
                DatabaseInitializer.InitializeIfNeeded();

                // Tester la connexion
                if (!DatabaseConnection.TestConnection())
                {
                    MessageBox.Show(
                        "Impossible de se connecter à la base de données.\n\n" +
                        "L'application va maintenant se fermer. Si le problème persiste, " +
                        "supprimez le dossier 'Data' et relancez l'application.",
                        "Erreur de connexion",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    this.Shutdown();
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Base de données initialisée : {DatabaseConnection.GetDatabasePath()}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur lors de l'initialisation de la base de données :\n\n{ex.Message}\n\n" +
                    "L'application va maintenant se fermer.",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                this.Shutdown();
            }
        }
    }
}
