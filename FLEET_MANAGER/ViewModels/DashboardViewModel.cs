using System.Collections.ObjectModel;
using System.Windows.Input;
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
        private ObservableCollection<Vehicule> _vehiculesFiltres;
        private string _rechercheTexte = string.Empty;
        private int _totalVehicules;
        private int _vehiculesEnService;
        private int _vehiculesEnMaintenance;
        private int _vehiculesDisponibles;
        private decimal _coutTotalCarburant;
        private bool _estEnChargement;

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

        public ObservableCollection<Vehicule> VehiculesFiltres
        {
            get => _vehiculesFiltres;
            set => SetProperty(ref _vehiculesFiltres, value, nameof(VehiculesFiltres));
        }

        public string RechercheTexte
        {
            get => _rechercheTexte;
            set
            {
                if (SetProperty(ref _rechercheTexte, value, nameof(RechercheTexte)))
                {
                    AppliquerFiltre();
                }
            }
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

        public bool EstEnChargement
        {
            get => _estEnChargement;
            set => SetProperty(ref _estEnChargement, value, nameof(EstEnChargement));
        }

        private VehiculeRepository _vehiculeRepository;
        private CarburantRepository _carburantRepository;
        private bool _donneesChargees = false;
        private int _nombreVehiculesCharges = 0;
        private const int TAILLE_PAGE = 12;
        private List<Vehicule> _tousLesVehicules = new List<Vehicule>();
        private bool _tousVehiculesCharges = false;

        public bool TousVehiculesCharges
        {
            get => _tousVehiculesCharges;
            set => SetProperty(ref _tousVehiculesCharges, value, nameof(TousVehiculesCharges));
        }

        public ICommand ChargerPlusCommand { get; }
        public ICommand SelectionnerVehiculeCommand { get; }

        // Événement pour la navigation
        public event Action<Vehicule>? NaviguerVersVehicule;

        public DashboardViewModel()
        {
            _vehicules = new ObservableCollection<Vehicule>();
            _vehiculesFiltres = new ObservableCollection<Vehicule>();
            _vehiculeRepository = new VehiculeRepository();
            _carburantRepository = new CarburantRepository();
            ChargerPlusCommand = new RelayCommand(_ => ChargerVehiculesSuivants());
            SelectionnerVehiculeCommand = new RelayCommand(param =>
            {
                if (param is Vehicule vehicule)
                {
                    NaviguerVersVehicule?.Invoke(vehicule);
                }
            });
            // Ne plus charger les données au constructeur
        }

        public async Task ChargerDonneesAsync()
        {
            if (_donneesChargees)
                return; // Éviter de recharger si déjà chargé

            EstEnChargement = true;
            try
            {
                await Task.Run(() =>
                {
                    // Charger tous les véhicules en arrière-plan pour les statistiques
                    _tousLesVehicules = _vehiculeRepository.ObtenirTousLesVehicules();

                    // Calculer les statistiques sur tous les véhicules
                    TotalVehicules = _tousLesVehicules.Count;
                    VehiculesEnService = _tousLesVehicules.Count(v => v.Etat == "En service");
                    VehiculesEnMaintenance = _tousLesVehicules.Count(v => v.Etat == "En maintenance");
                    VehiculesDisponibles = _tousLesVehicules.Count(v => v.Etat == "Disponible");

                    // Calculer le coût total de carburant
                    CalculerCoutTotalCarburant();
                });

                // Charger seulement les 12 premiers véhicules pour l'affichage
                ChargerVehiculesSuivants();

                _donneesChargees = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des données : {ex.Message}");
            }
            finally
            {
                EstEnChargement = false;
            }
        }

        private void ChargerVehiculesSuivants()
        {
            try
            {
                var vehiculesACharger = _tousLesVehicules
                    .Skip(_nombreVehiculesCharges)
                    .Take(TAILLE_PAGE)
                    .ToList();

                foreach (var vehicule in vehiculesACharger)
                {
                    Vehicules.Add(vehicule);
                }

                _nombreVehiculesCharges += vehiculesACharger.Count;
                TousVehiculesCharges = _nombreVehiculesCharges >= _tousLesVehicules.Count;

                // Appliquer le filtre après le chargement
                AppliquerFiltre();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des véhicules suivants : {ex.Message}");
            }
        }

        private void AppliquerFiltre()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(RechercheTexte))
                {
                    VehiculesFiltres = new ObservableCollection<Vehicule>(Vehicules);
                }
                else
                {
                    var texteRecherche = RechercheTexte.ToLower();
                    var vehiculesFiltres = Vehicules.Where(v =>
                        v.Marque.ToLower().Contains(texteRecherche) ||
                        v.Modele.ToLower().Contains(texteRecherche) ||
                        v.Immatriculation.ToLower().Contains(texteRecherche) ||
                        v.Etat.ToLower().Contains(texteRecherche) ||
                        v.TypeCarburant.ToLower().Contains(texteRecherche)
                    ).ToList();

                    VehiculesFiltres = new ObservableCollection<Vehicule>(vehiculesFiltres);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du filtrage : {ex.Message}");
            }
        }

        private void CalculerCoutTotalCarburant()
        {
            try
            {
                decimal total = 0;
                foreach (var vehicule in _tousLesVehicules)
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
            _ = ChargerDonneesAsync();
        }

        public void RéchargerDashboard()
        {
            _donneesChargees = false;
            _nombreVehiculesCharges = 0;
            _tousVehiculesCharges = false;
            Vehicules.Clear();
            _tousLesVehicules.Clear();
            _ = ChargerDonneesAsync();
        }
    }
}
