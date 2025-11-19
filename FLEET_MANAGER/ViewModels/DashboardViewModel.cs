using System.Collections.ObjectModel;
using FLEET_MANAGER.Models;
using FLEET_MANAGER.Repositories;

namespace FLEET_MANAGER.ViewModels
{
    /// <summary>
    /// ViewModel pour le tableau de bord principal
    /// </summary>
    public class DashboardViewModel : ViewModelBase
    {
        private Utilisateur? _utilisateurConnecte;
        private ObservableCollection<Vehicule> _vehicules;
        private int _totalVehicules;
        private int _vehiculesEnService;
        private int _vehiculesEnMaintenance;
        private int _vehiculesDisponibles;
        private decimal _coutTotalCarburant;

        public Utilisateur? UtilisateurConnecte
        {
            get => _utilisateurConnecte;
            set => SetProperty(ref _utilisateurConnecte, value, nameof(UtilisateurConnecte));
        }

        public ObservableCollection<Vehicule> Vehicules
        {
            get => _vehicules;
            set => SetProperty(ref _vehicules, value, nameof(Vehicules));
        }

        public int TotalVehicules
        {
            get => _totalVehicules;
            set => SetProperty(ref _totalVehicules, value, nameof(TotalVehicules));
        }

        public int VehiculesEnService
        {
            get => _vehiculesEnService;
            set => SetProperty(ref _vehiculesEnService, value, nameof(VehiculesEnService));
        }

        public int VehiculesEnMaintenance
        {
            get => _vehiculesEnMaintenance;
            set => SetProperty(ref _vehiculesEnMaintenance, value, nameof(VehiculesEnMaintenance));
        }

        public int VehiculesDisponibles
        {
            get => _vehiculesDisponibles;
            set => SetProperty(ref _vehiculesDisponibles, value, nameof(VehiculesDisponibles));
        }

        public decimal CoutTotalCarburant
        {
            get => _coutTotalCarburant;
            set => SetProperty(ref _coutTotalCarburant, value, nameof(CoutTotalCarburant));
        }

        private VehiculeRepository _vehiculeRepository;
        private CarburantRepository _carburantRepository;

        public DashboardViewModel()
        {
            _vehicules = new ObservableCollection<Vehicule>();
            _vehiculeRepository = new VehiculeRepository();
            _carburantRepository = new CarburantRepository();
            ChargerDonnees();
        }

        public void ChargerDonnees()
        {
            try
            {
                var vehicules = _vehiculeRepository.ObtenirTousLesVehicules();
                Vehicules = new ObservableCollection<Vehicule>(vehicules);
                
                // Calculer les statistiques
                TotalVehicules = vehicules.Count;
                VehiculesEnService = vehicules.Count(v => v.Etat == "En service");
                VehiculesEnMaintenance = vehicules.Count(v => v.Etat == "En maintenance");
                VehiculesDisponibles = vehicules.Count(v => v.Etat == "Disponible");
                
                // Calculer le coût total de carburant pour tous les véhicules
                CalculerCoutTotalCarburant();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des données : {ex.Message}");
            }
        }

        private void CalculerCoutTotalCarburant()
        {
            try
            {
                decimal total = 0;
                foreach (var vehicule in Vehicules)
                {
                    var carburants = _carburantRepository.ObtenirCarburantParVehicule(vehicule.IdVehicule);
                    total += carburants.Sum(c => c.CoutTotal);
                }
                CoutTotalCarburant = total;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du calcul du coût : {ex.Message}");
            }
        }

        public void InitialiserAvecUtilisateur(Utilisateur utilisateur)
        {
            UtilisateurConnecte = utilisateur;
            ChargerDonnees();
        }

        public void RéchargerDashboard()
        {
            ChargerDonnees();
        }
    }
}
