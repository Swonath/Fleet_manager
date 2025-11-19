# Guide de Démarrage du Développement

## ?? Comprendre l'Architecture MVVM

### Concept de base
```
View (XAML)
    ? (bindings)
ViewModel (logique de présentation)
    ? (appels)
Model (logique métier)
    ? (accès)
Repository/Data (base de données)
```

### Exemple simplifié
```csharp
// MODEL
public class Vehicule
{
    public string Marque { get; set; }
    public string Modele { get; set; }
}

// VIEWMODEL
public class VehiculeViewModel : ViewModelBase
{
    private Vehicule _vehicule;
    
    public string Marque
    {
        get => _vehicule.Marque;
        set => SetProperty(ref _vehicule.Marque, value, nameof(Marque));
    }
    
    public ICommand SauvegarderCommand { get; }
}

// VIEW (XAML)
<TextBox Text="{Binding Marque}"/>
<Button Command="{Binding SauvegarderCommand}" Content="Sauvegarder"/>
```

## ?? Créer un nouveau Module

### Étape 1 : Créer le Model
```csharp
// Models/Models.cs
public class MonObjet
{
    public int Id { get; set; }
    public string Nom { get; set; }
}
```

### Étape 2 : Créer le Repository
```csharp
// Repositories/Repositories.cs
public class MonObjetRepository
{
    public List<MonObjet> ObtenirTous()
    {
        var objets = new List<MonObjet>();
        string query = "SELECT * FROM mon_objet";
        
        using (var reader = DatabaseConnection.ExecuteQuery(query))
        {
            while (reader.Read())
            {
                objets.Add(new MonObjet
                {
                    Id = (int)reader["id"],
                    Nom = reader["nom"].ToString() ?? string.Empty
                });
            }
        }
        return objets;
    }
    
    public bool Ajouter(MonObjet objet)
    {
        string query = "INSERT INTO mon_objet (nom) VALUES (@nom)";
        var parameters = new Dictionary<string, object>
        {
            { "@nom", objet.Nom }
        };
        
        try
        {
            DatabaseConnection.ExecuteCommand(query, parameters);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
            return false;
        }
    }
}
```

### Étape 3 : Créer le ViewModel
```csharp
// ViewModels/MonObjetViewModel.cs
public class MonObjetViewModel : ViewModelBase
{
    private ObservableCollection<MonObjet> _objets;
    private MonObjet _monObjet;
    private MonObjetRepository _repository;

    public ObservableCollection<MonObjet> Objets
    {
        get => _objets;
        set => SetProperty(ref _objets, value, nameof(Objets));
    }

    public ICommand AjouterCommand { get; }
    public ICommand ChargerCommand { get; }

    public MonObjetViewModel()
    {
        _objets = new ObservableCollection<MonObjet>();
        _repository = new MonObjetRepository();
        _monObjet = new MonObjet();
        
        AjouterCommand = new RelayCommand(_ => PerformerAjout());
        ChargerCommand = new RelayCommand(_ => ChargerDonnees());
    }

    private void ChargerDonnees()
    {
        var objets = _repository.ObtenirTous();
        Objets = new ObservableCollection<MonObjet>(objets);
    }

    private void PerformerAjout()
    {
        if (_repository.Ajouter(_monObjet))
        {
            ChargerDonnees();
            _monObjet = new MonObjet();
        }
    }
}
```

### Étape 4 : Créer la Vue (XAML)
```xaml
<!-- Views/MonObjetWindow.xaml -->
<Window x:Class="FLEET_MANAGER.Views.MonObjetWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Ma Vue">
    <StackPanel Margin="20">
        <DataGrid ItemsSource="{Binding Objets}"/>
        
        <TextBox x:Name="NomTextBox" Margin="0,10,0,0"/>
        <Button Command="{Binding AjouterCommand}" Content="Ajouter" Margin="0,5,0,0"/>
    </StackPanel>
</Window>
```

### Étape 5 : Code-behind
```csharp
// Views/MonObjetWindow.xaml.cs
public partial class MonObjetWindow : Window
{
    public MonObjetWindow()
    {
        InitializeComponent();
        DataContext = new MonObjetViewModel();
    }
}
```

## ?? Binding en XAML

### Types de bindings
```xaml
<!-- OneWay : vue <- viewmodel -->
<TextBlock Text="{Binding Marque}"/>

<!-- TwoWay : vue <-> viewmodel -->
<TextBox Text="{Binding Marque, UpdateSourceTrigger=PropertyChanged}"/>

<!-- OneTime : vue <- viewmodel (une seule fois) -->
<TextBlock Text="{Binding Marque, Mode=OneTime}"/>

<!-- Avec convertisseur -->
<TextBlock Text="{Binding IsActif, Converter={StaticResource BoolToYesNoConverter}}"/>

<!-- Avec StringFormat -->
<TextBlock Text="{Binding Prix, StringFormat='€{0:F2}'}"/>

<!-- Command -->
<Button Command="{Binding SauvegarderCommand}" Content="Sauvegarder"/>

<!-- Command avec paramètre -->
<Button Command="{Binding SupprimerCommand}" CommandParameter="{Binding SelectedItem, ElementName=ListeVehicules}"/>
```

## ?? Travailler avec la Base de Données

### Récupérer des données
```csharp
string query = "SELECT * FROM vehicules WHERE etat = @etat";
var parameters = new Dictionary<string, object>
{
    { "@etat", "En service" }
};

using (var reader = DatabaseConnection.ExecuteQuery(query, parameters))
{
    while (reader.Read())
    {
        // Traiter les données
    }
}
```

### Modifier des données
```csharp
string query = "UPDATE vehicules SET kilométrage_actuel = @km WHERE id_vehicule = @id";
var parameters = new Dictionary<string, object>
{
    { "@id", 1 },
    { "@km", 50000 }
};

DatabaseConnection.ExecuteCommand(query, parameters);
```

### Exécuter un scalaire (COUNT, MAX, etc.)
```csharp
string query = "SELECT COUNT(*) FROM vehicules WHERE etat = 'En service'";
int count = (int)DatabaseConnection.ExecuteScalar(query);
```

## ?? Pattern de Validation

```csharp
public class VehiculeViewModel : ViewModelBase
{
    private string _marque = string.Empty;
    private string _marqueError = string.Empty;

    public string Marque
    {
        get => _marque;
        set
        {
            if (SetProperty(ref _marque, value, nameof(Marque)))
            {
                ValidateMarque();
            }
        }
    }

    public string MarqueError
    {
        get => _marqueError;
        set => SetProperty(ref _marqueError, value, nameof(MarqueError));
    }

    private void ValidateMarque()
    {
        if (string.IsNullOrWhiteSpace(Marque))
        {
            MarqueError = "La marque est requise.";
        }
        else if (Marque.Length < 2)
        {
            MarqueError = "La marque doit contenir au moins 2 caractères.";
        }
        else
        {
            MarqueError = string.Empty;
        }
    }
}
```

## ?? Debugging

### Afficher les informations en console
```csharp
System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
```

### Ajouter des points d'arrêt
1. Cliquer sur le numéro de ligne
2. Exécuter l'application
3. Le code s'arrêtera à ce point

### Vérifier les bindings
```xaml
<!-- Dans Output window, chercher les binding errors -->
System.Windows.Data Error: 40 : BindingExpression path error: 'MonObjet' property not found on 'object'
```

## ?? Ressources Utiles

- [Documentation MVVM](https://docs.microsoft.com/en-us/dotnet/architecture/maui/mvvm)
- [WPF Data Binding](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/data-binding-overview)
- [MySQL Connector](https://dev.mysql.com/doc/connector-net/en/)
- [C# 12 Features](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12)

## ? Checklist pour un nouveau Feature

- [ ] Créer le Model
- [ ] Créer le Repository avec les opérations CRUD
- [ ] Créer le ViewModel avec les commandes
- [ ] Créer la Vue XAML
- [ ] Créer le code-behind
- [ ] Ajouter les validations
- [ ] Tester les bindings
- [ ] Tester les opérations BD
- [ ] Documenter le code
- [ ] Tester en cas d'erreur

---

**Bon développement ! ??**
