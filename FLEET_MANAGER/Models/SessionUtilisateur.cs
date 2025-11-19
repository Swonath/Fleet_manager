namespace FLEET_MANAGER.Models
{
    /// <summary>
    /// Classe pour gérer la session utilisateur connecté
    /// </summary>
    public static class SessionUtilisateur
    {
        /// <summary>
        /// L'utilisateur actuellement connecté
        /// </summary>
        public static Utilisateur? UtilisateurConnecte { get; set; }

        /// <summary>
        /// Vérifie si l'utilisateur est Super Admin
        /// </summary>
        public static bool EstSuperAdmin =>
            UtilisateurConnecte?.Role == "super_admin";

        /// <summary>
        /// Vérifie si l'utilisateur est Admin ou Super Admin
        /// </summary>
        public static bool EstAdmin =>
            UtilisateurConnecte?.Role == "admin" || EstSuperAdmin;

        /// <summary>
        /// Vérifie si l'utilisateur est utilisateur normal
        /// </summary>
        public static bool EstUtilisateurNormal =>
            UtilisateurConnecte?.Role == "utilisateur";

        /// <summary>
        /// Déconnecte l'utilisateur
        /// </summary>
        public static void Deconnecter()
        {
            UtilisateurConnecte = null;
        }

        /// <summary>
        /// Vérifie si un utilisateur est connecté
        /// </summary>
        public static bool EstConnecte => UtilisateurConnecte != null;
    }
}
