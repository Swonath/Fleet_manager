using System;
using System.IO;

namespace FLEET_MANAGER.Helpers
{
    /// <summary>
    /// Classe pour logger les erreurs dans un fichier
    /// </summary>
    public static class Logger
    {
        private static readonly string LogFilePath;
        private static readonly object LockObject = new object();

        static Logger()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            LogFilePath = Path.Combine(appDirectory, "fleet_manager_log.txt");
        }

        /// <summary>
        /// Écrit un message dans le fichier de log
        /// </summary>
        public static void Log(string message)
        {
            try
            {
                lock (LockObject)
                {
                    string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                    File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);

                    // Aussi afficher dans la console de debug
                    System.Diagnostics.Debug.WriteLine(logEntry);
                }
            }
            catch
            {
                // Ignorer les erreurs de logging pour ne pas crasher l'application
            }
        }

        /// <summary>
        /// Écrit une erreur avec son stack trace dans le fichier de log
        /// </summary>
        public static void LogError(string context, Exception ex)
        {
            try
            {
                lock (LockObject)
                {
                    string logEntry = $@"
=== ERREUR [{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ===
Contexte: {context}
Message: {ex.Message}
Type: {ex.GetType().Name}
StackTrace: {ex.StackTrace}
=====================================
";
                    File.AppendAllText(LogFilePath, logEntry);

                    // Aussi afficher dans la console de debug
                    System.Diagnostics.Debug.WriteLine(logEntry);
                }
            }
            catch
            {
                // Ignorer les erreurs de logging pour ne pas crasher l'application
            }
        }

        /// <summary>
        /// Efface le contenu du fichier de log
        /// </summary>
        public static void ClearLog()
        {
            try
            {
                lock (LockObject)
                {
                    if (File.Exists(LogFilePath))
                    {
                        File.Delete(LogFilePath);
                    }
                }
            }
            catch
            {
                // Ignorer les erreurs
            }
        }

        /// <summary>
        /// Retourne le chemin du fichier de log
        /// </summary>
        public static string GetLogFilePath()
        {
            return LogFilePath;
        }
    }
}
