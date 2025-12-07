using FLEET_MANAGER.Models;
using FLEET_MANAGER.Data;
using FLEET_MANAGER.Helpers;
using System.Collections.ObjectModel;
using Logger = FLEET_MANAGER.Helpers.Logger;

namespace FLEET_MANAGER.Repositories
{
    public class VehiculeRepository
    {
        public List<Vehicule> ObtenirTousLesVehicules()
        {
            Logger.Log("DEBUT ObtenirTousLesVehicules");
            var vehicules = new List<Vehicule>();
            string query = "SELECT * FROM vehicules ORDER BY marque, modele";

            try
            {
                using (var reader = DatabaseConnection.ExecuteQuery(query))
                {
                    while (reader.Read())
                    {
                        vehicules.Add(new Vehicule
                        {
                            IdVehicule = reader.IsDBNull(reader.GetOrdinal("id_vehicule")) ? 0 : Convert.ToInt32(reader["id_vehicule"]),
                            Marque = reader.IsDBNull(reader.GetOrdinal("marque")) ? string.Empty : reader["marque"].ToString() ?? string.Empty,
                            Modele = reader.IsDBNull(reader.GetOrdinal("modele")) ? string.Empty : reader["modele"].ToString() ?? string.Empty,
                            Immatriculation = reader.IsDBNull(reader.GetOrdinal("immatriculation")) ? string.Empty : reader["immatriculation"].ToString() ?? string.Empty,
                            AnneeFabrication = reader.IsDBNull(reader.GetOrdinal("annee_fabrication")) ? 0 : Convert.ToInt32(reader["annee_fabrication"]),
                            TypeCarburant = reader.IsDBNull(reader.GetOrdinal("type_carburant")) ? string.Empty : reader["type_carburant"].ToString() ?? string.Empty,
                            KilomettrageInitial = reader.IsDBNull(reader.GetOrdinal("kilometrage_initial")) ? 0 : Convert.ToInt32(reader["kilometrage_initial"]),
                            KilomettrageActuel = reader.IsDBNull(reader.GetOrdinal("kilometrage_actuel")) ? 0 : Convert.ToInt32(reader["kilometrage_actuel"]),
                            DateAcquisition = reader.IsDBNull(reader.GetOrdinal("date_acquisition")) ? DateTime.Now : (DateTime.TryParse(reader["date_acquisition"].ToString(), out var dtAcq) ? dtAcq : DateTime.Now),
                            Etat = reader.IsDBNull(reader.GetOrdinal("etat")) ? "En service" : reader["etat"].ToString() ?? "En service",
                            DateCreation = reader.IsDBNull(reader.GetOrdinal("date_creation")) ? DateTime.Now : (DateTime.TryParse(reader["date_creation"].ToString(), out var dtVeh) ? dtVeh : DateTime.Now),
                            DateModification = reader.IsDBNull(reader.GetOrdinal("date_modification")) ? DateTime.Now : (DateTime.TryParse(reader["date_modification"].ToString(), out var dtMod) ? dtMod : DateTime.Now)
                        });
                    }
                }
                Logger.Log($"ObtenirTousLesVehicules - {vehicules.Count} vehicule(s) trouve(s)");
            }
            catch (Exception ex)
            {
                Logger.LogError("ObtenirTousLesVehicules", ex);
            }

            return vehicules;
        }

        public Vehicule? ObtenirVehiculeParId(int idVehicule)
        {
            string query = "SELECT * FROM vehicules WHERE id_vehicule = @id";
            var parameters = new Dictionary<string, object> { { "@id", idVehicule } };

            try
            {
                using (var reader = DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    if (reader.Read())
                    {
                        return new Vehicule
                        {
                            IdVehicule = Convert.ToInt32(reader["id_vehicule"]),
                            Marque = reader["marque"].ToString() ?? string.Empty,
                            Modele = reader["modele"].ToString() ?? string.Empty,
                            Immatriculation = reader["immatriculation"].ToString() ?? string.Empty,
                            AnneeFabrication = Convert.ToInt32(reader["annee_fabrication"]),
                            TypeCarburant = reader["type_carburant"].ToString() ?? string.Empty,
                            KilomettrageInitial = Convert.ToInt32(reader["kilometrage_initial"]),
                            KilomettrageActuel = Convert.ToInt32(reader["kilometrage_actuel"]),
                            DateAcquisition = DateTime.TryParse(reader["date_acquisition"].ToString(), out var dtAcq2) ? dtAcq2 : DateTime.Now,
                            Etat = reader["etat"].ToString() ?? string.Empty,
                            DateCreation = DateTime.TryParse(reader["date_creation"].ToString(), out var dtTemp) ? dtTemp : DateTime.Now,
                            DateModification = DateTime.TryParse(reader["date_modification"].ToString(), out var dtMod2) ? dtMod2 : DateTime.Now
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la recuperation du vehicule : {ex.Message}");
            }

            return null;
        }

        public bool AjouterVehicule(Vehicule vehicule)
        {
            string query = @"INSERT INTO vehicules
                (marque, modele, immatriculation, annee_fabrication, type_carburant,
                 kilometrage_initial, kilometrage_actuel, date_acquisition, etat)
                VALUES (@marque, @modele, @immat, @annee, @carburant, @km_initial, @km_actuel, @date_acq, @etat)";

            var parameters = new Dictionary<string, object>
            {
                { "@marque", vehicule.Marque },
                { "@modele", vehicule.Modele },
                { "@immat", vehicule.Immatriculation },
                { "@annee", vehicule.AnneeFabrication },
                { "@carburant", vehicule.TypeCarburant },
                { "@km_initial", vehicule.KilomettrageInitial },
                { "@km_actuel", vehicule.KilomettrageActuel },
                { "@date_acq", vehicule.DateAcquisition.ToString("yyyy-MM-dd") },
                { "@etat", vehicule.Etat }
            };

            try
            {
                Logger.Log("DEBUT AjouterVehicule");
                Logger.Log($"Parametres: Marque={vehicule.Marque}, Modele={vehicule.Modele}, Immat={vehicule.Immatriculation}, TypeCarburant={vehicule.TypeCarburant}, Etat={vehicule.Etat}");

                DatabaseConnection.ExecuteCommand(query, parameters);

                Logger.Log("Vehicule ajoute avec SUCCES");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("AjouterVehicule", ex);
                Logger.Log($"Parametres du vehicule - Marque: {vehicule.Marque}, Modele: {vehicule.Modele}, Immat: {vehicule.Immatriculation}, Annee: {vehicule.AnneeFabrication}, TypeCarburant: {vehicule.TypeCarburant}, Etat: {vehicule.Etat}, DateAcq: {vehicule.DateAcquisition}");
                return false;
            }
        }

        public bool ModifierVehicule(Vehicule vehicule)
        {
            string query = @"UPDATE vehicules SET
                marque = @marque, modele = @modele, annee_fabrication = @annee,
                type_carburant = @carburant, kilometrage_actuel = @km_actuel, etat = @etat
                WHERE id_vehicule = @id";

            var parameters = new Dictionary<string, object>
            {
                { "@id", vehicule.IdVehicule },
                { "@marque", vehicule.Marque },
                { "@modele", vehicule.Modele },
                { "@annee", vehicule.AnneeFabrication },
                { "@carburant", vehicule.TypeCarburant },
                { "@km_actuel", vehicule.KilomettrageActuel },
                { "@etat", vehicule.Etat }
            };

            try
            {
                DatabaseConnection.ExecuteCommand(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la modification du vehicule : {ex.Message}");
                return false;
            }
        }

        public bool SupprimerVehicule(int idVehicule)
        {
            string query = "DELETE FROM vehicules WHERE id_vehicule = @id";
            var parameters = new Dictionary<string, object> { { "@id", idVehicule } };

            try
            {
                DatabaseConnection.ExecuteCommand(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la suppression du vehicule : {ex.Message}");
                return false;
            }
        }
    }

    public class CarburantRepository
    {
        public List<Carburant> ObtenirCarburantParVehicule(int idVehicule)
        {
            var carburants = new List<Carburant>();
            string query = "SELECT * FROM carburants WHERE id_vehicule = @id ORDER BY date_saisie DESC";
            var parameters = new Dictionary<string, object> { { "@id", idVehicule } };

            Logger.Log($"DEBUT ObtenirCarburantParVehicule pour vehicule {idVehicule}");
            try
            {
                using (var reader = DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    while (reader.Read())
                    {
                        carburants.Add(new Carburant
                        {
                            IdCarburant = Convert.ToInt32(reader["id_carburant"]),
                            IdVehicule = Convert.ToInt32(reader["id_vehicule"]),
                            DateSaisie = DateTime.TryParse(reader["date_saisie"].ToString(), out var dtSaisie) ? dtSaisie : DateTime.Now,
                            QuantiteLitres = Convert.ToDecimal(Convert.ToDouble(reader["quantite_litres"])),
                            CoutTotal = Convert.ToDecimal(Convert.ToDouble(reader["cout_total"])),
                            CoutParLitre = Convert.ToDecimal(Convert.ToDouble(reader["cout_par_litre"])),
                            Kilometrage = Convert.ToInt32(reader["kilometrage"]),
                            Notes = reader["notes"] != DBNull.Value ? reader["notes"].ToString() : null,
                            IdUtilisateur = Convert.ToInt32(reader["id_utilisateur"]),
                            DateCreation = DateTime.TryParse(reader["date_creation"].ToString(), out var dtTemp) ? dtTemp : DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("ObtenirCarburantParVehicule", ex);
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la recuperation des carburants : {ex.Message}");
            }

            Logger.Log($"ObtenirCarburantParVehicule - {carburants.Count} carburant(s) trouve(s) pour vehicule {idVehicule}");
            return carburants;
        }

        public bool AjouterCarburant(Carburant carburant)
        {
            string query = @"INSERT INTO carburants
                (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilometrage, notes, id_utilisateur)
                VALUES (@id_veh, @date, @heure, @quantite, @cout_total, @cout_litre, @km, @notes, @id_user)";

            var parameters = new Dictionary<string, object>
            {
                { "@id_veh", carburant.IdVehicule },
                { "@date", carburant.DateSaisie.ToString("yyyy-MM-dd") },
                { "@heure", carburant.DateSaisie.ToString("HH:mm:ss") },
                { "@quantite", carburant.QuantiteLitres },
                { "@cout_total", carburant.CoutTotal },
                { "@cout_litre", carburant.CoutParLitre },
                { "@km", carburant.Kilometrage },
                { "@notes", carburant.Notes ?? (object)DBNull.Value },
                { "@id_user", carburant.IdUtilisateur }
            };

            try
            {
                Logger.Log("DEBUT AjouterCarburant");
                Logger.Log($"Parametres: IdVehicule={carburant.IdVehicule}, DateSaisie={carburant.DateSaisie}, QuantiteLitres={carburant.QuantiteLitres}, CoutTotal={carburant.CoutTotal}, Kilometrage={carburant.Kilometrage}");

                DatabaseConnection.ExecuteCommand(query, parameters);

                Logger.Log("Carburant ajoute avec SUCCES");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("AjouterCarburant", ex);
                Logger.Log($"Parametres du carburant - IdVehicule: {carburant.IdVehicule}, DateSaisie: {carburant.DateSaisie}, QuantiteLitres: {carburant.QuantiteLitres}, CoutTotal: {carburant.CoutTotal}, CoutParLitre: {carburant.CoutParLitre}, Kilometrage: {carburant.Kilometrage}, IdUtilisateur: {carburant.IdUtilisateur}");
                return false;
            }
        }
    }

    public class TrajetRepository
    {
        public List<Trajet> ObtenirTrajetsParVehicule(int idVehicule)
        {
            var trajets = new List<Trajet>();
            string query = "SELECT * FROM trajets WHERE id_vehicule = @id ORDER BY date_trajet DESC";
            var parameters = new Dictionary<string, object> { { "@id", idVehicule } };

            Logger.Log($"DEBUT ObtenirTrajetsParVehicule pour vehicule {idVehicule}");
            try
            {
                using (var reader = DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    while (reader.Read())
                    {
                        trajets.Add(new Trajet
                        {
                            IdTrajet = Convert.ToInt32(reader["id_trajet"]),
                            IdVehicule = Convert.ToInt32(reader["id_vehicule"]),
                            DateTrajet = DateTime.TryParse(reader["date_trajet"].ToString(), out var dtTrajet) ? dtTrajet : DateTime.Now,
                            HeureDepart = TimeSpan.TryParse(reader["heure_depart"].ToString(), out var hDepart) ? hDepart : TimeSpan.Zero,
                            HeureArrivee = TimeSpan.TryParse(reader["heure_arrivee"].ToString(), out var hArrivee) ? hArrivee : TimeSpan.Zero,
                            KilomettrageDepart = Convert.ToInt32(reader["kilometrage_depart"]),
                            KilomettrageArrivee = Convert.ToInt32(reader["kilometrage_arrivee"]),
                            DistanceParcourue = Convert.ToInt32(reader["distance_parcourue"]),
                            LieuDepart = reader["lieu_depart"].ToString() ?? string.Empty,
                            LieuArrivee = reader["lieu_arrivee"].ToString() ?? string.Empty,
                            TypeTrajet = reader["type_trajet"].ToString() ?? string.Empty,
                            Notes = reader["notes"] != DBNull.Value ? reader["notes"].ToString() : null,
                            IdUtilisateur = Convert.ToInt32(reader["id_utilisateur"]),
                            DateCreation = DateTime.TryParse(reader["date_creation"].ToString(), out var dtTemp) ? dtTemp : DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("ObtenirTrajetsParVehicule", ex);
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la recuperation des trajets : {ex.Message}");
            }

            Logger.Log($"ObtenirTrajetsParVehicule - {trajets.Count} trajet(s) trouve(s) pour vehicule {idVehicule}");
            return trajets;
        }

        public bool AjouterTrajet(Trajet trajet)
        {
            string query = @"INSERT INTO trajets
                (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilometrage_depart,
                 kilometrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
                VALUES (@id_veh, @date, @h_dep, @h_arr, @km_dep, @km_arr, @distance, @lieu_dep, @lieu_arr, @type, @notes, @id_user)";

            var parameters = new Dictionary<string, object>
            {
                { "@id_veh", trajet.IdVehicule },
                { "@date", trajet.DateTrajet.ToString("yyyy-MM-dd") },
                { "@h_dep", trajet.HeureDepart.ToString(@"hh\:mm\:ss") },
                { "@h_arr", trajet.HeureArrivee.ToString(@"hh\:mm\:ss") },
                { "@km_dep", trajet.KilomettrageDepart },
                { "@km_arr", trajet.KilomettrageArrivee },
                { "@distance", trajet.DistanceParcourue },
                { "@lieu_dep", trajet.LieuDepart },
                { "@lieu_arr", trajet.LieuArrivee },
                { "@type", trajet.TypeTrajet },
                { "@notes", trajet.Notes ?? (object)DBNull.Value },
                { "@id_user", trajet.IdUtilisateur }
            };

            try
            {
                Logger.Log("DEBUT AjouterTrajet");
                Logger.Log($"Parametres: IdVehicule={trajet.IdVehicule}, DateTrajet={trajet.DateTrajet}, HeureDepart={trajet.HeureDepart}, HeureArrivee={trajet.HeureArrivee}, TypeTrajet={trajet.TypeTrajet}");

                DatabaseConnection.ExecuteCommand(query, parameters);

                Logger.Log("Trajet ajoute avec SUCCES");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("AjouterTrajet", ex);
                Logger.Log($"Parametres du trajet - IdVehicule: {trajet.IdVehicule}, DateTrajet: {trajet.DateTrajet}, HeureDepart: {trajet.HeureDepart}, HeureArrivee: {trajet.HeureArrivee}, KmDepart: {trajet.KilomettrageDepart}, KmArrivee: {trajet.KilomettrageArrivee}, TypeTrajet: {trajet.TypeTrajet}, IdUtilisateur: {trajet.IdUtilisateur}");
                return false;
            }
        }
    }

    /// <summary>
    /// Repository pour la gestion des utilisateurs
    /// Les mots de passe sont hashés avec BCrypt avant stockage
    /// </summary>
    public class UtilisateurRepository
    {
        public List<Utilisateur> ObtenirTousLesUtilisateurs()
        {
            var utilisateurs = new List<Utilisateur>();
            string query = "SELECT * FROM utilisateurs ORDER BY nom, prenom";

            try
            {
                using (var reader = DatabaseConnection.ExecuteQuery(query))
                {
                    while (reader.Read())
                    {
                        utilisateurs.Add(new Utilisateur
                        {
                            IdUtilisateur = Convert.ToInt32(reader["id_utilisateur"]),
                            NomUtilisateur = reader["nom_utilisateur"].ToString() ?? string.Empty,
                            Email = reader["email"].ToString() ?? string.Empty,
                            MotDePasse = reader["mot_de_passe"].ToString() ?? string.Empty,
                            Nom = reader["nom"].ToString() ?? string.Empty,
                            Prenom = reader["prenom"].ToString() ?? string.Empty,
                            Role = reader["role"].ToString() ?? "utilisateur",
                            DateCreation = DateTime.TryParse(reader["date_creation"].ToString(), out var dt1) ? dt1 : DateTime.Now,
                            Actif = Convert.ToInt32(reader["actif"]) == 1
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la recuperation des utilisateurs : {ex.Message}");
            }

            return utilisateurs;
        }

        public Utilisateur? ObtenirUtilisateurParId(int idUtilisateur)
        {
            string query = "SELECT * FROM utilisateurs WHERE id_utilisateur = @id";
            var parameters = new Dictionary<string, object> { { "@id", idUtilisateur } };

            try
            {
                using (var reader = DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    if (reader.Read())
                    {
                        return new Utilisateur
                        {
                            IdUtilisateur = Convert.ToInt32(reader["id_utilisateur"]),
                            NomUtilisateur = reader["nom_utilisateur"].ToString() ?? string.Empty,
                            Email = reader["email"].ToString() ?? string.Empty,
                            MotDePasse = reader["mot_de_passe"].ToString() ?? string.Empty,
                            Nom = reader["nom"].ToString() ?? string.Empty,
                            Prenom = reader["prenom"].ToString() ?? string.Empty,
                            Role = reader["role"].ToString() ?? string.Empty,
                            DateCreation = DateTime.TryParse(reader["date_creation"].ToString(), out var dt2) ? dt2 : DateTime.Now,
                            Actif = Convert.ToInt32(reader["actif"]) == 1
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la recuperation de l'utilisateur : {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Ajoute un nouvel utilisateur avec mot de passe hashé BCrypt
        /// </summary>
        public bool AjouterUtilisateur(Utilisateur utilisateur)
        {
            // V�rifier que le mot de passe n'est pas vide
            if (string.IsNullOrWhiteSpace(utilisateur.MotDePasse))
            {
                System.Diagnostics.Debug.WriteLine("Erreur : Le mot de passe est vide");
                return false;
            }

            // Vérifier que le rôle est valide
            if (string.IsNullOrWhiteSpace(utilisateur.Role))
            {
                utilisateur.Role = "utilisateur";
            }

            // Hasher le mot de passe avec BCrypt avant de le stocker
            string motDePasseHashe = PasswordHelper.HasherMotDePasse(utilisateur.MotDePasse);

            string query = @"INSERT INTO utilisateurs 
                (nom_utilisateur, email, mot_de_passe, nom, prenom, role, actif)
                VALUES (@nom_user, @email, @pwd, @nom, @prenom, @role, @actif)";

            var parameters = new Dictionary<string, object>
            {
                { "@nom_user", utilisateur.NomUtilisateur },
                { "@email", utilisateur.Email },
                { "@pwd", motDePasseHashe },
                { "@nom", utilisateur.Nom },
                { "@prenom", utilisateur.Prenom },
                { "@role", utilisateur.Role },
                { "@actif", utilisateur.Actif }
            };

            try
            {
                DatabaseConnection.ExecuteCommand(query, parameters);
                System.Diagnostics.Debug.WriteLine($"Utilisateur {utilisateur.NomUtilisateur} ajoute avec succes (role: {utilisateur.Role})");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de l'ajout de l'utilisateur : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Modifie un utilisateur existant
        /// Si le mot de passe est fourni et n'est pas déjà hashé, il sera hashé
        /// </summary>
        public bool ModifierUtilisateur(Utilisateur utilisateur)
        {
            try
            {
                string query;
                Dictionary<string, object> parameters;

                // Si un nouveau mot de passe est fourni et n'est pas déjà un hash BCrypt
                if (!string.IsNullOrWhiteSpace(utilisateur.MotDePasse) &&
                    !utilisateur.MotDePasse.StartsWith("$2"))
                {
                    string motDePasseHashe = PasswordHelper.HasherMotDePasse(utilisateur.MotDePasse);

                    query = @"UPDATE utilisateurs SET 
                        nom_utilisateur = @nom_user, 
                        email = @email, 
                        mot_de_passe = @pwd, 
                        nom = @nom, 
                        prenom = @prenom, 
                        role = @role
                        WHERE id_utilisateur = @id";

                    parameters = new Dictionary<string, object>
                    {
                        { "@id", utilisateur.IdUtilisateur },
                        { "@nom_user", utilisateur.NomUtilisateur },
                        { "@email", utilisateur.Email },
                        { "@pwd", motDePasseHashe },
                        { "@nom", utilisateur.Nom },
                        { "@prenom", utilisateur.Prenom },
                        { "@role", utilisateur.Role }
                    };
                }
                else
                {
                    // Pas de nouveau mot de passe
                    query = @"UPDATE utilisateurs SET 
                        nom_utilisateur = @nom_user, 
                        email = @email, 
                        nom = @nom, 
                        prenom = @prenom, 
                        role = @role
                        WHERE id_utilisateur = @id";

                    parameters = new Dictionary<string, object>
                    {
                        { "@id", utilisateur.IdUtilisateur },
                        { "@nom_user", utilisateur.NomUtilisateur },
                        { "@email", utilisateur.Email },
                        { "@nom", utilisateur.Nom },
                        { "@prenom", utilisateur.Prenom },
                        { "@role", utilisateur.Role }
                    };
                }

                DatabaseConnection.ExecuteCommand(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la modification de l'utilisateur : {ex.Message}");
                return false;
            }
        }

        public bool SupprimerUtilisateur(int idUtilisateur)
        {
            string query = "DELETE FROM utilisateurs WHERE id_utilisateur = @id";
            var parameters = new Dictionary<string, object> { { "@id", idUtilisateur } };

            try
            {
                DatabaseConnection.ExecuteCommand(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la suppression de l'utilisateur : {ex.Message}");
                return false;
            }
        }
    }
}
