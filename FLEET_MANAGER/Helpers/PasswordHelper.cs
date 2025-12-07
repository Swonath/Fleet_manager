namespace FLEET_MANAGER.Helpers
{
    /// <summary>
    /// Classe utilitaire pour la gestion des mots de passe avec BCrypt
    /// 
    /// BCrypt est un algorithme de hashage sécurisé qui :
    /// - Génère un "sel" (salt) aléatoire automatiquement
    /// - Est volontairement lent pour résister aux attaques par force brute
    /// - Produit un hash différent à chaque fois (même pour le même mot de passe)
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Hashe un mot de passe en clair avec BCrypt
        /// </summary>
        /// <param name="motDePasse">Le mot de passe en clair</param>
        /// <returns>Le hash BCrypt du mot de passe</returns>
        public static string HasherMotDePasse(string motDePasse)
        {
            // Le paramètre 12 est le "work factor" (coùt)
            // Plus il est élevé, plus le hashage est lent (et sécurisé)
            // 12 est une bonne valeur par défaut
            return BCrypt.Net.BCrypt.HashPassword(motDePasse, workFactor: 12);
        }

        /// <summary>
        /// Vérifie si un mot de passe en clair correspond à un hash BCrypt
        /// </summary>
        /// <param name="motDePasse">Le mot de passe en clair à vérifier</param>
        /// <param name="hash">Le hash BCrypt stocké en base de données</param>
        /// <returns>True si le mot de passe est correct, False sinon</returns>
        public static bool VerifierMotDePasse(string motDePasse, string hash)
        {
            try
            {
                // BCrypt.Verify compare le mot de passe avec le hash
                // Il extrait automatiquement le sel du hash pour faire la comparaison
                return BCrypt.Net.BCrypt.Verify(motDePasse, hash);
            }
            catch
            {
                // En cas d'erreur (hash invalide, etc.), retourner false
                return false;
            }
        }
    }
}
