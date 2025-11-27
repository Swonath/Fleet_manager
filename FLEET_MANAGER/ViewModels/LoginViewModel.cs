using System.Windows.Input;
using FLEET_MANAGER.Data;
using FLEET_MANAGER.Models;
using FLEET_MANAGER.Helpers;
using MySql.Data.MySqlClient;

namespace FLEET_MANAGER.ViewModels
{
    /// <summary>
    /// ViewModel pour l'�cran de connexion
    /// Utilise BCrypt pour v�rifier les mots de passe hash�s
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private string _nomUtilisateur = string.Empty;
        private string _motDePasse = string.Empty;
        private string _message = string.Empty;
        private bool _isLoading = false;

        public string NomUtilisateur
        {
            get => _nomUtilisateur;
            set => SetProperty(ref _nomUtilisateur, value, nameof(NomUtilisateur));
        }

        public string MotDePasse
        {
            get => _motDePasse;
            set => SetProperty(ref _motDePasse, value, nameof(MotDePasse));
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value, nameof(Message));
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value, nameof(IsLoading));
        }

        public ICommand LoginCommand { get; }

        // �v�nement d�clench� lors d'une connexion r�ussie
        public event EventHandler<Utilisateur>? LoginSucceeded;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(_ => PerformerConnexion(), _ => !IsLoading);
        }

        private void PerformerConnexion()
        {
            if (string.IsNullOrWhiteSpace(NomUtilisateur) || string.IsNullOrWhiteSpace(MotDePasse))
            {
                Message = "Veuillez entrer vos identifiants.";
                return;
            }

            IsLoading = true;
            Message = "Connexion en cours...";

            try
            {
                var utilisateur = AuthentifierUtilisateur(NomUtilisateur, MotDePasse);

                if (utilisateur != null)
                {
                    Message = "Connexion r�ussie !";
                    LoginSucceeded?.Invoke(this, utilisateur);
                }
                else
                {
                    Message = "Nom d'utilisateur ou mot de passe incorrect.";
                }
            }
            catch (Exception ex)
            {
                // Log détaillé pour le débogage
                System.Diagnostics.Debug.WriteLine($"Erreur de connexion : {ex}");
                // Message générique pour l'utilisateur
                Message = "Une erreur est survenue lors de la connexion. Veuillez réessayer.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Authentifie un utilisateur en v�rifiant son mot de passe avec BCrypt
        /// </summary>
        private Utilisateur? AuthentifierUtilisateur(string nomUtilisateur, string motDePasse)
        {
            try
            {
                // �tape 1 : R�cup�rer l'utilisateur par son nom (sans v�rifier le mot de passe en SQL)
                string query = "SELECT * FROM utilisateurs WHERE nom_utilisateur = @username AND actif = 1";
                var parameters = new Dictionary<string, object>
                {
                    { "@username", nomUtilisateur }
                };

                using (var reader = DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    if (reader.Read())
                    {
                        // R�cup�rer le hash du mot de passe stock� en base
                        string hashStocke = reader["mot_de_passe"]?.ToString() ?? string.Empty;

                        // �tape 2 : V�rifier le mot de passe avec BCrypt
                        if (!string.IsNullOrEmpty(hashStocke) && PasswordHelper.VerifierMotDePasse(motDePasse, hashStocke))
                        {
                            // Mot de passe correct, retourner l'utilisateur
                            return new Utilisateur
                            {
                                IdUtilisateur = (int)reader["id_utilisateur"],
                                NomUtilisateur = reader["nom_utilisateur"].ToString() ?? string.Empty,
                                Email = reader["email"].ToString() ?? string.Empty,
                                Nom = reader["nom"].ToString() ?? string.Empty,
                                Prenom = reader["prenom"].ToString() ?? string.Empty,
                                Role = reader["role"].ToString() ?? "utilisateur",
                                DateCreation = (DateTime)reader["date_creation"],
                                Actif = (bool)reader["actif"]
                            };
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur d'authentification : {ex.Message}");
                throw;
            }
        }
    }
}
