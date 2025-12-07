using System.Text.RegularExpressions;

namespace FLEET_MANAGER.Helpers
{
    /// <summary>
    /// Classe utilitaire pour la validation des entrées utilisateur
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Valide un mot de passe selon une politique de sécurité
        /// </summary>
        /// <param name="motDePasse">Le mot de passe à valider</param>
        /// <returns>Tuple (isValid, errorMessage)</returns>
        public static (bool IsValid, string? ErrorMessage) ValiderMotDePasse(string motDePasse)
        {
            if (string.IsNullOrWhiteSpace(motDePasse))
            {
                return (false, "Le mot de passe ne peut pas être vide.");
            }

            if (motDePasse.Length < 8)
            {
                return (false, "Le mot de passe doit contenir au moins 8 caractères.");
            }

            if (!Regex.IsMatch(motDePasse, @"[A-Z]"))
            {
                return (false, "Le mot de passe doit contenir au moins une lettre majuscule.");
            }

            if (!Regex.IsMatch(motDePasse, @"[a-z]"))
            {
                return (false, "Le mot de passe doit contenir au moins une lettre minuscule.");
            }

            if (!Regex.IsMatch(motDePasse, @"[0-9]"))
            {
                return (false, "Le mot de passe doit contenir au moins un chiffre.");
            }

            return (true, null);
        }

        /// <summary>
        /// Valide un format d'email
        /// </summary>
        /// <param name="email">L'email à valider</param>
        /// <returns>Tuple (isValid, errorMessage)</returns>
        public static (bool IsValid, string? ErrorMessage) ValiderEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "L'email ne peut pas être vide.");
            }

            // Regex simple mais efficace pour la validation d'email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!Regex.IsMatch(email, pattern))
            {
                return (false, "Format d'email invalide.");
            }

            return (true, null);
        }

        /// <summary>
        /// Valide un nom d'utilisateur
        /// </summary>
        /// <param name="nomUtilisateur">Le nom d'utilisateur à valider</param>
        /// <returns>Tuple (isValid, errorMessage)</returns>
        public static (bool IsValid, string? ErrorMessage) ValiderNomUtilisateur(string nomUtilisateur)
        {
            if (string.IsNullOrWhiteSpace(nomUtilisateur))
            {
                return (false, "Le nom d'utilisateur ne peut pas être vide.");
            }

            if (nomUtilisateur.Length < 3)
            {
                return (false, "Le nom d'utilisateur doit contenir au moins 3 caractères.");
            }

            if (nomUtilisateur.Length > 50)
            {
                return (false, "Le nom d'utilisateur ne peut pas dépasser 50 caractères.");
            }

            // Autoriser uniquement lettres, chiffres, underscore et tiret
            if (!Regex.IsMatch(nomUtilisateur, @"^[a-zA-Z0-9_-]+$"))
            {
                return (false, "Le nom d'utilisateur ne peut contenir que des lettres, chiffres, tirets et underscores.");
            }

            return (true, null);
        }
    }
}
