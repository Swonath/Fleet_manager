using System.Windows;
using System.Windows.Controls;
using FLEET_MANAGER.ViewModels;
using FLEET_MANAGER.Models;

namespace FLEET_MANAGER.Views
{
    /// <summary>
    /// Vue pour la gestion des utilisateurs
    /// G�re notamment le PasswordBox qui ne supporte pas le binding en WPF
    /// </summary>
    public partial class UtilisateurView : UserControl
    {
        public UtilisateurView()
        {
            InitializeComponent();
            // Le DataContext est assign� par MainWindow, ne pas en cr�er un ici
        }

        /// <summary>
        /// Gestionnaire d'�v�nement pour transmettre le mot de passe au ViewModel
        /// Le PasswordBox ne supporte pas le binding direct en WPF pour des raisons de s�curit�
        /// </summary>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UtilisateurViewModel viewModel)
            {
                // Transmettre le mot de passe au ViewModel
                viewModel.MotDePasse = PasswordBox.Password;
            }
        }
    }
}
