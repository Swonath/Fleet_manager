# Fleet Manager - Gestion de Parc Automobile

Une application de bureau développée en C# avec WPF pour la gestion complète d'un parc automobile.

## ?? Cahier des Charges

- **Type** : Projet individuel universitaire
- **Langage** : C# 12.0
- **Framework** : .NET 8 (WPF)
- **Base de données** : MySQL
- **Architecture** : MVVM
- **Durée** : 03 Novembre - 24 Novembre 2025

## ?? Fonctionnalités

### Modules implémentés
- ? **Authentification** : Connexion sécurisée des utilisateurs
- ? **Tableau de bord** : Indicateurs clés et vue d'ensemble
- ? **Gestion des véhicules** : CRUD complet
- ? **Suivi du carburant** : Saisie et consultation
- ? **Gestion des trajets** : Enregistrement des distances
- ? **Maintenances** : Suivi des interventions
- ? **Rapports** : Génération de statistiques

## ?? Installation

### Prérequis
- Visual Studio 2022 (Community ou supérieur)
- .NET 8 SDK
- MySQL Server 8.0+
- Git

### Étapes d'installation

1. **Cloner le repository**
```bash
git clone <votre-repository>
cd FLEET_MANAGER
```

2. **Installer les dépendances NuGet**
```bash
dotnet restore
```

3. **Créer la base de données**

   a. Ouvrir MySQL Workbench ou MySQL Command Line
   
   b. Charger le script SQL :
   ```bash
   mysql -u root -p < FLEET_MANAGER/Database/fleet_manager.sql
   ```
   
   c. Ou copier-coller le contenu du fichier `FLEET_MANAGER/Database/fleet_manager.sql`

4. **Configurer la connexion à la base de données**

   Éditer `FLEET_MANAGER/Config/DatabaseConfig.cs` :
   ```csharp
   public static class DatabaseConfig
   {
       public static string Server { get; set; } = "localhost";
       public static string Database { get; set; } = "fleet_manager";
       public static string UserId { get; set; } = "root";
       public static string Password { get; set; } = "votre_mot_de_passe";
       public static int Port { get; set; } = 3306;
   }
   ```

5. **Compiler et exécuter**
```bash
dotnet build
dotnet run
```

## ?? Authentification

### Identifiants par défaut
- **Nom d'utilisateur** : `admin`
- **Mot de passe** : `admin123`
- **Rôle** : Administrateur

**Attention** : Changez ces identifiants en production !

## ?? Structure du projet

```
FLEET_MANAGER/
??? Config/                    # Configuration de l'application
?   ??? DatabaseConfig.cs
??? Converters/               # Convertisseurs XAML
?   ??? InvertBoolConverter.cs
??? Data/                     # Couche d'accès aux données
?   ??? DatabaseConnection.cs
??? Database/                 # Scripts SQL
?   ??? fleet_manager.sql
??? Models/                   # Modèles de données
?   ??? Models.cs
??? Repositories/             # Repositories (accès aux données)
?   ??? Repositories.cs
??? ViewModels/              # ViewModels (MVVM)
?   ??? ViewModelBase.cs
?   ??? LoginViewModel.cs
?   ??? DashboardViewModel.cs
??? Views/                    # Vues XAML
?   ??? LoginWindow.xaml
?   ??? LoginWindow.xaml.cs
?   ??? MainWindow.xaml
?   ??? MainWindow.xaml.cs
??? App.xaml
??? App.xaml.cs
??? FLEET_MANAGER.csproj
```

## ??? Schéma de la base de données

### Tables principales

- **utilisateurs** : Gestion des utilisateurs et authentification
- **vehicules** : Informations sur les véhicules
- **carburants** : Historique des ravitaillements
- **trajets** : Enregistrement des trajets effectués
- **maintenances** : Suivi des interventions de maintenance
- **rapports** : Cache des rapports générés

## ??? Technologies utilisées

- **C# 12.0** : Langage de programmation
- **WPF** : Framework d'interface utilisateur
- **MVVM** : Modèle architectural
- **MySQL.Data** : Connecteur MySQL
- **Binding** : Liaison de données
- **XAML** : Markup pour les interfaces

## ?? Utilisation

### 1. Connexion
1. Lancer l'application
2. Entrer les identifiants
3. Cliquer sur "Connexion"

### 2. Tableau de bord
- Vue d'ensemble des indicateurs clés
- Aperçu de la flotte de véhicules

### 3. Gestion des véhicules
- Ajouter un nouveau véhicule
- Modifier les informations
- Supprimer un véhicule
- Voir l'historique

### 4. Suivi du carburant
- Enregistrer une consommation
- Consulter l'historique
- Analyser les tendances

### 5. Gestion des trajets
- Enregistrer un trajet
- Consulter l'historique
- Générer des rapports

## ?? Fonctionnalités à venir

- [ ] Module de gestion des utilisateurs
- [ ] Graphiques et statistiques avancées
- [ ] Export en PDF et Excel
- [ ] Synchronisation cloud
- [ ] Application mobile
- [ ] Alertes de maintenance

## ?? Debugging

### Problèmes courants

**Erreur de connexion à la base de données**
- Vérifier que MySQL est en cours d'exécution
- Vérifier les paramètres de connexion dans `DatabaseConfig.cs`
- Vérifier que la base de données `fleet_manager` existe

**Erreur de compilation XAML**
- Nettoyer et reconstruire la solution
- Vérifier l'encodage UTF-8 des fichiers

**Erreur lors du login**
- Vérifier l'existence de l'utilisateur dans la base de données
- Vérifier que l'utilisateur est actif (`actif = 1`)

## ?? Notes de développement

### Architecture MVVM

L'application suit le modèle MVVM avec :
- **Models** : Représentation des données
- **ViewModels** : Logique de présentation et gestion de l'état
- **Views** : Interface utilisateur (XAML)
- **Repositories** : Accès aux données

### Binding et INotifyPropertyChanged

Tous les ViewModels héritent de `ViewModelBase` qui implémente `INotifyPropertyChanged` pour la notification de changements de propriétés.

### Commandes

Les commandes sont gérées par la classe `RelayCommand` qui implémente `ICommand`.

## ????? Auteur

[Votre nom]
Projet universitaire - 2025

## ?? Licence

À définir

## ?? Support

Pour toute question ou problème, veuillez ouvrir une issue sur GitHub.

---

**Dernière mise à jour** : 2025-11-03
