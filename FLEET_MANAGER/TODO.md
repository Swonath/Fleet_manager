# TODO - Fleet Manager

## Phase 1 : Infrastructure (? Complétée)
- [x] Structure MVVM
- [x] Connexion MySQL
- [x] Configuration de la base de données
- [x] Modèles de données
- [x] Repositories d'accès aux données
- [x] Authentification

## Phase 2 : Modules de Base
- [ ] **Module Gestion des Véhicules**
  - [ ] Vue d'ajout de véhicule
  - [ ] Vue de modification de véhicule
  - [ ] Vue de suppression de véhicule
  - [ ] Validation des données
  - [ ] Recherche et filtrage

- [ ] **Module Suivi du Carburant**
  - [ ] Vue d'enregistrement de ravitaillement
  - [ ] Historique des ravitaillements
  - [ ] Calcul de consommation
  - [ ] Graphiques de consommance
  - [ ] Alertes de consommation anormale

- [ ] **Module Gestion des Trajets**
  - [ ] Vue d'enregistrement de trajet
  - [ ] Historique des trajets
  - [ ] Calcul de distance
  - [ ] Filtrage par type de trajet
  - [ ] Statistiques de trajets

## Phase 3 : Fonctionnalités Avancées
- [ ] **Module Gestion des Maintenances**
  - [ ] Vue d'ajout de maintenance
  - [ ] Historique des maintenances
  - [ ] Alertes de maintenance à venir
  - [ ] Prévisions d'entretien
  - [ ] Fournisseurs et contacts

- [ ] **Module Gestion des Utilisateurs** (Admin uniquement)
  - [ ] Vue de gestion des utilisateurs
  - [ ] Création de nouvel utilisateur
  - [ ] Modification des rôles
  - [ ] Désactivation d'utilisateur
  - [ ] Journalisation des actions

- [ ] **Module Rapports**
  - [ ] Rapport de consommation
  - [ ] Rapport de coûts
  - [ ] Rapport d'utilisation
  - [ ] Rapport de maintenance
  - [ ] Export PDF
  - [ ] Export Excel

## Phase 4 : Amélioration UX/UI
- [ ] Amélioration du design du tableau de bord
- [ ] Graphiques et visualisations
- [ ] Navigation fluide entre les modules
- [ ] Responsive design
- [ ] Thème clair/sombre
- [ ] Icônes et images

## Phase 5 : Optimisations et Sécurité
- [ ] Chiffrement des mots de passe
- [ ] Validation des entrées
- [ ] Gestion des erreurs complète
- [ ] Logging complet
- [ ] Optimisation des requêtes
- [ ] Gestion des transactions
- [ ] Sauvegarde automatique

## Phase 6 : Tests
- [ ] Tests unitaires
- [ ] Tests d'intégration
- [ ] Tests de performance
- [ ] Tests de sécurité
- [ ] Validation des scénarios utilisateur

## Phase 7 : Documentation
- [ ] API documentation
- [ ] Schéma de base de données
- [ ] Guide utilisateur
- [ ] Manuel d'administration
- [ ] Diagrammes UML
- [ ] Rapport de projet

## Améliorations Futures
- [ ] Version web (ASP.NET Core)
- [ ] Application mobile (MAUI)
- [ ] Synchronisation cloud
- [ ] Intégration GPS
- [ ] API REST externe
- [ ] Notification par email
- [ ] Dashboard temps réel

---

## Priorités
1. ?? CRITIQUE : Module gestion véhicules
2. ?? CRITIQUE : Module suivi carburant
3. ?? HAUTE : Module gestion utilisateurs
4. ?? HAUTE : Module rapports
5. ?? MOYENNE : Améliorations UI
6. ?? BASSE : Fonctionnalités bonus

---

## Notes
- Utiliser des transactions pour les opérations multi-tables
- Implémenter le pattern Repository pour toutes les opérations BD
- Ajouter des validations côté client et serveur
- Générer des tests unitaires
- Documenter le code avec des commentaires XML

---

**Dernière mise à jour** : 2025-11-03
