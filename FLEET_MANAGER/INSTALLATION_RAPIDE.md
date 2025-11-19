# Guide d'Installation Rapide - Fleet Manager

## ?? Installation en 5 minutes

### Étape 1 : Configuration de MySQL

1. Ouvrir MySQL Command Line ou Workbench
2. Exécuter le script :
```sql
-- Pour utiliser le script depuis un fichier
SOURCE C:\chemin\vers\FLEET_MANAGER\Database\fleet_manager.sql;
```

Ou copier-coller directement le contenu du fichier `fleet_manager.sql`

### Étape 2 : Configuration de l'application

1. Ouvrir le fichier `FLEET_MANAGER/Config/DatabaseConfig.cs`
2. Adapter les paramètres :

```csharp
public static class DatabaseConfig
{
    public static string Server { get; set; } = "localhost";      // Serveur MySQL
    public static string Database { get; set; } = "fleet_manager"; // Nom BD
    public static string UserId { get; set; } = "root";           // Utilisateur MySQL
    public static string Password { get; set; } = "";             // Mot de passe MySQL
    public static int Port { get; set; } = 3306;                  // Port MySQL
}
```

### Étape 3 : Compiler et exécuter

```bash
cd FLEET_MANAGER
dotnet build
dotnet run
```

### Étape 4 : Se connecter

Utiliser les identifiants par défaut :
- **Nom d'utilisateur** : admin
- **Mot de passe** : admin123

## ? Vérification

Après le démarrage, vous devriez voir :
1. ? La fenêtre de connexion
2. ? Connexion réussie
3. ? Le tableau de bord avec les indicateurs

## ?? Troubleshooting

| Problème | Solution |
|----------|----------|
| Erreur "Cannot connect to database" | Vérifier que MySQL est en cours d'exécution |
| Erreur "Database fleet_manager not found" | Exécuter le script `fleet_manager.sql` |
| Erreur "Access denied for user" | Vérifier les identifiants MySQL dans `DatabaseConfig.cs` |
| Application figée au démarrage | Vérifier la connexion MySQL en ligne de commande |

## ?? Créer un nouvel utilisateur

```sql
INSERT INTO utilisateurs (nom_utilisateur, email, mot_de_passe, nom, prenom, role)
VALUES ('john_doe', 'john@example.com', 'password123', 'Doe', 'John', 'utilisateur');
```

## ?? Paramètres de connexion courants

### Windows Authentication
```csharp
public static string UserId { get; set; } = "root";
public static string Password { get; set; } = "votre_mot_de_passe";
```

### Serveur distant
```csharp
public static string Server { get; set; } = "192.168.1.100";
public static int Port { get; set; } = 3306;
```

## ?? Aide

Si vous avez des problèmes, consultez le `README.md` pour plus d'informations.
