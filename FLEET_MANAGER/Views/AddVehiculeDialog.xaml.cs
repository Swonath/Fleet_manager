using System.Windows;
using FLEET_MANAGER.ViewModels;

namespace FLEET_MANAGER.Views
{
    public partial class AddVehiculeDialog : Window
    {
        public VehiculeViewModel ViewModel { get; }

        public AddVehiculeDialog(VehiculeViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = viewModel;

            // Mettre à jour le titre et le bouton en fonction du mode
            if (viewModel.EstNouveauVehicule)
            {
                TitleTextBlock.Text = "Ajouter un véhicule";
                BtnSave.Content = "Ajouter le véhicule";
            }
            else
            {
                TitleTextBlock.Text = "Modifier le véhicule";
                BtnSave.Content = "Modifier le véhicule";
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SauvegarderCommand.CanExecute(null))
            {
                ViewModel.SauvegarderCommand.Execute(null);

                // Fermer le dialog si pas d'erreur
                if (string.IsNullOrEmpty(ViewModel.MessageErreur) ||
                    ViewModel.MessageErreur.Contains("succès"))
                {
                    DialogResult = true;
                    Close();
                }
            }
        }
    }
}
