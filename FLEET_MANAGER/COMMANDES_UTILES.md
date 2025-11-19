# ?? Commandes Utiles - Fleet Manager

## ??? Commandes Dotnet

```bash
# Restaurer les packages NuGet
dotnet restore

# Compiler le projet
dotnet build

# Compiler en mode Release
dotnet build -c Release

# Exécuter l'application
dotnet run

# Exécuter en mode Release
dotnet run -c Release

# Nettoyer les fichiers générés
dotnet clean

# Ajouter un package NuGet
dotnet add package NomDuPackage

# Supprimer un package NuGet
dotnet remove package NomDuPackage

# Lister les packages installés
dotnet list package

# Mettre à jour un package
dotnet add package NomDuPackage --version 9.0.0

# Publier l'application (créer un exécutable)
dotnet publish -c Release -o ./publish
```

## ??? Commandes MySQL

### Connexion à MySQL
```bash
# Connexion basique
mysql -u root -p

# Connexion avec mot de passe
mysql -u root -pVotreMotDePass

# Connexion à une base spécifique
mysql -u root -p fleet_manager

# Connexion à un serveur distant
mysql -h 192.168.1.100 -u root -p
```

### Gestion des Bases de Données
```sql
-- Afficher les bases de données
SHOW DATABASES;

-- Créer la base (alternative au script SQL)
CREATE DATABASE fleet_manager;

-- Utiliser une base
USE fleet_manager;

-- Afficher les tables
SHOW TABLES;

-- Afficher les colonnes d'une table
DESCRIBE vehicules;
-- OU
SHOW COLUMNS FROM vehicules;

-- Sauvegarder une base
mysqldump -u root -p fleet_manager > fleet_manager_backup.sql

-- Restaurer une base
mysql -u root -p fleet_manager < fleet_manager_backup.sql

-- Supprimer une base
DROP DATABASE fleet_manager;
```

### Opérations sur les Données
```sql
-- Compter les véhicules
SELECT COUNT(*) FROM vehicules;

-- Afficher tous les véhicules
SELECT * FROM vehicules;

-- Afficher avec filtrage
SELECT * FROM vehicules WHERE etat = 'En service';

-- Afficher les utilisateurs
SELECT * FROM utilisateurs;

-- Créer un nouvel utilisateur
INSERT INTO utilisateurs (nom_utilisateur, email, mot_de_passe, nom, prenom, role)
VALUES ('utilisateur_test', 'test@example.com', 'password123', 'Test', 'Utilisateur', 'utilisateur');

-- Modifier un utilisateur
UPDATE utilisateurs SET email = 'newemail@example.com' WHERE nom_utilisateur = 'admin';

-- Supprimer un utilisateur
DELETE FROM utilisateurs WHERE nom_utilisateur = 'utilisateur_test';

-- Afficher les véhicules actifs
SELECT * FROM vehicules WHERE etat = 'En service' ORDER BY marque;

-- Compter les trajets par véhicule
SELECT id_vehicule, COUNT(*) as nombre_trajets 
FROM trajets 
GROUP BY id_vehicule;

-- Voir le coût total des carburants
SELECT SUM(cout_total) as cout_total FROM carburants;

-- Exporter les données en CSV (certains clients MySQL)
SELECT * INTO OUTFILE '/tmp/vehicules.csv' FIELDS TERMINATED BY ',' FROM vehicules;
```

## ?? Configuration MySQL

### Créer un utilisateur spécifique pour l'app
```sql
-- Créer l'utilisateur
CREATE USER 'fleet_user'@'localhost' IDENTIFIED BY 'fleet_password';

-- Donner les permissions
GRANT ALL PRIVILEGES ON fleet_manager.* TO 'fleet_user'@'localhost';

-- Appliquer les changements
FLUSH PRIVILEGES;

-- Vérifier les permissions
SHOW GRANTS FOR 'fleet_user'@'localhost';
```

### Changer le mot de passe root
```sql
ALTER USER 'root'@'localhost' IDENTIFIED BY 'nouveau_mot_de_passe';
FLUSH PRIVILEGES;
```

## ?? Requêtes de Diagnostic

### Vérifier la connexion
```csharp
// En C#
if (DatabaseConnection.TestConnection())
{
    Console.WriteLine("Connexion réussie !");
}
else
{
    Console.WriteLine("Erreur de connexion");
}
```

### Voir la taille de la base de données
```sql
SELECT 
    table_schema as `Base`,
    SUM(data_length + index_length) / 1024 / 1024 AS `Taille (MB)`
FROM information_schema.tables
WHERE table_schema = 'fleet_manager'
GROUP BY table_schema;
```

### Afficher les tables et leurs tailles
```sql
SELECT 
    table_name,
    ROUND(((data_length + index_length) / 1024 / 1024), 2) as `Taille (MB)`
FROM information_schema.tables
WHERE table_schema = 'fleet_manager'
ORDER BY (data_length + index_length) DESC;
```

### Voir les clés étrangères
```sql
SELECT 
    CONSTRAINT_NAME,
    TABLE_NAME,
    COLUMN_NAME,
    REFERENCED_TABLE_NAME,
    REFERENCED_COLUMN_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_SCHEMA = 'fleet_manager';
```

## ??? Debugging en Visual Studio

### Points d'Arrêt
```csharp
// Mettre un point d'arrêt
Ctrl + F9
```

### Console de Débogage
```csharp
// Afficher des informations
System.Diagnostics.Debug.WriteLine("Message de debug");

// Avec condition
#if DEBUG
    System.Diagnostics.Debug.WriteLine("Ceci n'affichera qu'en Debug");
#endif
```

### Fenêtre de Sortie
```
Affichage ? Sortie (Ctrl + Alt + O)
```

### Fenêtre Locales
```
Affichage ? Fenêtres de débogage ? Locales (Ctrl + Alt + V, L)
```

### Espions
```
Affichage ? Fenêtres de débogage ? Espions
Ou clic droit sur variable ? Ajouter un espion
```

## ?? Build et Publication

### Builder le projet
```bash
# Build Debug (plus rapide, plus de détails de debug)
dotnet build

# Build Release (optimisé, plus rapide à l'exécution)
dotnet build -c Release

# Nettoyer puis builder
dotnet clean && dotnet build
```

### Publier l'application
```bash
# Publier en mode Release
dotnet publish -c Release -o ./publish

# Publier comme exécutable autonome (ne nécessite pas .NET)
dotnet publish -c Release -r win-x64 --self-contained -o ./publish

# Publier comme trimmed (plus petit, plus rapide)
dotnet publish -c Release -r win-x64 --self-contained --trimmed -o ./publish
```

## ?? Recherche et Navigation

### Rechercher du Code
```
Ctrl + F           # Recherche dans le fichier courant
Ctrl + H           # Recherche/Remplacer
Ctrl + Shift + F   # Recherche dans tous les fichiers
```

### Navigation
```
Ctrl + G           # Aller à la ligne
Ctrl + .           # Actions rapides et refactorisations
F12                # Aller à la définition
Ctrl + -           # Historique précédent
Ctrl + Maj + -     # Historique suivant
```

## ?? Gestion des Packages NuGet

```bash
# Vérifier les packages obsolètes
dotnet outdated

# Mettre à jour tous les packages
dotnet add package --update-all

# Afficher les packages avec des vulnérabilités
dotnet list package --vulnerable

# Afficher les packages obsolètes
dotnet list package --outdated
```

## ?? Tests

### Exécuter les tests
```bash
# Exécuter tous les tests
dotnet test

# Exécuter les tests avec verbose
dotnet test --verbosity detailed

# Exécuter un test spécifique
dotnet test --filter "TestName"

# Générer un rapport de couverture
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## ?? Gestion des Fichiers

### Fichiers importants à sauvegarder
```bash
# Sauvegarder la configuration
cp Config/DatabaseConfig.cs Config/DatabaseConfig.cs.backup

# Sauvegarder la base de données
mysqldump -u root -p fleet_manager > fleet_manager_backup.sql

# Compresser le projet
tar -czf fleet_manager_backup.tar.gz ./
```

## ?? Raccourcis Utiles Visual Studio

```
Ctrl + K, Ctrl + C    # Commenter les lignes sélectionnées
Ctrl + K, Ctrl + U    # Décommenter les lignes sélectionnées
Ctrl + Maj + L        # Formater le document
Ctrl + Shift + X      # Afficher les résultats de recherche
F5                    # Démarrer le déboggage
Maj + F5              # Arrêter le déboggage
Ctrl + Alt + B        # Fenêtre Points d'arrêt
Ctrl + Alt + W        # Fenêtre Espions
```

## ?? Astuces de Productivité

```bash
# Utiliser un alias Git pour les commandes fréquentes
git config --global alias.co checkout
git config --global alias.br branch
git config --global alias.ci commit
git config --global alias.st status

# Compiler sans ouvrir Visual Studio
dotnet build --project FLEET_MANAGER/FLEET_MANAGER.csproj

# Exécuter avec paramètres
dotnet run -- --help

# Créer un profil de run
# Débogage ? Propriétés de déboggage ? Créer un profil
```

---

**Dernière mise à jour** : 2025-11-03
