# 📚 MANUEL D'UTILISATION - FLEET MANAGER

## Guide Complet de l'Application de Gestion de Flotte

**Version :** 1.0
**Date :** Janvier 2025
**Application :** Fleet Manager - Système de Gestion de Flotte de Véhicules

---

## 📖 Table des Matières

1. [Introduction](#introduction)
2. [Installation et Configuration](#installation-et-configuration)
3. [Connexion à l'Application](#connexion)
4. [Rôles et Permissions](#roles-et-permissions)
5. [Navigation dans l'Interface](#navigation)
6. [Module Tableau de Bord](#module-dashboard)
7. [Module Véhicules](#module-vehicules)
8. [Module Carburant et Trajets](#module-carburant-trajets)
9. [Module Statistiques](#module-statistiques)
10. [Module Utilisateurs (Admin)](#module-utilisateurs)
11. [Déconnexion](#deconnexion)
12. [Dépannage](#depannage)

---

## 🎯 Introduction

### Qu'est-ce que Fleet Manager ?

Fleet Manager est une application de gestion de flotte de véhicules qui permet de :

- ✅ **Suivre votre parc automobile** : Gérez tous vos véhicules en un seul endroit
- ⛽ **Monitorer la consommation de carburant** : Enregistrez et analysez les ravitaillements
- 🚗 **Tracer les trajets** : Documentez tous les déplacements professionnels
- 📊 **Visualiser des statistiques** : Analysez les coûts et l'utilisation
- 👥 **Gérer les utilisateurs** : Contrôlez les accès et les permissions

### À qui s'adresse cette application ?

- **Entreprises** avec une flotte de véhicules
- **Gestionnaires de parc automobile**
- **Responsables logistiques**
- **Services comptables** (suivi des coûts)

---

## 💻 Installation et Configuration

### Prérequis

- **Système d'exploitation :** Windows 10/11 (64-bit)
- **Espace disque :** Minimum 500 MB
- **Mémoire RAM :** Minimum 2 GB
- **Droits d'administration :** Recommandés pour la première installation

### Installation - Première Utilisation

#### ✅ Installation Simplifiée

Fleet Manager est distribué comme une **application standalone** qui inclut tout ce dont vous avez besoin :
- ✅ Base de données intégrée (pas besoin d'installer MySQL)
- ✅ .NET Runtime inclus
- ✅ Configuration automatique

**Étapes d'installation :**

1. **Téléchargez** le fichier **FLEET_MANAGER.exe**
2. **Double-cliquez** sur le fichier pour lancer l'application
3. **Lors du premier lancement**, l'application initialise automatiquement :
   - La base de données locale
   - Les tables nécessaires
   - Le compte administrateur par défaut

**C'est tout ! L'application est prête à être utilisée.**

### ⚙️ Configuration Avancée (Optionnelle)

Par défaut, l'application fonctionne **sans aucune configuration**.

Si vous souhaitez utiliser une base de données MySQL externe (déploiement multi-postes) :

1. Localisez le fichier **App.config.example** dans le dossier de l'application
2. Copiez-le et renommez la copie en **App.config**
3. Ouvrez **App.config** avec un éditeur de texte
4. Modifiez les paramètres de connexion :

```xml
<appSettings>
    <add key="DB_Server" value="votre_serveur" />
    <add key="DB_Database" value="fleet_manager" />
    <add key="DB_UserId" value="votre_utilisateur" />
    <add key="DB_Password" value="votre_mot_de_passe" />
    <add key="DB_Port" value="3306" />
</appSettings>
```

### ⚠️ Important - Sécurité

- La base de données est stockée localement dans le dossier de l'application
- **Sauvegardez régulièrement** vos données (voir section Sauvegarde ci-dessous)
- Ne partagez jamais le fichier **App.config** s'il contient des credentials

### 💾 Sauvegarde et Restauration

#### Sauvegarder vos données

Les données de l'application sont stockées dans le dossier :
```
FLEET_MANAGER/Data/
```

**Pour faire une sauvegarde :**
1. Fermez l'application
2. Copiez le dossier **Data/** dans un emplacement sûr
3. Renommez la copie avec la date (ex: `Data_2025-01-27`)

**Fréquence recommandée :** Hebdomadaire ou après chaque grosse saisie

#### Restaurer vos données

1. Fermez l'application
2. Remplacez le dossier **Data/** par votre sauvegarde
3. Relancez l'application

---

## 🔐 Connexion à l'Application

### Première Connexion

L'application dispose d'un compte administrateur par défaut :

```
Nom d'utilisateur : admin
Mot de passe : [défini lors de l'installation]
```

### Processus de Connexion

1. **Lancez l'application** FLEET_MANAGER.exe
2. **Saisissez vos identifiants** :
   - Nom d'utilisateur
   - Mot de passe
3. **Cliquez sur "Se connecter"**

### Messages d'Erreur Possibles

| Message | Signification | Solution |
|---------|--------------|----------|
| "Nom d'utilisateur ou mot de passe incorrect" | Identifiants invalides | Vérifiez vos identifiants |
| "Erreur de connexion à la base de données" | MySQL non démarré | Démarrez MySQL |
| "Une erreur est survenue..." | Erreur système | Contactez l'administrateur |

### Exigences du Mot de Passe

Lors de la création d'un compte, le mot de passe doit contenir :

- ✅ **Minimum 8 caractères**
- ✅ **Au moins 1 lettre majuscule** (A-Z)
- ✅ **Au moins 1 lettre minuscule** (a-z)
- ✅ **Au moins 1 chiffre** (0-9)

**Exemple de mot de passe valide :** `MonMotDePasse123`

---

## 👤 Rôles et Permissions

### Types d'Utilisateurs

#### 🔵 Utilisateur Standard

**Permissions :**
- ✅ Voir le tableau de bord
- ✅ Consulter les véhicules
- ✅ Enregistrer des pleins et trajets
- ✅ Voir les statistiques
- ❌ Gérer les utilisateurs
- ❌ Modifier les véhicules
- ❌ Supprimer des données

**Cas d'usage :** Chauffeurs, employés terrain

#### 🟡 Administrateur

**Permissions :**
- ✅ Toutes les permissions d'un utilisateur standard
- ✅ Créer/modifier/supprimer des véhicules
- ✅ Supprimer des pleins et trajets
- ✅ Gérer les utilisateurs standards
- ❌ Modifier un super administrateur
- ❌ Créer un super administrateur

**Cas d'usage :** Gestionnaires de flotte, responsables

#### 🔴 Super Administrateur

**Permissions :**
- ✅ **TOUTES** les permissions
- ✅ Gérer tous les utilisateurs (y compris les admins)
- ✅ Créer des super administrateurs
- ✅ Accès complet au système

**Cas d'usage :** Direction, IT

### Matrice des Permissions

| Action | Utilisateur | Admin | Super Admin |
|--------|------------|-------|-------------|
| Voir dashboard | ✅ | ✅ | ✅ |
| Consulter véhicules | ✅ | ✅ | ✅ |
| Ajouter véhicule | ❌ | ✅ | ✅ |
| Modifier véhicule | ❌ | ✅ | ✅ |
| Supprimer véhicule | ❌ | ✅ | ✅ |
| Enregistrer carburant | ✅ | ✅ | ✅ |
| Supprimer carburant | ❌ | ✅ | ✅ |
| Enregistrer trajet | ✅ | ✅ | ✅ |
| Supprimer trajet | ❌ | ✅ | ✅ |
| Voir statistiques | ✅ | ✅ | ✅ |
| Gérer utilisateurs | ❌ | ✅ | ✅ |
| Créer admin | ❌ | ❌ | ✅ |
| Modifier admin | ❌ | ❌ | ✅ |

---

## 🧭 Navigation dans l'Interface

### Structure de l'Interface

L'application est divisée en **2 zones principales** :

```
┌─────────────────────────────────────────┐
│         Barre de Navigation             │
│  (Menu latéral gauche)                  │
├─────────────────────────────────────────┤
│                                         │
│                                         │
│         Zone de Contenu                 │
│         (Zone principale)               │
│                                         │
│                                         │
└─────────────────────────────────────────┘
```

### Menu de Navigation

Le menu latéral gauche contient :

1. **📊 Tableau de bord** - Vue d'ensemble
2. **🚗 Véhicules** - Gestion du parc
3. **⛽ Carburant & Trajets** - Enregistrements
4. **📈 Statistiques** - Analyses
5. **👥 Utilisateurs** - Gestion des accès *(Admin uniquement)*
6. **🚪 Déconnexion** - Quitter l'application

### Bouton Actif

Le bouton de la page actuelle est **surligné en bleu** pour vous repérer facilement.

---

## 📊 Module Tableau de Bord

### Vue d'Ensemble

Le tableau de bord offre une **vue synthétique** de votre flotte en temps réel.

### Sections du Tableau de Bord

#### 1️⃣ Statistiques Générales (En haut)

Affiche 4 indicateurs clés :

| Indicateur | Description |
|-----------|-------------|
| **Total Véhicules** | Nombre total de véhicules dans le parc |
| **En Service** | Véhicules actuellement en utilisation |
| **En Maintenance** | Véhicules en réparation/entretien |
| **Disponibles** | Véhicules prêts à être utilisés |

**Interprétation :**
- ✅ Idéal : Taux de disponibilité > 80%
- ⚠️ Attention : > 30% en maintenance
- 🚨 Critique : < 50% disponibles

#### 2️⃣ Coût Total du Carburant

Affiche le **coût cumulé** de tous les ravitaillements enregistrés.

**Calcul :**
```
Coût Total = Σ (Tous les pleins de tous les véhicules)
```

#### 3️⃣ Activité Récente

Liste des **10 dernières activités** (pleins et trajets) :

- 🔵 **Pleins** : Icône carburant
- 🟢 **Trajets** : Icône trajet

**Informations affichées :**
- Date et heure
- Véhicule concerné (Marque + Immatriculation)
- Détails (litres pour pleins, destination pour trajets)

#### 4️⃣ Liste des Véhicules

Affiche tous les véhicules avec **pagination** (12 par page).

**Pour chaque véhicule :**
- Photo (si disponible)
- Marque et modèle
- Immatriculation
- État actuel (badge coloré)
- Type de carburant

**Actions possibles :**
- **Cliquer sur un véhicule** : Ouvre la fiche détaillée

**Recherche de véhicules :**
- Utilisez la barre de recherche en haut
- Recherche par : marque, modèle, immatriculation, état, type carburant

### Rafraîchissement des Données

Les données du tableau de bord sont **automatiquement chargées** à l'ouverture et après chaque modification (ajout de véhicule, enregistrement, etc.).

---

## 🚗 Module Véhicules

### Accès au Module

Cliquez sur **"🚗 Véhicules"** dans le menu latéral.

### Vue Liste des Véhicules

#### Barre d'Outils

En haut de la page :

1. **Barre de recherche** : Filtrer les véhicules
2. **Bouton "Nouveau Véhicule"** *(Admin uniquement)* : Ajouter un véhicule

#### Filtres

- **Marque** (ex: Renault, Peugeot, Mercedes)
- **Modèle** (ex: Clio, 308, Vito)
- **Immatriculation** (ex: AB-123-CD)
- **État** (En service, Disponible, En maintenance)
- **Type de carburant** (Essence, Diesel, Électrique, Hybride)

#### Affichage des Véhicules

Chaque véhicule est affiché sous forme de **carte** avec :

- **Photo du véhicule** (ou placeholder)
- **Marque et Modèle**
- **Immatriculation**
- **Badge d'état** (coloré selon l'état)
- **Type de carburant**

**Cliquez sur un véhicule** pour voir sa fiche complète.

### Fiche Détaillée d'un Véhicule

#### Onglet "Informations"

Affiche toutes les informations du véhicule :

**Identification :**
- Marque
- Modèle
- Immatriculation
- Année de mise en circulation

**Caractéristiques :**
- Type de carburant
- Kilométrage actuel
- État actuel
- Numéro de châssis
- Date d'acquisition
- Notes

**Actions disponibles :**
- 🔵 **Modifier** *(Admin uniquement)* : Éditer les informations
- 🔴 **Supprimer** *(Admin uniquement)* : Supprimer le véhicule

#### Onglet "Historique"

Liste complète de l'**historique des pleins et trajets** du véhicule.

**Filtres :**
- **Tout** : Affiche pleins ET trajets mélangés
- **Pleins** : Uniquement les ravitaillements
- **Trajets** : Uniquement les déplacements

**Affichage :**
- Date
- Type (Plein ou Trajet)
- Détails
- Icône d'action (🗑️ pour supprimer - *Admin uniquement*)

**Pagination :**
- 12 éléments par page
- Bouton "Voir plus" pour charger la suite

### Ajouter un Véhicule (Admin)

1. **Cliquez** sur "Nouveau Véhicule"
2. **Remplissez le formulaire** :

**Champs obligatoires :**
- ✅ Marque (ex: Renault)
- ✅ Modèle (ex: Clio)
- ✅ Immatriculation (ex: AB-123-CD)
- ✅ Année (ex: 2023)
- ✅ Type de carburant (sélection liste)
- ✅ État (sélection liste)
- ✅ Kilométrage actuel (ex: 45000)

**Champs optionnels :**
- Numéro de châssis
- Date d'acquisition
- Notes

3. **Cliquez** sur "Enregistrer"

**États possibles :**
- **En service** : Véhicule en utilisation
- **Disponible** : Véhicule prêt à être utilisé
- **En maintenance** : Véhicule en réparation

**Types de carburant :**
- Essence
- Diesel
- Électrique
- Hybride
- GPL

### Modifier un Véhicule (Admin)

1. **Ouvrez** la fiche du véhicule
2. **Cliquez** sur "Modifier"
3. **Modifiez** les champs souhaités
4. **Cliquez** sur "Enregistrer"

### Supprimer un Véhicule (Admin)

1. **Ouvrez** la fiche du véhicule
2. **Cliquez** sur "Supprimer"
3. **Confirmez** la suppression

⚠️ **Attention :** La suppression est **définitive** et supprime aussi :
- Tous les pleins du véhicule
- Tous les trajets du véhicule
- L'historique complet

---

## ⛽ Module Carburant & Trajets

### Accès au Module

Cliquez sur **"⛽ Carburant & Trajets"** dans le menu latéral.

### Sélection du Véhicule

**Avant tout enregistrement**, vous devez sélectionner un véhicule :

1. Utilisez le **menu déroulant** en haut
2. Sélectionnez le véhicule concerné

### Onglet "Carburant"

Permet d'enregistrer un **ravitaillement en carburant**.

#### Formulaire de Saisie

**Champs à remplir :**

| Champ | Description | Exemple |
|-------|-------------|---------|
| **Date de saisie** | Date du plein | 15/01/2025 |
| **Quantité (L)** | Litres ravitaillés | 45.5 |
| **Coût total (€)** | Montant payé | 75.50 |
| **Coût au litre (€)** | Prix du litre (calculé auto) | 1.66 |
| **Kilométrage** | Km au moment du plein | 45250 |
| **Notes** | Observations (optionnel) | Plein complet |

**Calcul automatique :**
Le **coût par litre** est calculé automatiquement :
```
Coût par litre = Coût total ÷ Quantité
```

#### Enregistrer un Plein

1. **Sélectionnez** le véhicule
2. **Remplissez** le formulaire
3. **Cliquez** sur "Enregistrer le carburant"
4. **Confirmation** : Message de succès

#### Historique des Pleins

En bas de l'onglet, vous voyez l'**historique** du véhicule sélectionné :

- Date
- Litres
- Coût total
- Kilométrage
- Notes
- **🗑️ Supprimer** *(Admin uniquement)*

**Pagination :** 12 pleins par page

### Onglet "Trajet"

Permet d'enregistrer un **trajet effectué** avec le véhicule.

#### Formulaire de Saisie

**Informations du trajet :**

| Champ | Description | Exemple |
|-------|-------------|---------|
| **Date du trajet** | Date du déplacement | 15/01/2025 |
| **Heure de départ** | Heure de départ | 09:00 |
| **Heure d'arrivée** | Heure d'arrivée | 11:30 |
| **Lieu de départ** | Ville/Adresse départ | Paris |
| **Lieu d'arrivée** | Ville/Adresse arrivée | Lyon |
| **Km départ** | Kilométrage au départ | 45250 |
| **Km arrivée** | Kilométrage à l'arrivée | 45720 |
| **Distance (km)** | Distance (calculée auto) | 470 |
| **Type de trajet** | Professionnel/Personnel | Professionnel |
| **Notes** | Observations (optionnel) | Réunion client |

**Calcul automatique :**
La **distance** est calculée automatiquement :
```
Distance = Km arrivée - Km départ
```

**Types de trajet :**
- **Professionnel** : Déplacement professionnel
- **Personnel** : Usage personnel

#### Enregistrer un Trajet

1. **Sélectionnez** le véhicule
2. **Remplissez** le formulaire
3. **Cliquez** sur "Enregistrer le trajet"
4. **Confirmation** : Message de succès

#### Historique des Trajets

En bas de l'onglet, vous voyez l'**historique** :

- Date
- Départ
- Arrivée
- Distance
- Notes
- **🗑️ Supprimer** *(Admin uniquement)*

**Pagination :** 12 trajets par page

### Onglet "Historique Global"

Affiche l'**historique combiné** de **TOUS les véhicules**.

**Filtres :**
- **Tout** : Pleins + Trajets
- **Pleins uniquement**
- **Trajets uniquement**

**Utilité :**
- Vue d'ensemble de toute l'activité
- Recherche d'un enregistrement spécifique
- Audit et contrôle

**Pagination :** 12 éléments par page

### Supprimer un Enregistrement (Admin)

1. **Cliquez** sur l'icône 🗑️ à côté de l'enregistrement
2. **Confirmez** la suppression

⚠️ **La suppression est définitive et irréversible.**

---

## 📈 Module Statistiques

### Accès au Module

Cliquez sur **"📈 Statistiques"** dans le menu latéral.

### Statistiques Disponibles

#### 1️⃣ Statistiques Globales

**Indicateurs clés :**

- **Total des véhicules**
- **Coût total carburant** (tous véhicules)
- **Consommation moyenne** (L/100km)
- **Distance totale parcourue** (km)

#### 2️⃣ Statistiques par Véhicule

**Sélection du véhicule :**
- Menu déroulant en haut de page
- Sélectionnez un véhicule pour voir ses stats

**Indicateurs par véhicule :**

| Statistique | Description |
|------------|-------------|
| **Coût total carburant** | Somme de tous les pleins |
| **Litres totaux** | Quantité totale ravitaillée |
| **Consommation moyenne** | L/100km calculé |
| **Distance totale** | Somme de tous les trajets |
| **Nombre de pleins** | Nombre de ravitaillements |
| **Nombre de trajets** | Nombre de déplacements |

**Formule de consommation :**
```
Consommation (L/100km) = (Litres totaux ÷ Distance totale) × 100
```

#### 3️⃣ Graphiques et Visualisations

**Graphiques disponibles :**

1. **Évolution du coût carburant** (chronologique)
2. **Répartition par type de carburant**
3. **Top 5 véhicules** (par coût)
4. **Évolution de la distance** (par mois)

**Période d'analyse :**
- Filtrer par mois
- Filtrer par année
- Vue personnalisée

### Exports

**Formats disponibles :**
- 📄 PDF : Rapport complet
- 📊 Excel : Données brutes

**Contenu de l'export :**
- Tous les indicateurs
- Tableaux de données
- Graphiques
- Période d'analyse

---

## 👥 Module Utilisateurs (Admin)

### Accès au Module

⚠️ **Réservé aux Administrateurs**

Cliquez sur **"👥 Utilisateurs"** dans le menu latéral.

Si vous n'êtes pas administrateur, ce menu est **masqué**.

### Vue Liste des Utilisateurs

Affiche tous les utilisateurs du système.

**Pour chaque utilisateur :**
- Nom complet (Prénom + Nom)
- Nom d'utilisateur
- Email
- Rôle (badge coloré)
- Date de création
- Statut (Actif/Inactif)

**Actions disponibles :**
- ✏️ **Modifier** : Éditer l'utilisateur
- 🗑️ **Supprimer** : Supprimer l'utilisateur

### Recherche d'Utilisateurs

Utilisez la **barre de recherche** pour filtrer par :
- Nom
- Prénom
- Email
- Nom d'utilisateur

### Ajouter un Utilisateur

1. **Cliquez** sur "Nouvel Utilisateur"
2. **Remplissez le formulaire** :

**Champs obligatoires :**
- ✅ Nom d'utilisateur (3-50 caractères, alphanumérique)
- ✅ Email (format valide : xxx@xxx.xxx)
- ✅ Mot de passe (voir exigences ci-dessous)
- ✅ Nom
- ✅ Prénom
- ✅ Rôle (sélection liste)

**Exigences du mot de passe :**
- Minimum 8 caractères
- Au moins 1 majuscule
- Au moins 1 minuscule
- Au moins 1 chiffre

**Rôles disponibles (selon vos droits) :**
- **Utilisateur** : Accès basique
- **Administrateur** : Accès étendu *(Admin peut créer)*
- **Super Administrateur** : Accès complet *(Seul Super Admin peut créer)*

3. **Cliquez** sur "Enregistrer"

### Modifier un Utilisateur

1. **Cliquez** sur ✏️ à côté de l'utilisateur
2. **Modifiez** les champs souhaités
3. **Changez le mot de passe** (optionnel - laisser vide pour conserver l'ancien)
4. **Cliquez** sur "Enregistrer"

### Supprimer un Utilisateur

1. **Cliquez** sur 🗑️ à côté de l'utilisateur
2. **Confirmez** la suppression

**Règles de suppression :**
- ❌ Impossible de supprimer son propre compte
- ❌ Admin ne peut pas supprimer un autre Admin
- ❌ Admin ne peut pas supprimer un Super Admin
- ✅ Super Admin peut tout supprimer

### Restrictions de Permissions

**Ce qu'un Administrateur PEUT faire :**
- ✅ Créer des utilisateurs standards
- ✅ Modifier des utilisateurs standards
- ✅ Modifier son propre compte
- ✅ Supprimer des utilisateurs standards

**Ce qu'un Administrateur NE PEUT PAS faire :**
- ❌ Créer un administrateur
- ❌ Créer un super administrateur
- ❌ Modifier un autre administrateur
- ❌ Modifier un super administrateur
- ❌ Supprimer un administrateur
- ❌ Supprimer un super administrateur

**Ce qu'un Super Administrateur peut faire :**
- ✅ **TOUT** sans restriction

### Messages d'Erreur

| Message | Signification |
|---------|--------------|
| "Vous n'avez pas les droits pour créer un admin" | Seul Super Admin peut créer des admins |
| "Format d'email invalide" | Email mal formaté |
| "Le mot de passe doit contenir..." | Politique de mot de passe non respectée |
| "Le nom d'utilisateur doit contenir..." | Format invalide |

---

## 🚪 Déconnexion

### Se Déconnecter

1. **Cliquez** sur "🚪 Déconnexion" dans le menu latéral (en bas)
2. **Confirmation** : Vous êtes redirigé vers l'écran de connexion

### Sécurité

- Toujours **se déconnecter** en fin de session
- Ne laissez **jamais** une session ouverte sans surveillance
- Sur un ordinateur partagé, **fermer l'application complètement**

---

## 🔧 Dépannage

### Problèmes de Connexion

#### "Impossible de se connecter à la base de données"

**Causes possibles :**
1. Base de données locale corrompue
2. Fichier de base de données manquant
3. Droits d'accès insuffisants sur le dossier
4. App.config mal configuré (si vous utilisez une base externe)

**Solutions :**
1. **Vérifiez le dossier Data/** : Assurez-vous qu'il existe dans le dossier de l'application
2. **Droits d'accès** : Clic droit sur le dossier FLEET_MANAGER > Propriétés > Sécurité > Vérifiez que vous avez les droits en lecture/écriture
3. **Restaurez une sauvegarde** : Si vous avez une sauvegarde, restaurez-la
4. **Réinstallez** : En dernier recours, téléchargez à nouveau l'application
5. **Base externe** : Si vous utilisez une base MySQL externe, vérifiez les credentials dans App.config

#### "Nom d'utilisateur ou mot de passe incorrect"

**Solutions :**
- Vérifiez vos identifiants (attention à la casse)
- Contactez votre administrateur pour réinitialiser votre mot de passe
- Si vous êtes le premier utilisateur, utilisez les identifiants par défaut fournis lors de l'installation

#### "L'application ne démarre pas"

**Causes possibles :**
1. Fichier .exe corrompu
2. Antivirus bloque l'application
3. .NET Runtime manquant

**Solutions :**
1. **Retéléchargez** FLEET_MANAGER.exe
2. **Antivirus** : Ajoutez une exception pour FLEET_MANAGER.exe
3. **Droits administrateur** : Clic droit sur l'exe > Exécuter en tant qu'administrateur

### Problèmes d'Affichage

#### "Les véhicules ne s'affichent pas"

**Solutions :**
- Vérifiez que des véhicules existent
- Videz les filtres de recherche
- Rechargez la page (retournez au dashboard puis revenez)

#### "L'historique est vide"

**Cause :** Aucun enregistrement pour ce véhicule

**Solution :** Enregistrez des pleins et trajets

### Problèmes de Performance

#### "L'application est lente"

**Causes possibles :**
1. Base de données volumineuse (> 10 000 enregistrements)
2. Ressources système insuffisantes
3. Disque dur saturé
4. Trop d'applications ouvertes

**Solutions :**
1. **Nettoyez les anciens enregistrements** : Archivez ou supprimez les données anciennes
2. **Libérez de l'espace disque** : Minimum 1 GB d'espace libre recommandé
3. **Fermez les applications inutiles** : Libérez de la RAM
4. **Vérifiez les performances** : Gestionnaire des tâches > Onglet Performances
5. **Défragmentez** : Si vous utilisez un disque dur mécanique (HDD)

### Messages d'Erreur Génériques

#### "Une erreur est survenue"

**Action :**
1. Notez ce que vous faisiez
2. Réessayez l'opération
3. Si le problème persiste, contactez le support

---

## 📞 Support et Contact

### Obtenir de l'Aide

**En cas de problème :**

1. **Consultez ce manuel** : La plupart des questions y trouvent réponse
2. **Contactez votre administrateur** : Pour les problèmes de compte
3. **Support technique** : Pour les bugs et erreurs

### Informations à Fournir

Lorsque vous contactez le support, fournissez :

- ✅ Version de l'application
- ✅ Système d'exploitation (Windows 10/11)
- ✅ Description du problème
- ✅ Message d'erreur exact (si applicable)
- ✅ Étapes pour reproduire le problème

### Mises à Jour

**Vérifier les mises à jour :**
- Consultez régulièrement le changelog
- Les mises à jour corrigent les bugs et ajoutent des fonctionnalités

---

## 📝 Bonnes Pratiques

### Utilisation Quotidienne

1. **Enregistrez régulièrement** les pleins et trajets
2. **Vérifiez les kilomètres** avant chaque enregistrement
3. **Ajoutez des notes** pour faciliter le suivi
4. **Mettez à jour l'état** des véhicules (maintenance, etc.)

### Gestion de Flotte

1. **Contrôlez régulièrement** les statistiques
2. **Identifiez** les véhicules coûteux
3. **Planifiez** les maintenances préventives
4. **Exportez** les données mensuellement

### Sécurité

1. **Changez** votre mot de passe régulièrement
2. **Ne partagez jamais** vos identifiants
3. **Déconnectez-vous** systématiquement
4. **Sauvegardez** la base de données hebdomadairement

---

## 📖 Glossaire

| Terme | Définition |
|-------|------------|
| **Plein** | Ravitaillement en carburant d'un véhicule |
| **Trajet** | Déplacement effectué avec un véhicule |
| **Dashboard** | Tableau de bord, vue d'ensemble |
| **Pagination** | Affichage des données par pages |
| **Credentials** | Identifiants de connexion |
| **Admin** | Administrateur |
| **Super Admin** | Super Administrateur |
| **Repository** | Base de données, stockage |

---

## 📄 Annexes

### Raccourcis Clavier

*(À implémenter dans une future version)*

### Formats de Données

**Dates :** JJ/MM/AAAA (ex: 15/01/2025)
**Heures :** HH:MM (ex: 09:30)
**Nombres décimaux :** Virgule ou point (ex: 45,5 ou 45.5)

### Limites du Système

- **Véhicules :** Illimité (limité par l'espace disque)
- **Utilisateurs :** Illimité (limité par l'espace disque)
- **Enregistrements :** Illimité (limité par l'espace disque)
- **Taille base de données :** Illimitée (limité par l'espace disque disponible)
- **Performance optimale :** Recommandé jusqu'à 5 000 véhicules et 100 000 enregistrements par véhicule

---

## 🎓 Formation

### Pour les Nouveaux Utilisateurs

**Programme de formation recommandé :**

1. **Jour 1 - Découverte** (1h)
   - Connexion
   - Navigation
   - Dashboard

2. **Jour 2 - Utilisation basique** (1h)
   - Consulter les véhicules
   - Enregistrer un plein
   - Enregistrer un trajet

3. **Jour 3 - Fonctions avancées** (1h)
   - Statistiques
   - Recherche et filtres
   - Exports

### Pour les Administrateurs

**Programme de formation recommandé :**

1. **Installation et déploiement** (20min)
   - Installation de l'application
   - Premier lancement
   - Configuration optionnelle (base externe)

2. **Gestion des utilisateurs** (1h)
   - Créer des utilisateurs
   - Attribuer les rôles
   - Gérer les permissions
   - Réinitialiser les mots de passe

3. **Gestion de la flotte** (45min)
   - Ajouter/modifier des véhicules
   - Suivre l'activité
   - Générer des statistiques

4. **Maintenance et support** (45min)
   - Sauvegardes régulières
   - Restauration de données
   - Dépannage courant
   - Support utilisateurs

---

## 📅 Changelog

### Version 1.0 - Janvier 2025

**Fonctionnalités initiales :**
- ✅ Gestion de véhicules
- ✅ Enregistrement carburant et trajets
- ✅ Statistiques
- ✅ Gestion d'utilisateurs
- ✅ Tableau de bord
- ✅ Système de permissions
- ✅ Authentification sécurisée (BCrypt)
- ✅ Validation des données
- ✅ Pagination
- ✅ Recherche et filtres
- ✅ Historique global

---

**FIN DU MANUEL UTILISATEUR**

---

*Ce document est fourni à titre informatif. Les captures d'écran sont à venir dans une future mise à jour.*

*Pour toute question ou suggestion d'amélioration de ce manuel, contactez votre administrateur système.*
