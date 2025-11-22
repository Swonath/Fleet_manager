using System.Collections.ObjectModel;
using System.Windows.Input;
using FLEET_MANAGER.Models;
using FLEET_MANAGER.Repositories;

namespace FLEET_MANAGER.ViewModels
{
    /// <summary>
    /// ViewModel pour la gestion des utilisateurs
    /// Gère les permissions selon le rôle de l'utilisateur connecté
    /// </summary>
    public class UtilisateurViewModel : ViewModelBase
    {
        private ObservableCollection<Utilisateur> _utilisateurs;
        private Utilisateur? _utilisateurSelectionne;
        private bool _estEnEdition = false;
        private string _messageErreur = string.Empty;
        private bool _isLoading = false;
        private bool _estNouveauUtilisateur = false;
        private Utilisateur? _utilisateurConnecte;

        private string _nomUtilisateur = string.Empty;
        private string _email = string.Empty;
        private string _nom = string.Empty;
        private string _prenom = string.Empty;
        private string _motDePasse = string.Empty;
        private string _roleSelectionne = "utilisateur";

        private string _nomUtilisateurError = string.Empty;
        private string _emailError = string.Empty;

        private UtilisateurRepository _repository;

        public ObservableCollection<Utilisateur> Utilisateurs
        {
            get => _utilisateurs;
            set => SetProperty(ref _utilisateurs, value, nameof(Utilisateurs));
        }

        public Utilisateur? UtilisateurSelectionne
        {
            get => _utilisateurSelectionne;
            set
            {
                if (SetProperty(ref _utilisateurSelectionne, value, nameof(UtilisateurSelectionne)))
                {
                    if (value != null)
                    {
                        ChargerUtilisateurEnFormulaire(value);
                    }
                }
            }
        }

        public Utilisateur? UtilisateurConnecte
        {
            get => _utilisateurConnecte;
            set => SetProperty(ref _utilisateurConnecte, value, nameof(UtilisateurConnecte));
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

        public string NomUtilisateur
        {
            get => _nomUtilisateur;
            set => SetProperty(ref _nomUtilisateur, value, nameof(NomUtilisateur));
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value, nameof(Email));
        }

        public string Nom
        {
            get => _nom;
            set => SetProperty(ref _nom, value, nameof(Nom));
        }

        public string Prenom
        {
            get => _prenom;
            set => SetProperty(ref _prenom, value, nameof(Prenom));
        }

        public string MotDePasse
        {
            get => _motDePasse;
            set => SetProperty(ref _motDePasse, value, nameof(MotDePasse));
        }

        public string RoleSelectionne
        {
            get => _roleSelectionne;
            set => SetProperty(ref _roleSelectionne, value, nameof(RoleSelectionne));
        }

        public string NomUtilisateurError
        {
            get => _nomUtilisateurError;
            set => SetProperty(ref _nomUtilisateurError, value, nameof(NomUtilisateurError));
        }

        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value, nameof(EmailError));
        }

        // Commandes
        public ICommand ChargerCommand { get; }
        public ICommand SauvegarderCommand { get; }
        public ICommand ModifierCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand AnnulerCommand { get; }
        public ICommand Nouveau { get; }

        public UtilisateurViewModel()
        {
            _utilisateurs = new ObservableCollection<Utilisateur>();
            _repository = new UtilisateurRepository();

            ChargerCommand = new RelayCommand(_ => ChargerUtilisateurs());
            SauvegarderCommand = new RelayCommand(_ => PerformerSauvegarde(), _ => EstEnEdition && !IsLoading);
            ModifierCommand = new RelayCommand(_ => PerformerModification(), _ => UtilisateurSelectionne != null && !IsLoading && PeuxModifier());
            SupprimerCommand = new RelayCommand(_ => PerformerSuppression(), _ => UtilisateurSelectionne != null && !IsLoading && PeuxSupprimer());
            AnnulerCommand = new RelayCommand(_ => AnnulerEdition());
            Nouveau = new RelayCommand(_ => NouveauUtilisateur());

            ChargerUtilisateurs();
        }

        public void InitialiserAvecUtilisateurConnecte(Utilisateur utilisateur)
        {
            UtilisateurConnecte = utilisateur;
        }

        /// <summary>
        /// Vérifie si l'utilisateur connecté peut modifier l'utilisateur sélectionné
        /// </summary>
        private bool PeuxModifier()
        {
            if (UtilisateurConnecte == null || UtilisateurSelectionne == null)
                return false;

            // Super Admin : tous les droits
            if (UtilisateurConnecte.Role == "super_admin")
                return true;

            // Admin : ne peut pas modifier un Super Admin ou un autre Admin
            if (UtilisateurConnecte.Role == "admin")
            {
                if (UtilisateurSelectionne.Role == "super_admin")
                    return false;
                if (UtilisateurSelectionne.Role == "admin" && UtilisateurSelectionne.IdUtilisateur != UtilisateurConnecte.IdUtilisateur)
                    return false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Vérifie si l'utilisateur connecté peut supprimer l'utilisateur sélectionné
        /// </summary>
        private bool PeuxSupprimer()
        {
            if (UtilisateurConnecte == null || UtilisateurSelectionne == null)
                return false;

            // Ne peut pas se supprimer soi-même
            if (UtilisateurSelectionne.IdUtilisateur == UtilisateurConnecte.IdUtilisateur)
                return false;

            // Super Admin : tous les droits
            if (UtilisateurConnecte.Role == "super_admin")
                return true;

            // Admin : ne peut pas supprimer un Super Admin ou un autre Admin
            if (UtilisateurConnecte.Role == "admin")
            {
                return UtilisateurSelectionne.Role != "super_admin" && UtilisateurSelectionne.Role != "admin";
            }

            return false;
        }

        /// <summary>
        /// Vérifie si l'utilisateur connecté peut créer un utilisateur avec le rôle sélectionné
        /// </summary>
        private bool PeuxCreerAvecRole(string role)
        {
            if (UtilisateurConnecte == null)
                return false;

            // Super Admin : peut créer n'importe quel rôle
            if (UtilisateurConnecte.Role == "super_admin")
                return true;

            // Admin : peut créer des utilisateurs normaux uniquement
            if (UtilisateurConnecte.Role == "admin")
            {
                return role == "utilisateur";
            }

            return false;
        }

        public void ChargerUtilisateurs()
        {
            try
            {
                IsLoading = true;
                var utilisateurs = _repository.ObtenirTousLesUtilisateurs();
                Utilisateurs = new ObservableCollection<Utilisateur>(utilisateurs);
                MessageErreur = string.Empty;
            }
            catch (Exception ex)
            {
                MessageErreur = $"Erreur lors du chargement : {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void NouveauUtilisateur()
        {
            EstEnEdition = true;
            _estNouveauUtilisateur = true;
            UtilisateurSelectionne = null;
            ReinitialiserFormulaire();
            MessageErreur = string.Empty;
        }

        private void ChargerUtilisateurEnFormulaire(Utilisateur utilisateur)
        {
            NomUtilisateur = utilisateur.NomUtilisateur;
            Email = utilisateur.Email;
            Nom = utilisateur.Nom;
            Prenom = utilisateur.Prenom;
            RoleSelectionne = utilisateur.Role;
            MotDePasse = string.Empty; // Ne pas afficher le mot de passe
        }

        private void ReinitialiserFormulaire()
        {
            NomUtilisateur = string.Empty;
            Email = string.Empty;
            Nom = string.Empty;
            Prenom = string.Empty;
            MotDePasse = string.Empty;
            RoleSelectionne = "utilisateur";
            NomUtilisateurError = string.Empty;
            EmailError = string.Empty;
        }

        private void PerformerModification()
        {
            if (UtilisateurSelectionne == null)
            {
                MessageErreur = "Aucun utilisateur selectionne.";
                return;
            }

            EstEnEdition = true;
            _estNouveauUtilisateur = false;
        }

        private void PerformerSauvegarde()
        {
            if (!ValiderFormulaire())
                return;

            try
            {
                IsLoading = true;

                if (_estNouveauUtilisateur)
                {
                    // Vérifier les permissions pour créer avec ce rôle
                    if (!PeuxCreerAvecRole(RoleSelectionne))
                    {
                        MessageErreur = $"Vous n'avez pas les droits pour creer un {RoleSelectionne}.";
                        return;
                    }

                    // Vérifier que le mot de passe est renseigné pour un nouvel utilisateur
                    if (string.IsNullOrWhiteSpace(MotDePasse))
                    {
                        MessageErreur = "Le mot de passe est obligatoire pour un nouvel utilisateur.";
                        return;
                    }

                    // Créer le nouvel utilisateur
                    var nouvel = new Utilisateur
                    {
                        NomUtilisateur = NomUtilisateur,
                        Email = Email,
                        Nom = Nom,
                        Prenom = Prenom,
                        MotDePasse = MotDePasse,
                        Role = RoleSelectionne,
                        Actif = true
                    };

                    System.Diagnostics.Debug.WriteLine($"Ajout utilisateur: {nouvel.NomUtilisateur}, Role: {nouvel.Role}, MdP length: {nouvel.MotDePasse.Length}");

                    if (_repository.AjouterUtilisateur(nouvel))
                    {
                        MessageErreur = "Utilisateur ajoute avec succes !";
                        ChargerUtilisateurs();
                        AnnulerEdition();
                    }
                    else
                    {
                        MessageErreur = "Erreur lors de l'ajout. Verifiez les donnees.";
                    }
                }
                else if (UtilisateurSelectionne != null)
                {
                    // Modifier l'utilisateur existant
                    UtilisateurSelectionne.NomUtilisateur = NomUtilisateur;
                    UtilisateurSelectionne.Email = Email;
                    UtilisateurSelectionne.Nom = Nom;
                    UtilisateurSelectionne.Prenom = Prenom;
                    UtilisateurSelectionne.Role = RoleSelectionne;

                    // Mettre à jour le mot de passe seulement s'il est fourni
                    if (!string.IsNullOrWhiteSpace(MotDePasse))
                    {
                        UtilisateurSelectionne.MotDePasse = MotDePasse;
                    }

                    if (_repository.ModifierUtilisateur(UtilisateurSelectionne))
                    {
                        MessageErreur = "Utilisateur modifie avec succes !";
                        ChargerUtilisateurs();
                        AnnulerEdition();
                    }
                    else
                    {
                        MessageErreur = "Erreur lors de la modification.";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageErreur = $"Erreur : {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void PerformerSuppression()
        {
            if (UtilisateurSelectionne == null)
            {
                MessageErreur = "Aucun utilisateur selectionne.";
                return;
            }

            try
            {
                IsLoading = true;
                if (_repository.SupprimerUtilisateur(UtilisateurSelectionne.IdUtilisateur))
                {
                    MessageErreur = "Utilisateur supprime avec succes !";
                    ChargerUtilisateurs();
                    ReinitialiserFormulaire();
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
            _estNouveauUtilisateur = false;
            UtilisateurSelectionne = null;
            ReinitialiserFormulaire();
            MessageErreur = string.Empty;
        }

        private bool ValiderFormulaire()
        {
            bool valide = true;

            if (string.IsNullOrWhiteSpace(NomUtilisateur))
            {
                NomUtilisateurError = "Le nom d'utilisateur est requis.";
                valide = false;
            }
            else
            {
                NomUtilisateurError = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            {
                EmailError = "Un email valide est requis.";
                valide = false;
            }
            else
            {
                EmailError = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(RoleSelectionne))
            {
                MessageErreur = "Veuillez selectionner un role.";
                valide = false;
            }

            return valide;
        }
    }
}
