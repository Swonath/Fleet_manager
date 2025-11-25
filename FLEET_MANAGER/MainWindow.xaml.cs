using System.Windows;
using System.Windows.Controls;
using FLEET_MANAGER.Models;
using FLEET_MANAGER.ViewModels;
using FLEET_MANAGER.Views;

namespace FLEET_MANAGER
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DashboardViewModel _dashboardViewModel;
        private VehiculeViewModel _vehiculeViewModel;
        private CarburantTrajetViewModel _carburantTrajetViewModel;
        private UtilisateurViewModel _utilisateurViewModel;

        private DashboardView _dashboardView;
        private VehiculeView _vehiculeView;
        private CarburantTrajetView _carburantTrajetView;
        private UtilisateurView _utilisateurView;

        private Utilisateur? _utilisateurConnecte;

        public Utilisateur? UtilisateurConnecte
        {
            get => _utilisateurConnecte;
            set => _utilisateurConnecte = value;
        }

        public MainWindow()
        {
            InitializeComponent();
            
            _dashboardViewModel = new DashboardViewModel();
            _vehiculeViewModel = new VehiculeViewModel();
            _carburantTrajetViewModel = new CarburantTrajetViewModel();
            _utilisateurViewModel = new UtilisateurViewModel();

            _dashboardView = new DashboardView { DataContext = _dashboardViewModel };
            _vehiculeView = new VehiculeView { DataContext = _vehiculeViewModel };
            _carburantTrajetView = new CarburantTrajetView { DataContext = _carburantTrajetViewModel };
            _utilisateurView = new UtilisateurView { DataContext = _utilisateurViewModel };
            
            // S'abonner a la notification de mise a jour des vehicules
            _vehiculeViewModel.VehiculesChange += OnVehiculesChange;
            
            DataContext = _dashboardViewModel;
        }

        private void OnVehiculesChange()
        {
            // Recharger les vehicules dans les autres ViewModels
            _carburantTrajetViewModel.RéchargerVéhicules();
            _dashboardViewModel.RéchargerDashboard();
        }

        public void InitialiserAvecUtilisateur(Utilisateur utilisateur)
        {
            UtilisateurConnecte = utilisateur;
            _dashboardViewModel.InitialiserAvecUtilisateur(utilisateur);
            _utilisateurViewModel.InitialiserAvecUtilisateurConnecte(utilisateur);

            // Afficher/masquer le bouton Utilisateurs selon les droits
            if (utilisateur.Role == "admin" || utilisateur.Role == "super_admin")
            {
                BtnUtilisateurs.Visibility = Visibility.Visible;
            }
            else
            {
                BtnUtilisateurs.Visibility = Visibility.Collapsed;
            }

            // Afficher le dashboard par défaut
            BtnDashboard_Click(null, null);
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            DataContext = _dashboardViewModel;
            ContentArea.Content = _dashboardView;
        }

        private void BtnVehicules_Click(object sender, RoutedEventArgs e)
        {
            DataContext = _vehiculeViewModel;
            ContentArea.Content = _vehiculeView;
        }

        private void BtnCarburantTrajet_Click(object sender, RoutedEventArgs e)
        {
            DataContext = _carburantTrajetViewModel;
            ContentArea.Content = _carburantTrajetView;
        }

        private void BtnUtilisateurs_Click(object sender, RoutedEventArgs e)
        {
            if (UtilisateurConnecte?.Role != "admin" && UtilisateurConnecte?.Role != "super_admin")
            {
                MessageBox.Show("Vous n'avez pas accès à cette section.", "Accès refusé");
                return;
            }

            DataContext = _utilisateurViewModel;
            ContentArea.Content = _utilisateurView;
            _utilisateurViewModel.ChargerUtilisateurs();
        }

        private void BtnProfil_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Module Profil - En développement", "Profil");
        }

        private void BtnMaintenances_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Module Maintenances - A venir");
        }

        private void BtnRapports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Module Rapports - A venir");
        }

        private void BtnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            // Retourner à la page de connexion
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Btn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#3b82f6"));
            }
        }

        private void Btn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Background = System.Windows.Media.Brushes.Transparent;
            }
        }
    }
}