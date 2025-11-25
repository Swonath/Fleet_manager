using System.Collections.ObjectModel;
using System.Windows.Input;
using FLEET_MANAGER.Models;
using FLEET_MANAGER.Repositories;

namespace FLEET_MANAGER.ViewModels
{
    /// <summary>
    /// ViewModel combin� pour la gestion du carburant et des trajets
    /// </summary>
    public class CarburantTrajetViewModel : ViewModelBase
    {
        private ObservableCollection<Vehicule> _vehicules;
        private Vehicule? _vehiculeSelectionne;
        private ObservableCollection<Carburant> _historique;
        private ObservableCollection<Trajet> _trajets;

        // Propri�t�s Carburant
        private decimal _quantiteLitres = 0;
        private decimal _coutTotal = 0;
        private decimal _coutParLitre = 0;
        private DateTime _dateSaisie = DateTime.Now;
        private int _kilometrageCarburant = 0;
        private string _notesCarburant = string.Empty;

        // Propri�t�s Trajet
        private TimeSpan _heureDepart = new TimeSpan(9, 0, 0);
        private TimeSpan _heureArrivee = new TimeSpan(17, 0, 0);
        private int _kilometrageDepart = 0;
        private int _kilometrageArrivee = 0;
        private int _distanceParcourue = 0;
        private string _lieuDepart = string.Empty;
        private string _lieuArrivee = string.Empty;
        private string _typeTrajet = "Professionnel";
        private string _notesTrajet = string.Empty;
        private DateTime _dateTrajet = DateTime.Now;

        private string _messageErreur = string.Empty;
        private bool _isLoading = false;

        private VehiculeRepository _vehiculeRepository;
        private CarburantRepository _carburantRepository;
        private TrajetRepository _trajetRepository;

        // Pagination Carburant
        private const int TAILLE_PAGE = 12;
        private List<Carburant> _tousLesCarburants = new List<Carburant>();
        private int _nombreCarburantsCharges = 0;
        private bool _tousCarburantsCharges = false;

        // Pagination Trajets
        private List<Trajet> _tousLesTrajets = new List<Trajet>();
        private int _nombreTrajetsCharges = 0;
        private bool _tousTrajetsCharges = false;

        public bool TousCarburantsCharges
        {
            get => _tousCarburantsCharges;
            set => SetProperty(ref _tousCarburantsCharges, value, nameof(TousCarburantsCharges));
        }

        public bool TousTrajetsCharges
        {
            get => _tousTrajetsCharges;
            set => SetProperty(ref _tousTrajetsCharges, value, nameof(TousTrajetsCharges));
        }

        public ObservableCollection<Vehicule> Vehicules
        {
            get => _vehicules;
            set => SetProperty(ref _vehicules, value, nameof(Vehicules));
        }

        public Vehicule? VehiculeSelectionne
        {
            get => _vehiculeSelectionne;
            set
            {
                if (SetProperty(ref _vehiculeSelectionne, value, nameof(VehiculeSelectionne)))
                {
                    if (value != null)
                    {
                        ChargerHistorique(value.IdVehicule);
                        ChargerTrajets(value.IdVehicule);
                        // Pr�-remplir les kilom�tres depuis le dernier carburant
                        if (_historique.Count > 0)
                        {
                            _kilometrageDepart = _historique.Last().Kilometrage;
                        }
                    }
                }
            }
        }

        public ObservableCollection<Carburant> Historique
        {
            get => _historique;
            set => SetProperty(ref _historique, value, nameof(Historique));
        }

        public ObservableCollection<Trajet> Trajets
        {
            get => _trajets;
            set => SetProperty(ref _trajets, value, nameof(Trajets));
        }

        // Propri�t�s Carburant
        public decimal QuantiteLitres
        {
            get => _quantiteLitres;
            set
            {
                if (SetProperty(ref _quantiteLitres, value, nameof(QuantiteLitres)))
                {
                    CalculerCoutParLitre();
                }
            }
        }

        public decimal CoutTotal
        {
            get => _coutTotal;
            set
            {
                if (SetProperty(ref _coutTotal, value, nameof(CoutTotal)))
                {
                    CalculerCoutParLitre();
                }
            }
        }

        public decimal CoutParLitre
        {
            get => _coutParLitre;
            set => SetProperty(ref _coutParLitre, value, nameof(CoutParLitre));
        }

        public DateTime DateSaisie
        {
            get => _dateSaisie;
            set => SetProperty(ref _dateSaisie, value, nameof(DateSaisie));
        }

        public int KilometrageCarburant
        {
            get => _kilometrageCarburant;
            set => SetProperty(ref _kilometrageCarburant, value, nameof(KilometrageCarburant));
        }

        public string NotesCarburant
        {
            get => _notesCarburant;
            set => SetProperty(ref _notesCarburant, value, nameof(NotesCarburant));
        }

        // Propri�t�s Trajet
        public DateTime DateTrajet
        {
            get => _dateTrajet;
            set => SetProperty(ref _dateTrajet, value, nameof(DateTrajet));
        }

        public TimeSpan HeureDepart
        {
            get => _heureDepart;
            set => SetProperty(ref _heureDepart, value, nameof(HeureDepart));
        }

        public TimeSpan HeureArrivee
        {
            get => _heureArrivee;
            set => SetProperty(ref _heureArrivee, value, nameof(HeureArrivee));
        }

        public int KilometrageDepart
        {
            get => _kilometrageDepart;
            set
            {
                if (SetProperty(ref _kilometrageDepart, value, nameof(KilometrageDepart)))
                {
                    CalculerDistance();
                }
            }
        }

        public int KilometrageArrivee
        {
            get => _kilometrageArrivee;
            set
            {
                if (SetProperty(ref _kilometrageArrivee, value, nameof(KilometrageArrivee)))
                {
                    CalculerDistance();
                }
            }
        }

        public int DistanceParcourue
        {
            get => _distanceParcourue;
            set => SetProperty(ref _distanceParcourue, value, nameof(DistanceParcourue));
        }

        public string LieuDepart
        {
            get => _lieuDepart;
            set => SetProperty(ref _lieuDepart, value, nameof(LieuDepart));
        }

        public string LieuArrivee
        {
            get => _lieuArrivee;
            set => SetProperty(ref _lieuArrivee, value, nameof(LieuArrivee));
        }

        public string TypeTrajet
        {
            get => _typeTrajet;
            set => SetProperty(ref _typeTrajet, value, nameof(TypeTrajet));
        }

        public string NotesTrajet
        {
            get => _notesTrajet;
            set => SetProperty(ref _notesTrajet, value, nameof(NotesTrajet));
        }

        public string MessageErreur
        {
            get => _messageErreur;
            set => SetProperty(ref _messageErreur, value, nameof(MessageErreur));
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value, nameof(IsLoading));
        }

        // Commandes
        public ICommand EnregistrerCarburantCommand { get; }
        public ICommand EnregistrerTrajetCommand { get; }
        public ICommand ReinitialiserCommand { get; }
        public ICommand ChargerPlusCarburantCommand { get; }
        public ICommand ChargerPlusTrajetsCommand { get; }

        public CarburantTrajetViewModel()
        {
            _vehicules = new ObservableCollection<Vehicule>();
            _historique = new ObservableCollection<Carburant>();
            _trajets = new ObservableCollection<Trajet>();
            _vehiculeRepository = new VehiculeRepository();
            _carburantRepository = new CarburantRepository();
            _trajetRepository = new TrajetRepository();

            EnregistrerCarburantCommand = new RelayCommand(_ => PerformerEnregistrementCarburant(), _ => VehiculeSelectionne != null && !IsLoading);
            EnregistrerTrajetCommand = new RelayCommand(_ => PerformerEnregistrementTrajet(), _ => VehiculeSelectionne != null && !IsLoading);
            ReinitialiserCommand = new RelayCommand(_ => Reinitialiser());
            ChargerPlusCarburantCommand = new RelayCommand(_ => ChargerCarburantsSuivants());
            ChargerPlusTrajetsCommand = new RelayCommand(_ => ChargerTrajetsSuivants());

            ChargerVehicules();
        }

        private void ChargerVehicules()
        {
            try
            {
                var vehicules = _vehiculeRepository.ObtenirTousLesVehicules();
                Vehicules = new ObservableCollection<Vehicule>(vehicules);
                MessageErreur = string.Empty;
            }
            catch (Exception ex)
            {
                MessageErreur = $"Erreur lors du chargement des vehicules : {ex.Message}";
            }
        }

        public void RéchargerVéhicules()
        {
            ChargerVehicules();
        }

        private void ChargerHistorique(int idVehicule)
        {
            try
            {
                // Charger tous les carburants pour le véhicule
                _tousLesCarburants = _carburantRepository.ObtenirCarburantParVehicule(idVehicule);

                // Réinitialiser la pagination
                _nombreCarburantsCharges = 0;
                Historique.Clear();

                // Charger la première page
                ChargerCarburantsSuivants();
            }
            catch (Exception ex)
            {
                MessageErreur = $"Erreur lors du chargement de l'historique : {ex.Message}";
            }
        }

        private void ChargerCarburantsSuivants()
        {
            try
            {
                var carburantsACharger = _tousLesCarburants
                    .Skip(_nombreCarburantsCharges)
                    .Take(TAILLE_PAGE)
                    .ToList();

                foreach (var carburant in carburantsACharger)
                {
                    Historique.Add(carburant);
                }

                _nombreCarburantsCharges += carburantsACharger.Count;
                TousCarburantsCharges = _nombreCarburantsCharges >= _tousLesCarburants.Count;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des carburants suivants : {ex.Message}");
            }
        }

        private void ChargerTrajets(int idVehicule)
        {
            try
            {
                // Charger tous les trajets pour le véhicule
                _tousLesTrajets = _trajetRepository.ObtenirTrajetsParVehicule(idVehicule);

                // Réinitialiser la pagination
                _nombreTrajetsCharges = 0;
                Trajets.Clear();

                // Charger la première page
                ChargerTrajetsSuivants();
            }
            catch (Exception ex)
            {
                MessageErreur = $"Erreur lors du chargement des trajets : {ex.Message}";
            }
        }

        private void ChargerTrajetsSuivants()
        {
            try
            {
                var trajetsACharger = _tousLesTrajets
                    .Skip(_nombreTrajetsCharges)
                    .Take(TAILLE_PAGE)
                    .ToList();

                foreach (var trajet in trajetsACharger)
                {
                    Trajets.Add(trajet);
                }

                _nombreTrajetsCharges += trajetsACharger.Count;
                TousTrajetsCharges = _nombreTrajetsCharges >= _tousLesTrajets.Count;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des trajets suivants : {ex.Message}");
            }
        }

        private void PerformerEnregistrementCarburant()
        {
            if (VehiculeSelectionne == null)
            {
                MessageErreur = "Veuillez selectionner un vehicule.";
                return;
            }

            if (QuantiteLitres <= 0 || CoutTotal <= 0 || KilometrageCarburant <= 0)
            {
                MessageErreur = "Veuillez remplir tous les champs correctement.";
                return;
            }

            try
            {
                IsLoading = true;

                var carburant = new Carburant
                {
                    IdVehicule = VehiculeSelectionne.IdVehicule,
                    DateSaisie = DateSaisie,
                    QuantiteLitres = QuantiteLitres,
                    CoutTotal = CoutTotal,
                    CoutParLitre = CoutParLitre,
                    Kilometrage = KilometrageCarburant,
                    Notes = NotesCarburant,
                    IdUtilisateur = 1 // TODO: Utiliser l'utilisateur connecte
                };

                if (_carburantRepository.AjouterCarburant(carburant))
                {
                    MessageErreur = "Ravitaillement enregistre avec succes !";
                    ChargerHistorique(VehiculeSelectionne.IdVehicule);
                    // Pr�-remplir le kilom�trage du trajet
                    KilometrageDepart = KilometrageCarburant;
                }
                else
                {
                    MessageErreur = "Erreur lors de l'enregistrement.";
                }
            }
            catch (Exception ex)            {
                MessageErreur = $"Erreur : {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void PerformerEnregistrementTrajet()
        {
            if (VehiculeSelectionne == null)
            {
                MessageErreur = "Veuillez selectionner un vehicule.";
                return;
            }

            if (string.IsNullOrWhiteSpace(LieuDepart) || string.IsNullOrWhiteSpace(LieuArrivee))
            {
                MessageErreur = "Les lieux de depart et d'arrivee sont requis.";
                return;
            }

            if (DistanceParcourue <= 0)
            {
                MessageErreur = "La distance doit etre superieure a 0.";
                return;
            }

            try
            {
                IsLoading = true;

                var trajet = new Trajet
                {
                    IdVehicule = VehiculeSelectionne.IdVehicule,
                    DateTrajet = DateTrajet,
                    HeureDepart = HeureDepart,
                    HeureArrivee = HeureArrivee,
                    KilomettrageDepart = KilometrageDepart,
                    KilomettrageArrivee = KilometrageArrivee,
                    DistanceParcourue = DistanceParcourue,
                    LieuDepart = LieuDepart,
                    LieuArrivee = LieuArrivee,
                    TypeTrajet = TypeTrajet,
                    Notes = NotesTrajet,
                    IdUtilisateur = 1  // TODO: Utiliser l'utilisateur connecte
                };

                if (_trajetRepository.AjouterTrajet(trajet))
                {
                    MessageErreur = "Trajet enregistre avec succes !";
                    ChargerTrajets(VehiculeSelectionne.IdVehicule);
                    Reinitialiser();
                }
                else
                {
                    MessageErreur = "Erreur lors de l'enregistrement.";
                }
            }
            catch (Exception ex)
            {
                MessageErreur = $"Erreur : {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CalculerCoutParLitre()
        {
            if (QuantiteLitres > 0 && CoutTotal > 0)
            {
                CoutParLitre = CoutTotal / QuantiteLitres;
            }
        }

        private void CalculerDistance()
        {
            DistanceParcourue = KilometrageArrivee - KilometrageDepart;
        }

        private void Reinitialiser()
        {
            QuantiteLitres = 0;
            CoutTotal = 0;
            CoutParLitre = 0;
            DateSaisie = DateTime.Now;
            KilometrageCarburant = 0;
            NotesCarburant = string.Empty;

            DateTrajet = DateTime.Now;
            HeureDepart = new TimeSpan(9, 0, 0);
            HeureArrivee = new TimeSpan(17, 0, 0);
            KilometrageDepart = 0;
            KilometrageArrivee = 0;
            DistanceParcourue = 0;
            LieuDepart = string.Empty;
            LieuArrivee = string.Empty;
            TypeTrajet = "Professionnel";
            NotesTrajet = string.Empty;
        }
    }
}
