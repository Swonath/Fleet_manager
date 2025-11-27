using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FLEET_MANAGER.Models;
using FLEET_MANAGER.Repositories;

namespace FLEET_MANAGER.ViewModels
{
    /// <summary>
    /// ViewModel pour la gestion des utilisateurs
    /// G�re les permissions selon le r�le de l'utilisateur connect�
    /// </summary>
    public class UtilisateurViewModel : ViewModelBase
    {
        private ObservableCollection<Utilisateur> _utilisateurs;
        private ObservableCollection<Utilisateur> _utilisateursFiltres;
        private Utilisateur? _utilisateurSelectionne;
        private bool _estEnEdition = false;
        private string _messageErreur = string.Empty;
        private bool _isLoading = false;
        private bool _estNouveauUtilisateur = false;
        private Utilisateur? _utilisateurConnecte;
        private string _rechercheTexte = string.Empty;

        private string _nomUtilisateur = string.Empty;
        private string _email = string.Empty;
        private string _nom = string.Empty;
        private string _prenom = string.Empty;
        private string _motDePasse = string.Empty;
        private string _roleSelectionne = "utilisateur";

        private string _nomUtilisateurError = string.Empty;
        private string _emailError = string.Empty;

        private UtilisateurRepository _repository;
        private const int TAILLE_PAGE = 12;
        private List<Utilisateur> _tousLesUtilisateurs = new List<Utilisateur>();
        private List<Utilisateur> _utilisateursFiltresComplet = new List<Utilisateur>();
        private int _nombreUtilisateursCharges = 0;
        private bool _tousUtilisateursCharges = false;

        public bool TousUtilisateursCharges
        {
            get => _tousUtilisateursCharges;
            set => SetProperty(ref _tousUtilisateursCharges, value, nameof(TousUtilisateursCharges));
        }

        public ObservableCollection<Utilisateur> Utilisateurs
        {
            get => _utilisateurs;
            set => SetProperty(ref _utilisateurs, value, nameof(Utilisateurs));
        }

        public ObservableCollection<Utilisateur> UtilisateursFiltres
        {
            get => _utilisateursFiltres;
            set => SetProperty(ref _utilisateursFiltres, value, nameof(UtilisateursFiltres));
        }

        public string RechercheTexte
        {
            get => _rechercheTexte;
            set
            {
                if (SetProperty(ref _rechercheTexte, value, nameof(RechercheTexte)))
                {
                    FiltrerUtilisateurs();
                }
            }
        }

        public Utilisateur? UtilisateurSelectionne
        {
            get => _utilisateurSelectionne;
            set
            {
                if (SetProperty(ref _utilisateurSelectionne, value, nameof(UtilisateurSelectionne)))
                {
                    if (value != null && !EstEnEdition)
                    {
                        ChargerUtilisateurEnFormulaire(value);
                    }
                    OnPropertyChanged(nameof(AfficherFormulaire));
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
            set
            {
                if (SetProperty(ref _estEnEdition, value, nameof(EstEnEdition)))
                {
                    OnPropertyChanged(nameof(AfficherFormulaire));
                }
            }
        }

        public bool AfficherFormulaire => EstEnEdition || UtilisateurSelectionne != null;

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
        public ICommand ChargerPlusCommand { get; }

        public UtilisateurViewModel()
        {
            _utilisateurs = new ObservableCollection<Utilisateur>();
            _utilisateursFiltres = new ObservableCollection<Utilisateur>();
            _repository = new UtilisateurRepository();

            ChargerCommand = new RelayCommand(_ => ChargerUtilisateurs());
            SauvegarderCommand = new RelayCommand(_ => PerformerSauvegarde(), _ => EstEnEdition && !IsLoading);
            ModifierCommand = new RelayCommand(_ => PerformerModification(), _ => UtilisateurSelectionne != null && !IsLoading && PeuxModifier());
            SupprimerCommand = new RelayCommand(_ => PerformerSuppression(), _ => UtilisateurSelectionne != null && !IsLoading && PeuxSupprimer());
            AnnulerCommand = new RelayCommand(_ => AnnulerEdition());
            Nouveau = new RelayCommand(_ => NouveauUtilisateur());
            ChargerPlusCommand = new RelayCommand(_ => ChargerUtilisateursSuivants());

            ChargerUtilisateurs();
        }

        public void InitialiserAvecUtilisateurConnecte(Utilisateur utilisateur)
        {
            UtilisateurConnecte = utilisateur;
        }

        /// <summary>
        /// V�rifie si l'utilisateur connect� peut modifier l'utilisateur s�lectionn�
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
        /// V�rifie si l'utilisateur connect� peut supprimer l'utilisateur s�lectionn�
        /// </summary>
        private bool PeuxSupprimer()
        {
            if (UtilisateurConnecte == null || UtilisateurSelectionne == null)
                return false;

            // Ne peut pas se supprimer soi-m�me
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
        /// V�rifie si l'utilisateur connect� peut cr�er un utilisateur avec le r�le s�lectionn�
        /// </summary>
        private bool PeuxCreerAvecRole(string role)
        {
            if (UtilisateurConnecte == null)
                return false;

            // Super Admin : peut cr�er n'importe quel r�le
            if (UtilisateurConnecte.Role == "super_admin")
                return true;

            // Admin : peut cr�er des utilisateurs normaux uniquement
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
                // Charger tous les utilisateurs pour les stats et la recherche
                _tousLesUtilisateurs = _repository.ObtenirTousLesUtilisateurs();
                Utilisateurs = new ObservableCollection<Utilisateur>(_tousLesUtilisateurs);

                // Réinitialiser la pagination
                _nombreUtilisateursCharges = 0;
                UtilisateursFiltres.Clear();

                // Filtrer et charger la première page
                FiltrerUtilisateurs();
                MessageErreur = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement : {ex}");
                MessageErreur = "Erreur lors du chargement des utilisateurs.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FiltrerUtilisateurs()
        {
            // Appliquer le filtre de recherche
            if (string.IsNullOrWhiteSpace(RechercheTexte))
            {
                _utilisateursFiltresComplet = _tousLesUtilisateurs.ToList();
            }
            else
            {
                var recherche = RechercheTexte.ToLower();
                _utilisateursFiltresComplet = _tousLesUtilisateurs.Where(u =>
                    u.NomUtilisateur.ToLower().Contains(recherche) ||
                    u.Email.ToLower().Contains(recherche) ||
                    u.Nom.ToLower().Contains(recherche) ||
                    u.Prenom.ToLower().Contains(recherche) ||
                    u.Role.ToLower().Contains(recherche)
                ).ToList();
            }

            // Réinitialiser et charger la première page
            _nombreUtilisateursCharges = 0;
            UtilisateursFiltres.Clear();
            ChargerUtilisateursSuivants();
        }

        private void ChargerUtilisateursSuivants()
        {
            try
            {
                var utilisateursACharger = _utilisateursFiltresComplet
                    .Skip(_nombreUtilisateursCharges)
                    .Take(TAILLE_PAGE)
                    .ToList();

                foreach (var utilisateur in utilisateursACharger)
                {
                    UtilisateursFiltres.Add(utilisateur);
                }

                _nombreUtilisateursCharges += utilisateursACharger.Count;
                TousUtilisateursCharges = _nombreUtilisateursCharges >= _utilisateursFiltresComplet.Count;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des utilisateurs suivants : {ex.Message}");
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
                    // V�rifier les permissions pour cr�er avec ce r�le
                    if (!PeuxCreerAvecRole(RoleSelectionne))
                    {
                        MessageErreur = $"Vous n'avez pas les droits pour creer un {RoleSelectionne}.";
                        return;
                    }

                    // V�rifier que le mot de passe est renseign� pour un nouvel utilisateur
                    if (string.IsNullOrWhiteSpace(MotDePasse))
                    {
                        MessageErreur = "Le mot de passe est obligatoire pour un nouvel utilisateur.";
                        return;
                    }

                    // Cr�er le nouvel utilisateur
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

                    // Mettre � jour le mot de passe seulement s'il est fourni
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
                System.Diagnostics.Debug.WriteLine($"Erreur sauvegarde utilisateur : {ex}");
                MessageErreur = "Erreur lors de la sauvegarde. Veuillez réessayer.";
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
                System.Diagnostics.Debug.WriteLine($"Erreur suppression utilisateur : {ex}");
                MessageErreur = "Erreur lors de la suppression. Veuillez réessayer.";
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

            // Valider le nom d'utilisateur
            var (nomUtilisateurValide, nomUtilisateurErreur) = Helpers.ValidationHelper.ValiderNomUtilisateur(NomUtilisateur);
            if (!nomUtilisateurValide)
            {
                NomUtilisateurError = nomUtilisateurErreur ?? "Nom d'utilisateur invalide.";
                valide = false;
            }
            else
            {
                NomUtilisateurError = string.Empty;
            }

            // Valider l'email
            var (emailValide, emailErreur) = Helpers.ValidationHelper.ValiderEmail(Email);
            if (!emailValide)
            {
                EmailError = emailErreur ?? "Email invalide.";
                valide = false;
            }
            else
            {
                EmailError = string.Empty;
            }

            // Valider le mot de passe (seulement pour nouvel utilisateur ou si fourni pour modification)
            if (_estNouveauUtilisateur || !string.IsNullOrWhiteSpace(MotDePasse))
            {
                var (motDePasseValide, motDePasseErreur) = Helpers.ValidationHelper.ValiderMotDePasse(MotDePasse);
                if (!motDePasseValide)
                {
                    MessageErreur = motDePasseErreur ?? "Mot de passe invalide.";
                    valide = false;
                }
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
