using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FLEET_MANAGER.Models;
using FLEET_MANAGER.Repositories;

namespace FLEET_MANAGER.ViewModels
{
    /// <summary>
    /// ViewModel pour la gestion des véhicules
    /// </summary>
    public class VehiculeViewModel : ViewModelBase
    {
        private ObservableCollection<Vehicule> _vehicules;
        private ObservableCollection<Vehicule> _vehiculesFiltres;
        private Vehicule? _vehiculeSelectionne;
        private Vehicule _nouveau;
        private bool _estEnEdition = false;
        private string _messageErreur = string.Empty;
        private bool _isLoading = false;
        private bool _estNouveauVehicule = false; // Ajouter cette propriété
        private string _rechercheTexte = string.Empty;
        private string _filtreStatut = "Tous";
        private ObservableCollection<ElementHistorique> _historique;
        private List<ElementHistorique> _historiqueComplet = new List<ElementHistorique>();
        private string _filtreHistorique = "Tout";
        private ObservableCollection<PointGraphique> _pointsGraphique;

        private VehiculeRepository _repository;
        private CarburantRepository _carburantRepository;
        private TrajetRepository _trajetRepository;

        // Evenement pour notifier les changements
        public event Action? VehiculesChange;

        // Proprietes pour le formulaire
        private string _marque = string.Empty;
        private string _modele = string.Empty;
        private string _immatriculation = string.Empty;
        private int _annee = DateTime.Now.Year;
        private string _typeCarburant = "Essence";
        private int _kilometrageInitial = 0;
        private DateTime _dateAcquisition = DateTime.Now;
        private string _etat = "En service";

        // Erreurs de validation
        private string _marqueError = string.Empty;
        private string _modeleError = string.Empty;
        private string _immatriculationError = string.Empty;
        private string _kilometrageInitialError = string.Empty;

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
                    AppliquerFiltres();
                }
            }
        }

        public string FiltreStatut
        {
            get => _filtreStatut;
            set
            {
                if (SetProperty(ref _filtreStatut, value, nameof(FiltreStatut)))
                {
                    AppliquerFiltres();
                }
            }
        }

        public ObservableCollection<ElementHistorique> Historique
        {
            get => _historique;
            set => SetProperty(ref _historique, value, nameof(Historique));
        }

        public ObservableCollection<PointGraphique> PointsGraphique
        {
            get => _pointsGraphique;
            set => SetProperty(ref _pointsGraphique, value, nameof(PointsGraphique));
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
                        ChargerVehiculeEnFormulaire(value);
                        ChargerHistorique(value.IdVehicule);
                    }
                    else
                    {
                        Historique = new ObservableCollection<ElementHistorique>();
                    }
                }
            }
        }

        public string Marque
        {
            get => _marque;
            set
            {
                if (SetProperty(ref _marque, value, nameof(Marque)))
                {
                    ValidateMarque();
                }
            }
        }

        public string Modele
        {
            get => _modele;
            set
            {
                if (SetProperty(ref _modele, value, nameof(Modele)))
                {
                    ValidateModele();
                }
            }
        }

        public string Immatriculation
        {
            get => _immatriculation;
            set
            {
                if (SetProperty(ref _immatriculation, value, nameof(Immatriculation)))
                {
                    ValidateImmatriculation();
                }
            }
        }

        public int Annee
        {
            get => _annee;
            set => SetProperty(ref _annee, value, nameof(Annee));
        }

        public string TypeCarburant
        {
            get => _typeCarburant;
            set => SetProperty(ref _typeCarburant, value, nameof(TypeCarburant));
        }

        public int KilomettrageInitial
        {
            get => _kilometrageInitial;
            set
            {
                if (SetProperty(ref _kilometrageInitial, value, nameof(KilomettrageInitial)))
                {
                    ValidateKilometrageInitial();
                }
            }
        }

        public DateTime DateAcquisition
        {
            get => _dateAcquisition;
            set => SetProperty(ref _dateAcquisition, value, nameof(DateAcquisition));
        }

        public string Etat
        {
            get => _etat;
            set => SetProperty(ref _etat, value, nameof(Etat));
        }

        public bool EstEnEdition
        {
            get => _estEnEdition;
            set => SetProperty(ref _estEnEdition, value, nameof(EstEnEdition));
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

        public string MarqueError
        {
            get => _marqueError;
            set => SetProperty(ref _marqueError, value, nameof(MarqueError));
        }

        public string ModeleError
        {
            get => _modeleError;
            set => SetProperty(ref _modeleError, value, nameof(ModeleError));
        }

        public string ImmatriculationError
        {
            get => _immatriculationError;
            set => SetProperty(ref _immatriculationError, value, nameof(ImmatriculationError));
        }

        public string KilometrageInitialError
        {
            get => _kilometrageInitialError;
            set => SetProperty(ref _kilometrageInitialError, value, nameof(KilometrageInitialError));
        }

        public bool EstNouveauVehicule
        {
            get => _estNouveauVehicule;
            set => SetProperty(ref _estNouveauVehicule, value, nameof(EstNouveauVehicule));
        }

        public string FiltreHistorique
        {
            get => _filtreHistorique;
            set
            {
                if (SetProperty(ref _filtreHistorique, value, nameof(FiltreHistorique)))
                {
                    AppliquerFiltreHistorique();
                }
            }
        }

        // Commandes
        public ICommand ChargerCommand { get; }
        public ICommand AjouterCommand { get; }
        public ICommand SauvegarderCommand { get; }
        public ICommand ModifierCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand AnnulerCommand { get; }
        public ICommand Nouveau { get; }
        public ICommand ChangerFiltreHistoriqueCommand { get; }

        public VehiculeViewModel()
        {
            _vehicules = new ObservableCollection<Vehicule>();
            _vehiculesFiltres = new ObservableCollection<Vehicule>();
            _historique = new ObservableCollection<ElementHistorique>();
            _pointsGraphique = new ObservableCollection<PointGraphique>();
            _nouveau = new Vehicule();
            _repository = new VehiculeRepository();
            _carburantRepository = new CarburantRepository();
            _trajetRepository = new TrajetRepository();

            ChargerCommand = new RelayCommand(_ => ChargerVehicules());
            AjouterCommand = new RelayCommand(_ => PerformerAjout(), _ => !EstEnEdition && !IsLoading);
            SauvegarderCommand = new RelayCommand(_ => PerformerSauvegarde(), _ => EstEnEdition && !IsLoading);
            ModifierCommand = new RelayCommand(_ => PerformerModification(), _ => VehiculeSelectionne != null && !IsLoading);
            SupprimerCommand = new RelayCommand(_ => PerformerSuppression(), _ => VehiculeSelectionne != null && !IsLoading);
            AnnulerCommand = new RelayCommand(_ => AnnulerEdition());
            Nouveau = new RelayCommand(_ => NouveauVehicule());
            ChangerFiltreHistoriqueCommand = new RelayCommand(param => ChangerFiltreHistorique(param?.ToString() ?? "Tout"));

            ChargerVehicules();
        }

        public void ChargerVehicules()
        {
            try
            {
                IsLoading = true;
                var vehicules = _repository.ObtenirTousLesVehicules();
                Vehicules = new ObservableCollection<Vehicule>(vehicules);
                AppliquerFiltres();
                MessageErreur = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur chargement véhicules : {ex}");
                MessageErreur = "Erreur lors du chargement des véhicules.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void SelectionnerVehicule(Vehicule vehicule)
        {
            if (vehicule == null)
                return;

            // Charger les véhicules si nécessaire
            if (Vehicules == null || Vehicules.Count == 0)
            {
                ChargerVehicules();
            }

            // Sélectionner le véhicule
            VehiculeSelectionne = Vehicules.FirstOrDefault(v => v.IdVehicule == vehicule.IdVehicule);
        }

        private void AppliquerFiltres()
        {
            var vehiculesFiltres = Vehicules.AsEnumerable();

            // Filtre par texte de recherche
            if (!string.IsNullOrWhiteSpace(RechercheTexte))
            {
                var recherche = RechercheTexte.ToLower();
                vehiculesFiltres = vehiculesFiltres.Where(v =>
                    v.Marque.ToLower().Contains(recherche) ||
                    v.Modele.ToLower().Contains(recherche) ||
                    v.Immatriculation.ToLower().Contains(recherche));
            }

            // Filtre par statut
            if (FiltreStatut != "Tous")
            {
                vehiculesFiltres = vehiculesFiltres.Where(v => v.Etat == FiltreStatut);
            }

            VehiculesFiltres = new ObservableCollection<Vehicule>(vehiculesFiltres);
        }

        private void NouveauVehicule()
        {
            EstEnEdition = true;
            EstNouveauVehicule = true;
            VehiculeSelectionne = null;
            ReinitialiserFormulaire();
            MessageErreur = string.Empty;
        }

        private void ChargerVehiculeEnFormulaire(Vehicule vehicule)
        {
            Marque = vehicule.Marque;
            Modele = vehicule.Modele;
            Immatriculation = vehicule.Immatriculation;
            Annee = vehicule.AnneeFabrication;
            TypeCarburant = vehicule.TypeCarburant;
            KilomettrageInitial = vehicule.KilomettrageInitial;
            DateAcquisition = vehicule.DateAcquisition;
            Etat = vehicule.Etat;
        }

        private void ReinitialiserFormulaire()
        {
            Marque = string.Empty;
            Modele = string.Empty;
            Immatriculation = string.Empty;
            Annee = DateTime.Now.Year;
            TypeCarburant = "Essence";
            KilomettrageInitial = 0;
            DateAcquisition = DateTime.Now;
            Etat = "En service";
            MarqueError = string.Empty;
            ModeleError = string.Empty;
            ImmatriculationError = string.Empty;
            KilometrageInitialError = string.Empty;
        }

        private void PerformerAjout()
        {
            if (!ValiderFormulaire())
                return;

            try
            {
                IsLoading = true;
                var nouveau = new Vehicule
                {
                    Marque = Marque,
                    Modele = Modele,
                    Immatriculation = Immatriculation,
                    AnneeFabrication = Annee,
                    TypeCarburant = TypeCarburant,
                    KilomettrageInitial = KilomettrageInitial,
                    KilomettrageActuel = KilomettrageInitial,
                    DateAcquisition = DateAcquisition,
                    Etat = Etat
                };

                if (_repository.AjouterVehicule(nouveau))
                {
                    MessageErreur = "Vehicule ajoute avec succes !";
                    ChargerVehicules();
                    ReinitialiserFormulaire();
                    VehiculesChange?.Invoke();  // Notifier le changement
                }
                else
                {
                    MessageErreur = "Erreur lors de l'ajout du vehicule.";
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

        private void PerformerModification()
        {
            if (VehiculeSelectionne == null)
            {
                MessageErreur = "Aucun vehicule selectionne.";
                return;
            }

            EstEnEdition = true;
            EstNouveauVehicule = false;
            ChargerVehiculeEnFormulaire(VehiculeSelectionne);
            MessageErreur = string.Empty;
        }

        private void PerformerSauvegarde()
        {
            if (!ValiderFormulaire())
                return;

            try
            {
                IsLoading = true;

                if (EstNouveauVehicule)
                {
                    // Cas : Ajout d'un nouveau vehicule
                    var nouveau = new Vehicule
                    {
                        Marque = Marque,
                        Modele = Modele,
                        Immatriculation = Immatriculation,
                        AnneeFabrication = Annee,
                        TypeCarburant = TypeCarburant,
                        KilomettrageInitial = KilomettrageInitial,
                        KilomettrageActuel = KilomettrageInitial,
                        DateAcquisition = DateAcquisition,
                        Etat = Etat
                    };

                    if (_repository.AjouterVehicule(nouveau))
                    {
                        MessageErreur = "Vehicule ajoute avec succes !";
                        ChargerVehicules();
                        AnnulerEdition();
                        VehiculesChange?.Invoke();  // Notifier le changement
                    }
                    else
                    {
                        MessageErreur = "Erreur lors de l'ajout du vehicule.";
                    }
                }
                else if (VehiculeSelectionne != null)
                {
                    // Cas : Modification d'un vehicule existant
                    VehiculeSelectionne.Marque = Marque;
                    VehiculeSelectionne.Modele = Modele;
                    VehiculeSelectionne.Immatriculation = Immatriculation;
                    VehiculeSelectionne.AnneeFabrication = Annee;
                    VehiculeSelectionne.TypeCarburant = TypeCarburant;
                    VehiculeSelectionne.Etat = Etat;

                    if (_repository.ModifierVehicule(VehiculeSelectionne))
                    {
                        MessageErreur = "Vehicule modifie avec succes !";
                        ChargerVehicules();
                        AnnulerEdition();
                        VehiculesChange?.Invoke();  // Notifier le changement
                    }
                    else
                    {
                        MessageErreur = "Erreur lors de la modification.";
                    }
                }
                else
                {
                    MessageErreur = "Aucun vehicule selectionne.";
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

        private void PerformerSuppression()
        {
            if (VehiculeSelectionne == null)
            {
                MessageErreur = "Aucun vehicule selectionne.";
                return;
            }

            try
            {
                IsLoading = true;
                if (_repository.SupprimerVehicule(VehiculeSelectionne.IdVehicule))
                {
                    MessageErreur = "Vehicule supprime avec succes !";
                    ChargerVehicules();
                    ReinitialiserFormulaire();
                    VehiculesChange?.Invoke();  // Notifier le changement
                }
                else
                {
                    MessageErreur = "Erreur lors de la suppression.";
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

        private void AnnulerEdition()
        {
            EstEnEdition = false;
            EstNouveauVehicule = false;
            VehiculeSelectionne = null;
            ReinitialiserFormulaire();
            MessageErreur = string.Empty;
        }

        private bool ValiderFormulaire()
        {
            bool valide = true;

            if (string.IsNullOrWhiteSpace(Marque))
            {
                MarqueError = "La marque est requise.";
                valide = false;
            }
            else
            {
                MarqueError = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Modele))
            {
                ModeleError = "Le modele est requis.";
                valide = false;
            }
            else
            {
                ModeleError = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Immatriculation))
            {
                ImmatriculationError = "L'immatriculation est requise.";
                valide = false;
            }
            else
            {
                ImmatriculationError = string.Empty;
            }

            if (_kilometrageInitial < 0)
            {
                KilometrageInitialError = "Le kilometrage ne peut pas etre negatif.";
                valide = false;
            }
            else
            {
                KilometrageInitialError = string.Empty;
            }

            return valide;
        }

        private void ValidateMarque()
        {
            if (string.IsNullOrWhiteSpace(Marque))
                MarqueError = "La marque est requise.";
            else
                MarqueError = string.Empty;
        }

        private void ValidateModele()
        {
            if (string.IsNullOrWhiteSpace(Modele))
                ModeleError = "Le modele est requis.";
            else
                ModeleError = string.Empty;
        }

        private void ValidateImmatriculation()
        {
            if (string.IsNullOrWhiteSpace(Immatriculation))
                ImmatriculationError = "L'immatriculation est requise.";
            else
                ImmatriculationError = string.Empty;
        }

        private void ValidateKilometrageInitial()
        {
            if (_kilometrageInitial < 0)
                KilometrageInitialError = "Le kilometrage ne peut pas etre negatif.";
            else
                KilometrageInitialError = string.Empty;
        }

        private void ChargerHistorique(int idVehicule)
        {
            try
            {
                var historique = new List<ElementHistorique>();
                var pointsGraphique = new List<PointGraphique>();

                // Charger les ravitaillements en carburant
                var carburants = _carburantRepository.ObtenirCarburantParVehicule(idVehicule);
                foreach (var c in carburants)
                {
                    historique.Add(new ElementHistorique
                    {
                        Date = c.DateSaisie,
                        Type = "Carburant",
                        Titre = "Plein de carburant",
                        Description = $"{c.QuantiteLitres}L • {c.CoutTotal:F2} €",
                        Kilometrage = c.Kilometrage,
                        Icone = "⛽",
                        CouleurFond = "#10B981"
                    });

                    if (c.Kilometrage > 0)
                    {
                        pointsGraphique.Add(new PointGraphique
                        {
                            Date = c.DateSaisie,
                            Valeur = c.Kilometrage,
                            Label = c.DateSaisie.ToString("dd/MM")
                        });
                    }
                }

                // Charger les trajets
                var trajets = _trajetRepository.ObtenirTrajetsParVehicule(idVehicule);
                foreach (var t in trajets)
                {
                    historique.Add(new ElementHistorique
                    {
                        Date = t.DateTrajet,
                        Type = "Trajet",
                        Titre = "Trajet effectué",
                        Description = $"{t.LieuDepart} → {t.LieuArrivee} • {t.DistanceParcourue} km",
                        Kilometrage = t.KilomettrageArrivee,
                        Icone = "🚗",
                        CouleurFond = "#3B82F6"
                    });

                    if (t.KilomettrageArrivee > 0)
                    {
                        pointsGraphique.Add(new PointGraphique
                        {
                            Date = t.DateTrajet,
                            Valeur = t.KilomettrageArrivee,
                            Label = t.DateTrajet.ToString("dd/MM")
                        });
                    }
                }

                // Stocker la liste complète et trier
                _historiqueComplet = historique.OrderByDescending(h => h.Date).ToList();

                // Créer les points du graphique triés par date
                PointsGraphique = new ObservableCollection<PointGraphique>(
                    pointsGraphique.OrderBy(p => p.Date).Take(10)
                );

                // Appliquer le filtre actuel
                AppliquerFiltreHistorique();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement de l'historique : {ex.Message}");
                _historiqueComplet = new List<ElementHistorique>();
                Historique = new ObservableCollection<ElementHistorique>();
                PointsGraphique = new ObservableCollection<PointGraphique>();
            }
        }

        private void ChangerFiltreHistorique(string filtre)
        {
            FiltreHistorique = filtre;
        }

        private void AppliquerFiltreHistorique()
        {
            IEnumerable<ElementHistorique> historiqueFiltré = _historiqueComplet;

            if (FiltreHistorique == "Pleins")
            {
                historiqueFiltré = _historiqueComplet.Where(h => h.Type == "Carburant");
            }
            else if (FiltreHistorique == "Trajets")
            {
                historiqueFiltré = _historiqueComplet.Where(h => h.Type == "Trajet");
            }
            // Si "Tout", on garde tous les éléments

            Historique = new ObservableCollection<ElementHistorique>(historiqueFiltré);
        }
    }
}
