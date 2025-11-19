using System.Windows.Input;
using FLEET_MANAGER.Data;
using FLEET_MANAGER.Models;
using MySql.Data.MySqlClient;

namespace FLEET_MANAGER.ViewModels
{
    /// <summary>
    /// ViewModel pour l'écran de connexion
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

        // Événement déclenché lors d'une connexion réussie
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
                    Message = "Connexion réussie !";
                    LoginSucceeded?.Invoke(this, utilisateur);
                }
                else
                {
                    Message = "Nom d'utilisateur ou mot de passe incorrect.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Erreur : {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private Utilisateur? AuthentifierUtilisateur(string nomUtilisateur, string motDePasse)
        {
            try
            {
                string query = "SELECT * FROM utilisateurs WHERE nom_utilisateur = @username AND mot_de_passe = @password AND actif = 1";
                var parameters = new Dictionary<string, object>
                {
                    { "@username", nomUtilisateur },
                    { "@password", motDePasse }
                };

                using (var reader = DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    if (reader.Read())
                    {
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
