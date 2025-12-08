namespace FLEET_MANAGER.Models
{
    /// <summary>
    /// Modéle représentant un utilisateur
    /// </summary>
    public class Utilisateur
    {
        public int IdUtilisateur { get; set; }
        public string NomUtilisateur { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Role { get; set; } = "utilisateur"; // "administrateur" ou "utilisateur"
        public DateTime DateCreation { get; set; }
        public bool Actif { get; set; } = true;

        public string NomComplet => $"{Prenom} {Nom}";
    }

    /// <summary>
    /// Modéle représentant un véhicule
    /// </summary>
    public class Vehicule
    {
        public int IdVehicule { get; set; }
        public string Marque { get; set; } = string.Empty;
        public string Modele { get; set; } = string.Empty;
        public string Immatriculation { get; set; } = string.Empty;
        public int AnneeFabrication { get; set; }
        public string TypeCarburant { get; set; } = string.Empty; // Essence, Diesel, Hybride, électrique
        public int KilomettrageInitial { get; set; }
        public int KilomettrageActuel { get; set; }
        public DateTime DateAcquisition { get; set; }
        public string Etat { get; set; } = "En service"; // En service, En maintenance, Retiré du service
        public DateTime DateCreation { get; set; }
        public DateTime DateModification { get; set; }

        // Capacités énergétiques
        public decimal? CapaciteReservoir { get; set; } // Capacité du réservoir en litres (pour Essence, Diesel, Hybride)
        public decimal? CapaciteBatterie { get; set; } // Capacité de la batterie en kWh (pour Électrique, Hybride)

        public string DescriptionComplete => $"{Marque} {Modele} ({Immatriculation})";
    }

    /// <summary>
    /// Modéle représentant un ravitaillement en carburant
    /// </summary>
    public class Carburant
    {
        public int IdCarburant { get; set; }
        public int IdVehicule { get; set; }
        public DateTime DateSaisie { get; set; }
        public decimal QuantiteLitres { get; set; }
        public decimal CoutTotal { get; set; }
        public decimal CoutParLitre { get; set; }
        public int Kilometrage { get; set; }
        public string? Notes { get; set; }
        public int IdUtilisateur { get; set; }
        public DateTime DateCreation { get; set; }

        public decimal ConsommationMoyenne { get; set; } // Calculee a partir de donnees historiques
    }

    /// <summary>
    /// Modéle représentant un trajet
    /// </summary>
    public class Trajet
    {
        public int IdTrajet { get; set; }
        public int IdVehicule { get; set; }
        public DateTime DateTrajet { get; set; }
        public TimeSpan HeureDepart { get; set; }
        public TimeSpan HeureArrivee { get; set; }
        public int KilomettrageDepart { get; set; }
        public int KilomettrageArrivee { get; set; }
        public int DistanceParcourue { get; set; }
        public string LieuDepart { get; set; } = string.Empty;
        public string LieuArrivee { get; set; } = string.Empty;
        public string TypeTrajet { get; set; } = "Professionnel"; // Personnel, Professionnel, Service
        public string? Notes { get; set; }
        public int IdUtilisateur { get; set; }
        public DateTime DateCreation { get; set; }
    }

    /// <summary>
    /// Modéle représentant une maintenance
    /// </summary>
    public class Maintenance
    {
        public int IdMaintenance { get; set; }
        public int IdVehicule { get; set; }
        public DateTime DateMaintenance { get; set; }
        public string TypeMaintenance { get; set; } = string.Empty; // Revision, Reparation, etc.
        public string Description { get; set; } = string.Empty;
        public decimal CoutMaintenance { get; set; }
        public string? Fournisseur { get; set; }
        public int Kilometrage { get; set; }
        public DateTime? DateProchaineMaintenance { get; set; }
        public int IdUtilisateur { get; set; }
        public DateTime DateCreation { get; set; }
    }

    /// <summary>
    /// Modéle représentant un rapport
    /// </summary>
    public class Rapport
    {
        public int IdRapport { get; set; }
        public int? IdVehicule { get; set; }
        public string TypeRapport { get; set; } = string.Empty; // Consommation, Coùts, Utilisation, etc.
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? DonneesJson { get; set; }
        public DateTime DateGeneration { get; set; }
        public int IdUtilisateur { get; set; }
    }

    /// <summary>
    /// Modele representant un element d'historique pour un vehicule
    /// </summary>
    public class ElementHistorique
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty; // "Carburant", "Trajet", "Maintenance"
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Kilometrage { get; set; }
        public string Icone { get; set; } = string.Empty; // Pour afficher l'icone appropriee
        public string CouleurFond { get; set; } = string.Empty; // Pour la couleur de fond de l'icone
    }

    /// <summary>
    /// Modele representant un point sur un graphique
    /// </summary>
    public class PointGraphique
    {
        public DateTime Date { get; set; }
        public int Valeur { get; set; }
        public string Label { get; set; } = string.Empty;
    }
}
