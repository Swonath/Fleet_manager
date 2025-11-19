-- ============================================
-- Exemples de requêtes utiles pour Fleet Manager
-- ============================================

-- 1. Récupérer tous les véhicules avec leurs statistiques
SELECT 
    v.id_vehicule,
    v.marque,
    v.modele,
    v.immatriculation,
    v.etat,
    COUNT(DISTINCT c.id_carburant) as nb_ravitaillements,
    COUNT(DISTINCT t.id_trajet) as nb_trajets,
    COALESCE(SUM(t.distance_parcourue), 0) as distance_totale,
    COALESCE(SUM(c.cout_total), 0) as cout_total_carburant
FROM vehicules v
LEFT JOIN carburants c ON v.id_vehicule = c.id_vehicule
LEFT JOIN trajets t ON v.id_vehicule = t.id_vehicule
GROUP BY v.id_vehicule
ORDER BY v.marque, v.modele;

-- 2. Consommation moyenne par véhicule
SELECT 
    v.marque,
    v.modele,
    v.immatriculation,
    AVG(t.distance_parcourue / NULLIF(c.quantite_litres, 0)) as consommation_moyenne_km_l
FROM vehicules v
JOIN trajets t ON v.id_vehicule = t.id_vehicule
JOIN carburants c ON v.id_vehicule = c.id_vehicule
GROUP BY v.id_vehicule
ORDER BY consommation_moyenne_km_l DESC;

-- 3. Coûts totaux par mois
SELECT 
    DATE_FORMAT(c.date_saisie, '%Y-%m') as mois,
    SUM(c.cout_total) as cout_total_mois,
    COUNT(DISTINCT c.id_vehicule) as nb_vehicules
FROM carburants c
GROUP BY DATE_FORMAT(c.date_saisie, '%Y-%m')
ORDER BY mois DESC;

-- 4. Maintenances à venir (prochains 30 jours)
SELECT 
    m.id_maintenance,
    v.marque,
    v.modele,
    v.immatriculation,
    m.type_maintenance,
    m.date_prochaine_maintenance,
    DATEDIFF(m.date_prochaine_maintenance, NOW()) as jours_restants
FROM maintenances m
JOIN vehicules v ON m.id_vehicule = v.id_vehicule
WHERE m.date_prochaine_maintenance IS NOT NULL
AND m.date_prochaine_maintenance <= DATE_ADD(NOW(), INTERVAL 30 DAY)
ORDER BY m.date_prochaine_maintenance ASC;

-- 5. Activités des utilisateurs (dernier mois)
SELECT 
    u.nom_utilisateur,
    COUNT(DISTINCT c.id_carburant) as carburants_saisis,
    COUNT(DISTINCT t.id_trajet) as trajets_enregistres,
    COUNT(DISTINCT m.id_maintenance) as maintenances_ajoutees
FROM utilisateurs u
LEFT JOIN carburants c ON u.id_utilisateur = c.id_utilisateur 
    AND DATE(c.date_creation) >= DATE_SUB(NOW(), INTERVAL 1 MONTH)
LEFT JOIN trajets t ON u.id_utilisateur = t.id_utilisateur 
    AND DATE(t.date_creation) >= DATE_SUB(NOW(), INTERVAL 1 MONTH)
LEFT JOIN maintenances m ON u.id_utilisateur = m.id_utilisateur 
    AND DATE(m.date_creation) >= DATE_SUB(NOW(), INTERVAL 1 MONTH)
GROUP BY u.id_utilisateur, u.nom_utilisateur;

-- 6. État de la flotte
SELECT 
    etat,
    COUNT(*) as nombre,
    ROUND(COUNT(*) * 100 / (SELECT COUNT(*) FROM vehicules), 2) as pourcentage
FROM vehicules
GROUP BY etat
ORDER BY nombre DESC;

-- 7. Kilométrage par véhicule
SELECT 
    marque,
    modele,
    immatriculation,
    kilométrage_initial,
    kilométrage_actuel,
    (kilométrage_actuel - kilométrage_initial) as distance_parcourue_total,
    date_acquisition,
    DATEDIFF(NOW(), date_acquisition) as jours_en_service
FROM vehicules
ORDER BY (kilométrage_actuel - kilométrage_initial) DESC;

-- 8. Ajouter des données de test
INSERT INTO utilisateurs (nom_utilisateur, email, mot_de_passe, nom, prenom, role) VALUES
('marie_dupont', 'marie@example.com', 'pass123', 'Dupont', 'Marie', 'utilisateur'),
('jean_martin', 'jean@example.com', 'pass123', 'Martin', 'Jean', 'utilisateur');

-- 9. Rapport de consommation hebdomadaire
SELECT 
    WEEK(c.date_saisie) as semaine,
    YEAR(c.date_saisie) as annee,
    v.marque,
    v.modele,
    SUM(c.quantite_litres) as total_litres,
    SUM(c.cout_total) as total_cout
FROM carburants c
JOIN vehicules v ON c.id_vehicule = v.id_vehicule
WHERE c.date_saisie >= DATE_SUB(NOW(), INTERVAL 1 MONTH)
GROUP BY WEEK(c.date_saisie), YEAR(c.date_saisie), v.id_vehicule
ORDER BY annee DESC, semaine DESC;

-- 10. Véhicules à haut coût d'exploitation
SELECT 
    v.marque,
    v.modele,
    v.immatriculation,
    COUNT(m.id_maintenance) as nombre_maintenances,
    SUM(m.cout_maintenance) as cout_maintenance_total,
    SUM(c.cout_total) as cout_carburant_total,
    (SUM(m.cout_maintenance) + SUM(c.cout_total)) as cout_exploitation_total
FROM vehicules v
LEFT JOIN maintenances m ON v.id_vehicule = m.id_vehicule
LEFT JOIN carburants c ON v.id_vehicule = c.id_vehicule
GROUP BY v.id_vehicule
HAVING cout_exploitation_total > 0
ORDER BY cout_exploitation_total DESC;
