using System.Collections.ObjectModel;
using FLEET_MANAGER.Models;
using FLEET_MANAGER.Repositories;
using FLEET_MANAGER.Data;

namespace FLEET_MANAGER.ViewModels
{
    /// <summary>
    /// ViewModel pour la page de statistiques avec indicateurs de performance
    /// </summary>
    public class StatistiquesViewModel : ViewModelBase
    {
        #region Propriétés - Vue d'ensemble

        private int _totalVehicules;
        private int _totalTrajets;
        private decimal _kilometrageTotalParcouru;
        private decimal _coutTotalGlobal;

        public int TotalVehicules
        {
            get => _totalVehicules;
            set => SetProperty(ref _totalVehicules, value, nameof(TotalVehicules));
        }

        public int TotalTrajets
        {
            get => _totalTrajets;
            set => SetProperty(ref _totalTrajets, value, nameof(TotalTrajets));
        }

        public decimal KilometrageTotalParcouru
        {
            get => _kilometrageTotalParcouru;
            set => SetProperty(ref _kilometrageTotalParcouru, value, nameof(KilometrageTotalParcouru));
        }

        public decimal CoutTotalGlobal
        {
            get => _coutTotalGlobal;
            set => SetProperty(ref _coutTotalGlobal, value, nameof(CoutTotalGlobal));
        }

        #endregion

        #region Propriétés - Coûts

        private decimal _coutTotalCarburant;
        private decimal _coutMoyenParVehicule;
        private decimal _coutParKilometre;
        private decimal _coutMoisActuel;

        public decimal CoutTotalCarburant
        {
            get => _coutTotalCarburant;
            set => SetProperty(ref _coutTotalCarburant, value, nameof(CoutTotalCarburant));
        }

        public decimal CoutMoyenParVehicule
        {
            get => _coutMoyenParVehicule;
            set => SetProperty(ref _coutMoyenParVehicule, value, nameof(CoutMoyenParVehicule));
        }

        public decimal CoutParKilometre
        {
            get => _coutParKilometre;
            set => SetProperty(ref _coutParKilometre, value, nameof(CoutParKilometre));
        }

        public decimal CoutMoisActuel
        {
            get => _coutMoisActuel;
            set => SetProperty(ref _coutMoisActuel, value, nameof(CoutMoisActuel));
        }

        #endregion

        #region Propriétés - Consommation

        private decimal _consommationMoyenne;
        private decimal _litresMoisActuel;
        private decimal _litresTotal;

        public decimal ConsommationMoyenne
        {
            get => _consommationMoyenne;
            set => SetProperty(ref _consommationMoyenne, value, nameof(ConsommationMoyenne));
        }

        public decimal LitresMoisActuel
        {
            get => _litresMoisActuel;
            set => SetProperty(ref _litresMoisActuel, value, nameof(LitresMoisActuel));
        }

        public decimal LitresTotal
        {
            get => _litresTotal;
            set => SetProperty(ref _litresTotal, value, nameof(LitresTotal));
        }

        #endregion

        #region Propriétés - Utilisation

        private decimal _distanceMoyenneParVehicule;
        private int _trajetsMoisActuel;
        private decimal _distanceMoisActuel;

        public decimal DistanceMoyenneParVehicule
        {
            get => _distanceMoyenneParVehicule;
            set => SetProperty(ref _distanceMoyenneParVehicule, value, nameof(DistanceMoyenneParVehicule));
        }

        public int TrajetsMoisActuel
        {
            get => _trajetsMoisActuel;
            set => SetProperty(ref _trajetsMoisActuel, value, nameof(TrajetsMoisActuel));
        }

        public decimal DistanceMoisActuel
        {
            get => _distanceMoisActuel;
            set => SetProperty(ref _distanceMoisActuel, value, nameof(DistanceMoisActuel));
        }

        #endregion

        #region Propriétés - Top véhicules

        private ObservableCollection<VehiculeStatistique> _topVehiculesEconomiques;
        private ObservableCollection<VehiculeStatistique> _topVehiculesGourmands;
        private ObservableCollection<VehiculeStatistique> _topVehiculesPlusUtilises;

        public ObservableCollection<VehiculeStatistique> TopVehiculesEconomiques
        {
            get => _topVehiculesEconomiques;
            set => SetProperty(ref _topVehiculesEconomiques, value, nameof(TopVehiculesEconomiques));
        }

        public ObservableCollection<VehiculeStatistique> TopVehiculesGourmands
        {
            get => _topVehiculesGourmands;
            set => SetProperty(ref _topVehiculesGourmands, value, nameof(TopVehiculesGourmands));
        }

        public ObservableCollection<VehiculeStatistique> TopVehiculesPlusUtilises
        {
            get => _topVehiculesPlusUtilises;
            set => SetProperty(ref _topVehiculesPlusUtilises, value, nameof(TopVehiculesPlusUtilises));
        }

        #endregion

        #region Propriétés - Répartition

        private ObservableCollection<RepartitionStatistique> _repartitionTypesTrajet;
        private ObservableCollection<RepartitionStatistique> _repartitionTypesCarburant;

        public ObservableCollection<RepartitionStatistique> RepartitionTypesTrajet
        {
            get => _repartitionTypesTrajet;
            set => SetProperty(ref _repartitionTypesTrajet, value, nameof(RepartitionTypesTrajet));
        }

        public ObservableCollection<RepartitionStatistique> RepartitionTypesCarburant
        {
            get => _repartitionTypesCarburant;
            set => SetProperty(ref _repartitionTypesCarburant, value, nameof(RepartitionTypesCarburant));
        }

        #endregion

        private VehiculeRepository _vehiculeRepository;
        private CarburantRepository _carburantRepository;
        private TrajetRepository _trajetRepository;

        public StatistiquesViewModel()
        {
            _vehiculeRepository = new VehiculeRepository();
            _carburantRepository = new CarburantRepository();
            _trajetRepository = new TrajetRepository();

            _topVehiculesEconomiques = new ObservableCollection<VehiculeStatistique>();
            _topVehiculesGourmands = new ObservableCollection<VehiculeStatistique>();
            _topVehiculesPlusUtilises = new ObservableCollection<VehiculeStatistique>();
            _repartitionTypesTrajet = new ObservableCollection<RepartitionStatistique>();
            _repartitionTypesCarburant = new ObservableCollection<RepartitionStatistique>();

            ChargerStatistiques();
        }

        public void ChargerStatistiques()
        {
            try
            {
                var vehicules = _vehiculeRepository.ObtenirTousLesVehicules();
                TotalVehicules = vehicules.Count;

                if (vehicules.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Aucun véhicule trouvé dans la base de données");
                    return;
                }

                // Récupérer toutes les données
                var tousLesCarburants = new List<Carburant>();
                var tousLesTrajets = new List<Trajet>();
                var statsParVehicule = new List<VehiculeStatistique>();

                foreach (var vehicule in vehicules)
                {
                    var carburants = _carburantRepository.ObtenirCarburantParVehicule(vehicule.IdVehicule);
                    var trajets = _trajetRepository.ObtenirTrajetsParVehicule(vehicule.IdVehicule);

                    tousLesCarburants.AddRange(carburants);
                    tousLesTrajets.AddRange(trajets);

                    // Calculer les stats par véhicule
                    var coutTotal = carburants.Sum(c => c.CoutTotal);
                    var litresTotal = carburants.Sum(c => c.QuantiteLitres);
                    var distanceTotal = trajets.Sum(t => t.DistanceParcourue);
                    var nombreTrajets = trajets.Count;

                    var consommation = distanceTotal > 0 && litresTotal > 0
                        ? (litresTotal / distanceTotal) * 100
                        : 0;

                    statsParVehicule.Add(new VehiculeStatistique
                    {
                        Nom = $"{vehicule.Marque} {vehicule.Modele}",
                        Immatriculation = vehicule.Immatriculation,
                        CoutTotal = coutTotal,
                        ConsommationMoyenne = consommation,
                        DistanceTotale = distanceTotal,
                        NombreTrajets = nombreTrajets
                    });
                }

                TotalTrajets = tousLesTrajets.Count;

                // Statistiques globales
                CalculerStatistiquesGlobales(tousLesCarburants, tousLesTrajets, vehicules.Count);

                // Statistiques de coûts
                CalculerStatistiquesCouts(tousLesCarburants, tousLesTrajets, vehicules.Count);

                // Statistiques de consommation
                CalculerStatistiquesConsommation(tousLesCarburants, tousLesTrajets);

                // Statistiques d'utilisation
                CalculerStatistiquesUtilisation(tousLesTrajets, vehicules.Count);

                // Top véhicules
                CalculerTopVehicules(statsParVehicule);

                // Répartitions
                CalculerRepartitions(tousLesTrajets, vehicules);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des statistiques : {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace : {ex.StackTrace}");
                System.Windows.MessageBox.Show($"Erreur lors du chargement des statistiques :\n{ex.Message}", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void CalculerStatistiquesGlobales(List<Carburant> carburants, List<Trajet> trajets, int nbVehicules)
        {
            KilometrageTotalParcouru = trajets.Sum(t => t.DistanceParcourue);
            CoutTotalGlobal = carburants.Sum(c => c.CoutTotal);
        }

        private void CalculerStatistiquesCouts(List<Carburant> carburants, List<Trajet> trajets, int nbVehicules)
        {
            CoutTotalCarburant = carburants.Sum(c => c.CoutTotal);
            CoutMoyenParVehicule = nbVehicules > 0 ? CoutTotalCarburant / nbVehicules : 0;

            var kmTotal = trajets.Sum(t => t.DistanceParcourue);
            CoutParKilometre = kmTotal > 0 ? CoutTotalCarburant / kmTotal : 0;

            var moisActuel = DateTime.Now.Month;
            var anneeActuelle = DateTime.Now.Year;
            CoutMoisActuel = carburants
                .Where(c => c.DateSaisie.Month == moisActuel && c.DateSaisie.Year == anneeActuelle)
                .Sum(c => c.CoutTotal);
        }

        private void CalculerStatistiquesConsommation(List<Carburant> carburants, List<Trajet> trajets)
        {
            LitresTotal = carburants.Sum(c => c.QuantiteLitres);

            var kmTotal = trajets.Sum(t => t.DistanceParcourue);
            ConsommationMoyenne = kmTotal > 0 && LitresTotal > 0 ? (LitresTotal / kmTotal) * 100 : 0;

            var moisActuel = DateTime.Now.Month;
            var anneeActuelle = DateTime.Now.Year;
            LitresMoisActuel = carburants
                .Where(c => c.DateSaisie.Month == moisActuel && c.DateSaisie.Year == anneeActuelle)
                .Sum(c => c.QuantiteLitres);
        }

        private void CalculerStatistiquesUtilisation(List<Trajet> trajets, int nbVehicules)
        {
            var kmTotal = trajets.Sum(t => t.DistanceParcourue);
            DistanceMoyenneParVehicule = nbVehicules > 0 ? kmTotal / nbVehicules : 0;

            var moisActuel = DateTime.Now.Month;
            var anneeActuelle = DateTime.Now.Year;
            var trajetsMois = trajets.Where(t => t.DateTrajet.Month == moisActuel && t.DateTrajet.Year == anneeActuelle).ToList();

            TrajetsMoisActuel = trajetsMois.Count;
            DistanceMoisActuel = trajetsMois.Sum(t => t.DistanceParcourue);
        }

        private void CalculerTopVehicules(List<VehiculeStatistique> stats)
        {
            // Top 5 véhicules économiques (consommation la plus faible)
            TopVehiculesEconomiques = new ObservableCollection<VehiculeStatistique>(
                stats.Where(s => s.ConsommationMoyenne > 0)
                     .OrderBy(s => s.ConsommationMoyenne)
                     .Take(5)
            );

            // Top 5 véhicules gourmands (consommation la plus élevée)
            TopVehiculesGourmands = new ObservableCollection<VehiculeStatistique>(
                stats.Where(s => s.ConsommationMoyenne > 0)
                     .OrderByDescending(s => s.ConsommationMoyenne)
                     .Take(5)
            );

            // Top 5 véhicules les plus utilisés (distance totale)
            TopVehiculesPlusUtilises = new ObservableCollection<VehiculeStatistique>(
                stats.Where(s => s.DistanceTotale > 0)
                     .OrderByDescending(s => s.DistanceTotale)
                     .Take(5)
            );
        }

        private void CalculerRepartitions(List<Trajet> trajets, List<Vehicule> vehicules)
        {
            // Répartition par type de trajet
            var repartitionTrajets = trajets
                .GroupBy(t => t.TypeTrajet)
                .Select(g => new RepartitionStatistique
                {
                    Categorie = g.Key,
                    Valeur = g.Count(),
                    Pourcentage = trajets.Count > 0 ? (decimal)g.Count() / trajets.Count * 100 : 0
                })
                .OrderByDescending(r => r.Valeur);

            RepartitionTypesTrajet = new ObservableCollection<RepartitionStatistique>(repartitionTrajets);

            // Répartition par type de carburant
            var repartitionCarburant = vehicules
                .GroupBy(v => v.TypeCarburant)
                .Select(g => new RepartitionStatistique
                {
                    Categorie = g.Key,
                    Valeur = g.Count(),
                    Pourcentage = vehicules.Count > 0 ? (decimal)g.Count() / vehicules.Count * 100 : 0
                })
                .OrderByDescending(r => r.Valeur);

            RepartitionTypesCarburant = new ObservableCollection<RepartitionStatistique>(repartitionCarburant);
        }

        public void Rafraichir()
        {
            ChargerStatistiques();
        }
    }

    /// <summary>
    /// Modèle pour les statistiques par véhicule
    /// </summary>
    public class VehiculeStatistique
    {
        public string Nom { get; set; } = string.Empty;
        public string Immatriculation { get; set; } = string.Empty;
        public decimal CoutTotal { get; set; }
        public decimal ConsommationMoyenne { get; set; }
        public decimal DistanceTotale { get; set; }
        public int NombreTrajets { get; set; }

        public string ConsommationFormatee => $"{ConsommationMoyenne:F2} L/100km";
        public string CoutFormate => $"{CoutTotal:C}";
        public string DistanceFormatee => $"{DistanceTotale:N0} km";
    }

    /// <summary>
    /// Modèle pour les statistiques de répartition
    /// </summary>
    public class RepartitionStatistique
    {
        public string Categorie { get; set; } = string.Empty;
        public int Valeur { get; set; }
        public decimal Pourcentage { get; set; }

        public string PourcentageFormate => $"{Pourcentage:F1}%";
    }
}
