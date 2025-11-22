using FLEET_MANAGER.Models;
using FLEET_MANAGER.Data;
using FLEET_MANAGER.Helpers;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace FLEET_MANAGER.Repositories
{
    public class VehiculeRepository
    {
        public List<Vehicule> ObtenirTousLesVehicules()
        {
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
                            IdVehicule = reader.IsDBNull(reader.GetOrdinal("id_vehicule")) ? 0 : (int)reader["id_vehicule"],
                            Marque = reader.IsDBNull(reader.GetOrdinal("marque")) ? string.Empty : reader["marque"].ToString() ?? string.Empty,
                            Modele = reader.IsDBNull(reader.GetOrdinal("modele")) ? string.Empty : reader["modele"].ToString() ?? string.Empty,
                            Immatriculation = reader.IsDBNull(reader.GetOrdinal("immatriculation")) ? string.Empty : reader["immatriculation"].ToString() ?? string.Empty,
                            AnneeFabrication = reader.IsDBNull(reader.GetOrdinal("annee_fabrication")) ? 0 : (int)reader["annee_fabrication"],
                            TypeCarburant = reader.IsDBNull(reader.GetOrdinal("type_carburant")) ? string.Empty : reader["type_carburant"].ToString() ?? string.Empty,
                            KilomettrageInitial = reader.IsDBNull(reader.GetOrdinal("kilométrage_initial")) ? 0 : (int)reader["kilométrage_initial"],
                            KilomettrageActuel = reader.IsDBNull(reader.GetOrdinal("kilométrage_actuel")) ? 0 : (int)reader["kilométrage_actuel"],
                            DateAcquisition = reader.IsDBNull(reader.GetOrdinal("date_acquisition")) ? DateTime.Now : (DateTime)reader["date_acquisition"],
                            Etat = reader.IsDBNull(reader.GetOrdinal("etat")) ? "En service" : reader["etat"].ToString() ?? "En service",
                            DateCreation = reader.IsDBNull(reader.GetOrdinal("date_creation")) ? DateTime.Now : (DateTime)reader["date_creation"],
                            DateModification = reader.IsDBNull(reader.GetOrdinal("date_modification")) ? DateTime.Now : (DateTime)reader["date_modification"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la recuperation des vehicules : {ex.Message}");
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
                            IdVehicule = (int)reader["id_vehicule"],
                            Marque = reader["marque"].ToString() ?? string.Empty,
                            Modele = reader["modele"].ToString() ?? string.Empty,
                            Immatriculation = reader["immatriculation"].ToString() ?? string.Empty,
                            AnneeFabrication = (int)reader["annee_fabrication"],
                            TypeCarburant = reader["type_carburant"].ToString() ?? string.Empty,
                            KilomettrageInitial = (int)reader["kilométrage_initial"],
                            KilomettrageActuel = (int)reader["kilométrage_actuel"],
                            DateAcquisition = (DateTime)reader["date_acquisition"],
                            Etat = reader["etat"].ToString() ?? string.Empty,
                            DateCreation = (DateTime)reader["date_creation"],
                            DateModification = (DateTime)reader["date_modification"]
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
                 kilométrage_initial, kilométrage_actuel, date_acquisition, etat)
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
                { "@date_acq", vehicule.DateAcquisition },
                { "@etat", vehicule.Etat }
            };

            try
            {
                DatabaseConnection.ExecuteCommand(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de l'ajout du vehicule : {ex.Message}");
                return false;
            }
        }

        public bool ModifierVehicule(Vehicule vehicule)
        {
            string query = @"UPDATE vehicules SET 
                marque = @marque, modele = @modele, annee_fabrication = @annee, 
                type_carburant = @carburant, kilométrage_actuel = @km_actuel, etat = @etat
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

            try
            {
                using (var reader = DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    while (reader.Read())
                    {
                        carburants.Add(new Carburant
                        {
                            IdCarburant = (int)reader["id_carburant"],
                            IdVehicule = (int)reader["id_vehicule"],
                            DateSaisie = (DateTime)reader["date_saisie"],
                            QuantiteLitres = (decimal)reader["quantite_litres"],
                            CoutTotal = (decimal)reader["cout_total"],
                            CoutParLitre = (decimal)reader["cout_par_litre"],
                            Kilometrage = (int)reader["kilométrage"],
                            Notes = reader["notes"] != DBNull.Value ? reader["notes"].ToString() : null,
                            IdUtilisateur = (int)reader["id_utilisateur"],
                            DateCreation = (DateTime)reader["date_creation"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la recuperation des carburants : {ex.Message}");
            }

            return carburants;
        }

        public bool AjouterCarburant(Carburant carburant)
        {
            string query = @"INSERT INTO carburants 
                (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
                VALUES (@id_veh, @date, @heure, @quantite, @cout_total, @cout_litre, @km, @notes, @id_user)";

            var parameters = new Dictionary<string, object>
            {
                { "@id_veh", carburant.IdVehicule },
                { "@date", carburant.DateSaisie.Date },
                { "@heure", carburant.DateSaisie.TimeOfDay },
                { "@quantite", carburant.QuantiteLitres },
                { "@cout_total", carburant.CoutTotal },
                { "@cout_litre", carburant.CoutParLitre },
                { "@km", carburant.Kilometrage },
                { "@notes", carburant.Notes ?? (object)DBNull.Value },
                { "@id_user", carburant.IdUtilisateur }
            };

            try
            {
                DatabaseConnection.ExecuteCommand(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de l'ajout du carburant : {ex.Message}");
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

            try
            {
                using (var reader = DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    while (reader.Read())
                    {
                        trajets.Add(new Trajet
                        {
                            IdTrajet = (int)reader["id_trajet"],
                            IdVehicule = (int)reader["id_vehicule"],
                            DateTrajet = (DateTime)reader["date_trajet"],
                            HeureDepart = (TimeSpan)reader["heure_depart"],
                            HeureArrivee = (TimeSpan)reader["heure_arrivee"],
                            KilomettrageDepart = (int)reader["kilométrage_depart"],
                            KilomettrageArrivee = (int)reader["kilométrage_arrivee"],
                            DistanceParcourue = (int)reader["distance_parcourue"],
                            LieuDepart = reader["lieu_depart"].ToString() ?? string.Empty,
                            LieuArrivee = reader["lieu_arrivee"].ToString() ?? string.Empty,
                            TypeTrajet = reader["type_trajet"].ToString() ?? string.Empty,
                            Notes = reader["notes"] != DBNull.Value ? reader["notes"].ToString() : null,
                            IdUtilisateur = (int)reader["id_utilisateur"],
                            DateCreation = (DateTime)reader["date_creation"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la recuperation des trajets : {ex.Message}");
            }

            return trajets;
        }

        public bool AjouterTrajet(Trajet trajet)
        {
            string query = @"INSERT INTO trajets 
                (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, 
                 kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
                VALUES (@id_veh, @date, @h_dep, @h_arr, @km_dep, @km_arr, @distance, @lieu_dep, @lieu_arr, @type, @notes, @id_user)";

            var parameters = new Dictionary<string, object>
            {
                { "@id_veh", trajet.IdVehicule },
                { "@date", trajet.DateTrajet },
                { "@h_dep", trajet.HeureDepart },
                { "@h_arr", trajet.HeureArrivee },
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
                DatabaseConnection.ExecuteCommand(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de l'ajout du trajet : {ex.Message}");
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
                            IdUtilisateur = (int)reader["id_utilisateur"],
                            NomUtilisateur = reader["nom_utilisateur"].ToString() ?? string.Empty,
                            Email = reader["email"].ToString() ?? string.Empty,
                            MotDePasse = reader["mot_de_passe"].ToString() ?? string.Empty,
                            Nom = reader["nom"].ToString() ?? string.Empty,
                            Prenom = reader["prenom"].ToString() ?? string.Empty,
                            Role = reader["role"].ToString() ?? "utilisateur",
                            DateCreation = (DateTime)reader["date_creation"],
                            Actif = (bool)reader["actif"]
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
                            IdUtilisateur = (int)reader["id_utilisateur"],
                            NomUtilisateur = reader["nom_utilisateur"].ToString() ?? string.Empty,
                            Email = reader["email"].ToString() ?? string.Empty,
                            MotDePasse = reader["mot_de_passe"].ToString() ?? string.Empty,
                            Nom = reader["nom"].ToString() ?? string.Empty,
                            Prenom = reader["prenom"].ToString() ?? string.Empty,
                            Role = reader["role"].ToString() ?? string.Empty,
                            DateCreation = (DateTime)reader["date_creation"],
                            Actif = (bool)reader["actif"]
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
            // Vérifier que le mot de passe n'est pas vide
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
