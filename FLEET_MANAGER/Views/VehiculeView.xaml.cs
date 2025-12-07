using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FLEET_MANAGER.Models;
using FLEET_MANAGER.ViewModels;

namespace FLEET_MANAGER.Views
{
    public partial class VehiculeView : UserControl
    {
        public VehiculeView()
        {
            InitializeComponent();
        }

        private void VehicleCard_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.FrameworkElement element && element.Tag is Vehicule vehicule)
            {
                if (DataContext is VehiculeViewModel viewModel)
                {
                    viewModel.VehiculeSelectionne = vehicule;
                }
            }
        }

        private void BtnNouveauVehicule_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is VehiculeViewModel viewModel)
            {
                // Préparer le formulaire pour un nouveau véhicule
                viewModel.Nouveau.Execute(null);

                // Ouvrir le modal
                var dialog = new AddVehiculeDialog(viewModel)
                {
                    Owner = Window.GetWindow(this)
                };

                if (dialog.ShowDialog() == true)
                {
                    // Le véhicule a été ajouté avec succès
                    viewModel.ChargerVehicules();
                }
                else
                {
                    // L'utilisateur a annulé
                    viewModel.AnnulerCommand.Execute(null);
                }
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is VehiculeViewModel viewModel && viewModel.VehiculeSelectionne != null)
            {
                // Préparer le formulaire en mode édition
                viewModel.ModifierCommand.Execute(null);

                // Ouvrir le modal
                var dialog = new AddVehiculeDialog(viewModel)
                {
                    Owner = Window.GetWindow(this),
                    Title = "Modifier le véhicule"
                };

                if (dialog.ShowDialog() == true)
                {
                    // Le véhicule a été modifié avec succès
                    viewModel.ChargerVehicules();
                }
                else
                {
                    // L'utilisateur a annulé
                    viewModel.AnnulerCommand.Execute(null);
                }
            }
        }

        private void BtnAjouterPleinTrajet_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is VehiculeViewModel viewModel && viewModel.VehiculeSelectionne != null)
            {
                // Obtenir la fenêtre principale
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    // Naviguer vers la page Carburant & Trajet avec le véhicule sélectionné
                    mainWindow.NaviguerVersCarburantTrajet(viewModel.VehiculeSelectionne);
                }
            }
        }
    }
}
