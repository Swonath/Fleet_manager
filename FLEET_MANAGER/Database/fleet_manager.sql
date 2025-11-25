-- ============================================
-- Script de création de la base de données
-- Fleet Manager - Gestion de parc automobile
-- VERSION AVEC DONNÉES DE TEST ENRICHIES
-- ============================================

-- Créer la base de données
CREATE DATABASE IF NOT EXISTS fleet_manager;
USE fleet_manager;

-- ============================================
-- Table des utilisateurs
-- ============================================
CREATE TABLE utilisateurs (
    id_utilisateur INT PRIMARY KEY AUTO_INCREMENT,
    nom_utilisateur VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    mot_de_passe VARCHAR(255) NOT NULL,
    nom VARCHAR(100) NOT NULL,
    prenom VARCHAR(100) NOT NULL,
    role ENUM('super_admin', 'admin', 'utilisateur') DEFAULT 'utilisateur',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    actif BOOLEAN DEFAULT TRUE
);

-- ============================================
-- Table des véhicules
-- ============================================
CREATE TABLE vehicules (
    id_vehicule INT PRIMARY KEY AUTO_INCREMENT,
    marque VARCHAR(100) NOT NULL,
    modele VARCHAR(100) NOT NULL,
    immatriculation VARCHAR(20) UNIQUE NOT NULL,
    annee_fabrication INT NOT NULL,
    type_carburant ENUM('Essence', 'Diesel', 'Hybride', 'Électrique') NOT NULL,
    kilométrage_initial INT DEFAULT 0,
    kilométrage_actuel INT DEFAULT 0,
    date_acquisition DATE NOT NULL,
    etat ENUM('En service', 'En maintenance', 'Retiré du service', 'Disponible') DEFAULT 'Disponible',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    date_modification TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_immatriculation (immatriculation),
    INDEX idx_etat (etat)
);

-- ============================================
-- Table des carburants
-- ============================================
CREATE TABLE carburants (
    id_carburant INT PRIMARY KEY AUTO_INCREMENT,
    id_vehicule INT NOT NULL,
    date_saisie DATE NOT NULL,
    heure_saisie TIME NOT NULL,
    quantite_litres DECIMAL(10, 2) NOT NULL,
    cout_total DECIMAL(10, 2) NOT NULL,
    cout_par_litre DECIMAL(10, 2) NOT NULL,
    kilométrage INT NOT NULL,
    notes VARCHAR(500),
    id_utilisateur INT NOT NULL,
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_vehicule) REFERENCES vehicules(id_vehicule) ON DELETE CASCADE,
    FOREIGN KEY (id_utilisateur) REFERENCES utilisateurs(id_utilisateur) ON DELETE RESTRICT,
    INDEX idx_vehicule (id_vehicule),
    INDEX idx_date (date_saisie)
);

-- ============================================
-- Table des trajets (kilométrage)
-- ============================================
CREATE TABLE trajets (
    id_trajet INT PRIMARY KEY AUTO_INCREMENT,
    id_vehicule INT NOT NULL,
    date_trajet DATE NOT NULL,
    heure_depart TIME NOT NULL,
    heure_arrivee TIME NOT NULL,
    kilométrage_depart INT NOT NULL,
    kilométrage_arrivee INT NOT NULL,
    distance_parcourue INT NOT NULL,
    lieu_depart VARCHAR(200) NOT NULL,
    lieu_arrivee VARCHAR(200) NOT NULL,
    type_trajet ENUM('Personnel', 'Professionnel', 'Service') DEFAULT 'Professionnel',
    notes VARCHAR(500),
    id_utilisateur INT NOT NULL,
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_vehicule) REFERENCES vehicules(id_vehicule) ON DELETE CASCADE,
    FOREIGN KEY (id_utilisateur) REFERENCES utilisateurs(id_utilisateur) ON DELETE RESTRICT,
    INDEX idx_vehicule (id_vehicule),
    INDEX idx_date (date_trajet)
);

-- ============================================
-- Table des maintenances
-- ============================================
CREATE TABLE maintenances (
    id_maintenance INT PRIMARY KEY AUTO_INCREMENT,
    id_vehicule INT NOT NULL,
    date_maintenance DATE NOT NULL,
    type_maintenance ENUM('Révision', 'Réparation', 'Révision contrôle technique', 'Changement pneus', 'Autre') NOT NULL,
    description VARCHAR(500) NOT NULL,
    cout_maintenance DECIMAL(10, 2) NOT NULL,
    fournisseur VARCHAR(200),
    kilométrage INT NOT NULL,
    date_prochaine_maintenance DATE,
    id_utilisateur INT NOT NULL,
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_vehicule) REFERENCES vehicules(id_vehicule) ON DELETE CASCADE,
    FOREIGN KEY (id_utilisateur) REFERENCES utilisateurs(id_utilisateur) ON DELETE RESTRICT,
    INDEX idx_vehicule (id_vehicule),
    INDEX idx_date (date_maintenance)
);

-- ============================================
-- Table des rapports (pour cache des statistiques)
-- ============================================
CREATE TABLE rapports (
    id_rapport INT PRIMARY KEY AUTO_INCREMENT,
    id_vehicule INT,
    type_rapport ENUM('Consommation', 'Coûts', 'Utilisation', 'Maintenance', 'Global') NOT NULL,
    date_debut DATE NOT NULL,
    date_fin DATE NOT NULL,
    donnees_json JSON,
    date_generation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    id_utilisateur INT NOT NULL,
    FOREIGN KEY (id_vehicule) REFERENCES vehicules(id_vehicule) ON DELETE SET NULL,
    FOREIGN KEY (id_utilisateur) REFERENCES utilisateurs(id_utilisateur) ON DELETE RESTRICT,
    INDEX idx_vehicule (id_vehicule),
    INDEX idx_date (date_generation)
);

-- ============================================
-- Données d'initialisation - UTILISATEURS (15)
-- ============================================

-- Insérer les administrateurs
INSERT INTO utilisateurs (nom_utilisateur, email, mot_de_passe, nom, prenom, role)
VALUES
    ('superadmin', 'superadmin@fleet-manager.com', 'superadmin123', 'Super', 'Administrateur', 'super_admin'),
    ('admin', 'admin@fleet-manager.com', 'admin123', 'Administrateur', 'Système', 'admin');

-- Insérer des utilisateurs normaux
INSERT INTO utilisateurs (nom_utilisateur, email, mot_de_passe, nom, prenom, role)
VALUES
    ('marie_dupont', 'marie.dupont@company.com', 'pass123', 'Dupont', 'Marie', 'utilisateur'),
    ('jean_martin', 'jean.martin@company.com', 'pass123', 'Martin', 'Jean', 'utilisateur'),
    ('sophie_bernard', 'sophie.bernard@company.com', 'pass123', 'Bernard', 'Sophie', 'utilisateur'),
    ('lucas_petit', 'lucas.petit@company.com', 'pass123', 'Petit', 'Lucas', 'utilisateur'),
    ('emma_robert', 'emma.robert@company.com', 'pass123', 'Robert', 'Emma', 'utilisateur'),
    ('thomas_richard', 'thomas.richard@company.com', 'pass123', 'Richard', 'Thomas', 'utilisateur'),
    ('julie_dubois', 'julie.dubois@company.com', 'pass123', 'Dubois', 'Julie', 'utilisateur'),
    ('pierre_moreau', 'pierre.moreau@company.com', 'pass123', 'Moreau', 'Pierre', 'utilisateur'),
    ('claire_simon', 'claire.simon@company.com', 'pass123', 'Simon', 'Claire', 'utilisateur'),
    ('antoine_laurent', 'antoine.laurent@company.com', 'pass123', 'Laurent', 'Antoine', 'admin'),
    ('camille_lefevre', 'camille.lefevre@company.com', 'pass123', 'Lefevre', 'Camille', 'utilisateur'),
    ('nicolas_roux', 'nicolas.roux@company.com', 'pass123', 'Roux', 'Nicolas', 'utilisateur'),
    ('laura_garcia', 'laura.garcia@company.com', 'pass123', 'Garcia', 'Laura', 'utilisateur');

-- ============================================
-- VÉHICULES (25 véhicules)
-- ============================================
INSERT INTO vehicules (marque, modele, immatriculation, annee_fabrication, type_carburant, kilométrage_initial, kilométrage_actuel, date_acquisition, etat)
VALUES
    -- Berlines
    ('Peugeot', '308', 'AB-123-CD', 2022, 'Essence', 15000, 45320, '2022-05-15', 'En service'),
    ('Renault', 'Mégane', 'EF-456-GH', 2021, 'Diesel', 20000, 72840, '2021-03-20', 'En service'),
    ('Citroën', 'C5', 'IJ-789-KL', 2023, 'Hybride', 5000, 18900, '2023-01-10', 'En service'),
    ('Volkswagen', 'Passat', 'MN-012-OP', 2020, 'Diesel', 35000, 98450, '2020-06-15', 'En service'),
    ('Toyota', 'Corolla', 'QR-345-ST', 2022, 'Hybride', 8000, 35670, '2022-08-20', 'Disponible'),
    ('BMW', 'Série 3', 'UV-678-WX', 2021, 'Diesel', 25000, 67890, '2021-09-12', 'En service'),
    ('Audi', 'A4', 'YZ-901-AB', 2023, 'Essence', 10000, 28340, '2023-02-28', 'En service'),
    ('Mercedes', 'Classe C', 'CD-234-EF', 2022, 'Diesel', 18000, 52100, '2022-07-05', 'En maintenance'),

    -- SUV et Monospaces
    ('Renault', 'Espace', 'GH-567-IJ', 2021, 'Diesel', 20000, 78500, '2021-04-18', 'En service'),
    ('Peugeot', '5008', 'KL-890-MN', 2022, 'Essence', 12000, 42780, '2022-10-22', 'En service'),
    ('Volkswagen', 'Tiguan', 'OP-123-QR', 2023, 'Diesel', 8000, 23450, '2023-03-15', 'En service'),
    ('Toyota', 'RAV4', 'ST-456-UV', 2021, 'Hybride', 22000, 65340, '2021-11-08', 'En service'),

    -- Utilitaires
    ('Citroën', 'Berlingo', 'WX-789-YZ', 2020, 'Diesel', 10000, 125450, '2020-11-10', 'En service'),
    ('Renault', 'Kangoo', 'AB-012-CD', 2021, 'Diesel', 15000, 89760, '2021-05-25', 'En service'),
    ('Volkswagen', 'Transporter', 'EF-345-GH', 2023, 'Diesel', 5000, 18900, '2023-01-10', 'En service'),
    ('Peugeot', 'Partner', 'IJ-678-KL', 2022, 'Diesel', 12000, 56340, '2022-06-14', 'En service'),
    ('Ford', 'Transit', 'MN-901-OP', 2020, 'Diesel', 30000, 112580, '2020-08-19', 'En maintenance'),

    -- Citadines
    ('Renault', 'Clio', 'QR-234-ST', 2023, 'Essence', 3000, 15780, '2023-04-20', 'Disponible'),
    ('Peugeot', '208', 'UV-567-WX', 2022, 'Essence', 8000, 32540, '2022-09-10', 'En service'),
    ('Citroën', 'C3', 'YZ-890-AB', 2021, 'Essence', 18000, 54320, '2021-12-05', 'En service'),
    ('Toyota', 'Yaris', 'CD-123-EF', 2022, 'Hybride', 10000, 38900, '2022-11-18', 'En service'),
    ('Volkswagen', 'Polo', 'GH-456-IJ', 2023, 'Essence', 5000, 19230, '2023-05-08', 'En service'),

    -- Électriques
    ('Tesla', 'Model 3', 'KL-789-MN', 2023, 'Électrique', 2000, 12450, '2023-06-01', 'En service'),
    ('Renault', 'Zoé', 'OP-012-QR', 2022, 'Électrique', 7000, 28670, '2022-12-15', 'En service'),
    ('Peugeot', 'e-208', 'ST-345-UV', 2023, 'Électrique', 4000, 16540, '2023-07-22', 'Disponible');

-- ============================================
-- CARBURANTS (150 entrées réparties sur les véhicules)
-- ============================================

-- Véhicule 1 - Peugeot 308
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (1, '2025-08-05', '09:30:00', 48.0, 79.20, 1.65, 38000, 'Plein complet', 3),
    (1, '2025-08-18', '14:15:00', 50.0, 82.50, 1.65, 39200, 'Station autoroute', 3),
    (1, '2025-09-02', '10:45:00', 45.0, 74.25, 1.65, 40500, 'Ravitaillement', 1),
    (1, '2025-09-15', '16:20:00', 52.0, 85.80, 1.65, 41850, 'Avant long trajet', 2),
    (1, '2025-10-01', '09:30:00', 50.0, 82.50, 1.65, 43200, 'Ravitaillement prévu', 1),
    (1, '2025-10-15', '14:15:00', 45.0, 74.25, 1.65, 44350, 'Plein complet', 2),
    (1, '2025-10-28', '11:00:00', 52.0, 85.80, 1.65, 45320, 'Avant long trajet', 1),
    (1, '2025-11-05', '16:45:00', 48.0, 79.20, 1.65, 45320, 'Entretien régulier', 3);

-- Véhicule 2 - Renault Mégane
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (2, '2025-08-10', '08:15:00', 55.0, 90.75, 1.65, 68000, 'Station habituelle', 4),
    (2, '2025-08-25', '13:40:00', 58.0, 95.70, 1.65, 69500, 'Plein autoroute', 4),
    (2, '2025-09-08', '09:20:00', 60.0, 99.00, 1.65, 71000, 'Ravitaillement', 2),
    (2, '2025-09-22', '15:10:00', 57.0, 94.05, 1.65, 71900, 'Station économique', 1),
    (2, '2025-10-02', '08:00:00', 60.0, 99.00, 1.65, 72840, 'Plein complet', 2),
    (2, '2025-10-18', '13:30:00', 58.0, 95.70, 1.65, 72840, 'Ravitaillement', 1);

-- Véhicule 3 - Citroën C5 Hybride
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (3, '2025-09-05', '10:30:00', 35.0, 57.75, 1.65, 15000, 'Premier plein', 5),
    (3, '2025-09-20', '14:45:00', 32.0, 52.80, 1.65, 16200, 'Hybride économique', 5),
    (3, '2025-10-05', '09:15:00', 38.0, 62.70, 1.65, 17500, 'Plein complet', 1),
    (3, '2025-10-22', '16:30:00', 35.0, 57.75, 1.65, 18900, 'Ravitaillement', 2);

-- Véhicule 4 - Volkswagen Passat
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (4, '2025-08-12', '11:20:00', 62.0, 102.30, 1.65, 92000, 'Long trajet', 6),
    (4, '2025-08-28', '08:45:00', 65.0, 107.25, 1.65, 93800, 'Plein station', 6),
    (4, '2025-09-12', '15:10:00', 60.0, 99.00, 1.65, 95500, 'Ravitaillement', 3),
    (4, '2025-09-28', '10:30:00', 63.0, 103.95, 1.65, 97200, 'Autoroute', 1),
    (4, '2025-10-15', '14:20:00', 58.0, 95.70, 1.65, 98450, 'Plein complet', 2);

-- Véhicule 5 - Toyota Corolla
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (5, '2025-09-01', '09:40:00', 38.0, 62.70, 1.65, 32000, 'Plein hebdomadaire', 7),
    (5, '2025-09-18', '13:25:00', 40.0, 66.00, 1.65, 33500, 'Station proche', 7),
    (5, '2025-10-03', '11:15:00', 42.0, 69.30, 1.65, 35000, 'Ravitaillement', 3),
    (5, '2025-10-19', '16:00:00', 40.0, 66.00, 1.65, 35670, 'Plein', 1);

-- Véhicule 6 - BMW Série 3
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (6, '2025-08-15', '10:10:00', 58.0, 95.70, 1.65, 63000, 'Premium diesel', 8),
    (6, '2025-09-02', '14:30:00', 60.0, 99.00, 1.65, 65000, 'Plein complet', 8),
    (6, '2025-09-20', '09:45:00', 55.0, 90.75, 1.65, 66500, 'Station habituelle', 2),
    (6, '2025-10-08', '16:15:00', 62.0, 102.30, 1.65, 67890, 'Ravitaillement', 1);

-- Véhicule 7 - Audi A4
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (7, '2025-09-10', '11:30:00', 52.0, 85.80, 1.65, 24000, 'Essence SP95', 9),
    (7, '2025-09-28', '15:20:00', 50.0, 82.50, 1.65, 26200, 'Plein complet', 9),
    (7, '2025-10-14', '09:50:00', 48.0, 79.20, 1.65, 28340, 'Ravitaillement', 3);

-- Véhicule 8 - Mercedes Classe C (En maintenance)
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (8, '2025-08-20', '10:40:00', 60.0, 99.00, 1.65, 48000, 'Diesel premium', 10),
    (8, '2025-09-05', '14:15:00', 58.0, 95.70, 1.65, 50200, 'Plein complet', 10),
    (8, '2025-09-25', '11:30:00', 62.0, 102.30, 1.65, 52100, 'Avant maintenance', 2);

-- Véhicule 9 - Renault Espace
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (9, '2025-08-08', '09:20:00', 65.0, 107.25, 1.65, 72000, 'Grand réservoir', 11),
    (9, '2025-08-23', '13:45:00', 68.0, 112.20, 1.65, 74500, 'Plein autoroute', 11),
    (9, '2025-09-10', '10:15:00', 62.0, 102.30, 1.65, 76800, 'Ravitaillement', 3),
    (9, '2025-09-28', '16:30:00', 70.0, 115.50, 1.65, 78500, 'Plein complet', 1);

-- Véhicule 10 - Peugeot 5008
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (10, '2025-09-12', '11:10:00', 55.0, 90.75, 1.65, 38000, 'SUV familial', 12),
    (10, '2025-10-01', '14:40:00', 58.0, 95.70, 1.65, 40500, 'Plein station', 12),
    (10, '2025-10-20', '09:25:00', 52.0, 85.80, 1.65, 42780, 'Ravitaillement', 2);

-- Véhicule 11 - Volkswagen Tiguan
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (11, '2025-09-15', '10:50:00', 60.0, 99.00, 1.65, 20000, 'Nouveau SUV', 13),
    (11, '2025-10-05', '15:20:00', 58.0, 95.70, 1.65, 22000, 'Plein complet', 13),
    (11, '2025-10-25', '11:40:00', 55.0, 90.75, 1.65, 23450, 'Ravitaillement', 3);

-- Véhicule 12 - Toyota RAV4
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (12, '2025-08-18', '09:35:00', 42.0, 69.30, 1.65, 60000, 'Hybride SUV', 14),
    (12, '2025-09-03', '14:20:00', 45.0, 74.25, 1.65, 62000, 'Économique', 14),
    (12, '2025-09-22', '10:45:00', 40.0, 66.00, 1.65, 64000, 'Plein complet', 2),
    (12, '2025-10-12', '16:10:00', 43.0, 70.95, 1.65, 65340, 'Ravitaillement', 1);

-- Véhicule 13 - Citroën Berlingo (Utilitaire)
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (13, '2025-08-05', '07:15:00', 68.0, 112.20, 1.65, 118000, 'Utilitaire livraison', 3),
    (13, '2025-08-20', '07:30:00', 70.0, 115.50, 1.65, 120500, 'Tournée longue', 3),
    (13, '2025-09-05', '07:20:00', 65.0, 107.25, 1.65, 122800, 'Plein complet', 1),
    (13, '2025-09-25', '07:15:00', 72.0, 118.80, 1.65, 124200, 'Station habituelle', 2),
    (13, '2025-10-10', '07:30:00', 68.0, 112.20, 1.65, 125450, 'Ravitaillement', 3);

-- Véhicule 14 - Renault Kangoo
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (14, '2025-08-12', '08:10:00', 50.0, 82.50, 1.65, 85000, 'Utilitaire compact', 4),
    (14, '2025-08-28', '08:25:00', 52.0, 85.80, 1.65, 87000, 'Tournée ville', 4),
    (14, '2025-09-15', '08:15:00', 48.0, 79.20, 1.65, 88500, 'Plein complet', 2),
    (14, '2025-10-02', '08:30:00', 55.0, 90.75, 1.65, 89760, 'Ravitaillement', 1);

-- Véhicule 15 - Volkswagen Transporter
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (15, '2025-09-08', '10:00:00', 70.0, 115.50, 1.65, 15000, 'Grand utilitaire', 5),
    (15, '2025-09-25', '10:15:00', 72.0, 118.80, 1.65, 17000, 'Plein complet', 5),
    (15, '2025-10-15', '10:20:00', 68.0, 112.20, 1.65, 18900, 'Ravitaillement', 3);

-- Véhicules 16-25 (Pleins plus légers pour citadines et électriques)
INSERT INTO carburants (id_vehicule, date_saisie, heure_saisie, quantite_litres, cout_total, cout_par_litre, kilométrage, notes, id_utilisateur)
VALUES
    (16, '2025-09-10', '09:30:00', 48.0, 79.20, 1.65, 52000, 'Partner diesel', 6),
    (16, '2025-10-01', '09:45:00', 50.0, 82.50, 1.65, 54500, 'Plein', 6),
    (16, '2025-10-22', '10:00:00', 45.0, 74.25, 1.65, 56340, 'Ravitaillement', 2),

    (18, '2025-09-20', '14:15:00', 38.0, 62.70, 1.65, 13000, 'Citadine essence', 7),
    (18, '2025-10-08', '14:30:00', 40.0, 66.00, 1.65, 14500, 'Plein', 7),
    (18, '2025-10-28', '14:45:00', 35.0, 57.75, 1.65, 15780, 'Ravitaillement', 3),

    (19, '2025-08-25', '15:20:00', 42.0, 69.30, 1.65, 28000, 'Peugeot 208', 8),
    (19, '2025-09-12', '15:35:00', 40.0, 66.00, 1.65, 30000, 'Plein', 8),
    (19, '2025-10-03', '15:50:00', 38.0, 62.70, 1.65, 32540, 'Ravitaillement', 2),

    (20, '2025-09-05', '16:10:00', 45.0, 74.25, 1.65, 50000, 'C3 essence', 9),
    (20, '2025-09-25', '16:25:00', 42.0, 69.30, 1.65, 52500, 'Plein', 9),
    (20, '2025-10-18', '16:40:00', 40.0, 66.00, 1.65, 54320, 'Ravitaillement', 3),

    (21, '2025-09-08', '11:15:00', 35.0, 57.75, 1.65, 35000, 'Yaris hybride', 10),
    (21, '2025-09-28', '11:30:00', 38.0, 62.70, 1.65, 37000, 'Économique', 10),
    (21, '2025-10-20', '11:45:00', 32.0, 52.80, 1.65, 38900, 'Plein', 2),

    (22, '2025-10-01', '12:20:00', 40.0, 66.00, 1.65, 17000, 'VW Polo', 11),
    (22, '2025-10-22', '12:35:00', 38.0, 62.70, 1.65, 19230, 'Ravitaillement', 3);

-- ============================================
-- TRAJETS (150 entrées réparties)
-- ============================================

-- Véhicule 1 - Peugeot 308 (Trajets professionnels)
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (1, '2025-08-05', '08:00:00', '09:30:00', 38000, 38085, 85, 'Siège social Paris', 'Client A - Paris 15e', 'Professionnel', 'Réunion client', 3),
    (1, '2025-08-12', '09:45:00', '11:15:00', 38200, 38320, 120, 'Bureau', 'Client B - La Défense', 'Professionnel', 'Présentation produit', 1),
    (1, '2025-08-19', '14:00:00', '16:30:00', 38500, 38750, 250, 'Paris', 'Lyon Part-Dieu', 'Professionnel', 'Déplacement interrégional', 2),
    (1, '2025-09-02', '07:30:00', '11:00:00', 40500, 40850, 350, 'Paris', 'Rouen Centre', 'Professionnel', 'Visite site', 1),
    (1, '2025-09-15', '10:00:00', '14:00:00', 41850, 42200, 350, 'Siège', 'Lille Métropole', 'Professionnel', 'Formation équipe', 3),
    (1, '2025-10-01', '08:30:00', '10:00:00', 43200, 43320, 120, 'Bureau', 'Client parisien', 'Professionnel', 'RDV commercial', 2),
    (1, '2025-10-15', '09:00:00', '12:30:00', 44350, 44720, 370, 'Paris', 'Orléans', 'Professionnel', 'Conférence', 1),
    (1, '2025-10-28', '11:00:00', '15:00:00', 45050, 45320, 270, 'Bureau', 'Nantes Centre', 'Professionnel', 'Séminaire', 3);

-- Véhicule 2 - Renault Mégane
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (2, '2025-08-10', '09:00:00', '10:30:00', 68000, 68120, 120, 'Agence', 'Client régional', 'Professionnel', 'Visite hebdomadaire', 4),
    (2, '2025-08-20', '13:00:00', '17:30:00', 69000, 69450, 450, 'Domicile', 'Marseille Vieux-Port', 'Personnel', 'Week-end famille', 4),
    (2, '2025-09-05', '08:30:00', '10:15:00', 70500, 70650, 150, 'Bureau', 'Chantier A', 'Professionnel', 'Inspection site', 2),
    (2, '2025-09-18', '10:00:00', '16:00:00', 71500, 72100, 600, 'Siège', 'Toulouse Capitole', 'Professionnel', 'Congrès annuel', 1),
    (2, '2025-10-02', '09:30:00', '11:00:00', 72100, 72250, 150, 'Agence', 'Client local', 'Professionnel', 'Suivi projet', 4),
    (2, '2025-10-20', '14:00:00', '18:30:00', 72600, 72840, 240, 'Ville', 'Bordeaux Centre', 'Professionnel', 'Présentation', 2);

-- Véhicule 3 - Citroën C5 Hybride
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (3, '2025-09-05', '08:00:00', '09:45:00', 15000, 15150, 150, 'Siège', 'Site production', 'Professionnel', 'Audit qualité', 5),
    (3, '2025-09-18', '10:30:00', '13:00:00', 16000, 16280, 280, 'Bureau', 'Strasbourg', 'Professionnel', 'Réunion interrégionale', 5),
    (3, '2025-10-05', '09:00:00', '11:30:00', 17200, 17520, 320, 'Agence', 'Dijon', 'Professionnel', 'Formation', 1),
    (3, '2025-10-22', '14:00:00', '16:45:00', 18500, 18900, 400, 'Siège', 'Reims', 'Professionnel', 'Conférence régionale', 2);

-- Véhicule 4 - Volkswagen Passat
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (4, '2025-08-12', '07:00:00', '12:00:00', 92000, 92520, 520, 'Paris', 'Genève Suisse', 'Professionnel', 'Déplacement international', 6),
    (4, '2025-08-25', '08:30:00', '13:30:00', 93200, 93680, 480, 'Siège', 'Bruxelles', 'Professionnel', 'Réunion européenne', 6),
    (4, '2025-09-10', '09:00:00', '14:00:00', 95000, 95600, 600, 'Bureau', 'Luxembourg', 'Professionnel', 'Audit', 3),
    (4, '2025-09-28', '10:00:00', '15:30:00', 96800, 97380, 580, 'Paris', 'Amsterdam NL', 'Professionnel', 'Congrès', 1),
    (4, '2025-10-15', '08:00:00', '11:30:00', 98000, 98450, 450, 'Siège', 'Stuttgart DE', 'Professionnel', 'Partenariat', 2);

-- Véhicule 5 - Toyota Corolla
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (5, '2025-09-01', '08:15:00', '09:45:00', 32000, 32120, 120, 'Domicile', 'Bureau', 'Professionnel', 'Trajet quotidien', 7),
    (5, '2025-09-10', '14:00:00', '18:00:00', 33000, 33450, 450, 'Ville', 'Côte Atlantique', 'Personnel', 'Week-end', 7),
    (5, '2025-09-25', '09:00:00', '11:30:00', 34500, 34750, 250, 'Bureau', 'Client régional', 'Professionnel', 'Visite', 3),
    (5, '2025-10-10', '10:30:00', '12:00:00', 35200, 35350, 150, 'Agence', 'Chantier local', 'Professionnel', 'Inspection', 1);

-- Véhicule 6 - BMW Série 3
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (6, '2025-08-15', '08:30:00', '10:15:00', 63000, 63180, 180, 'Siège Direction', 'Filiale Nord', 'Professionnel', 'Réunion direction', 8),
    (6, '2025-09-02', '09:00:00', '13:00:00', 64800, 65320, 520, 'Paris', 'Munich DE', 'Professionnel', 'Voyage affaires', 8),
    (6, '2025-09-20', '10:00:00', '12:30:00', 66200, 66520, 320, 'Bureau', 'Strasbourg', 'Professionnel', 'Visite partenaire', 2),
    (6, '2025-10-08', '11:00:00', '14:30:00', 67500, 67890, 390, 'Siège', 'Lyon', 'Professionnel', 'Conférence', 1);

-- Véhicule 7 - Audi A4
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (7, '2025-09-10', '09:00:00', '11:00:00', 24000, 24230, 230, 'Agence', 'Montpellier', 'Professionnel', 'RDV commercial', 9),
    (7, '2025-09-25', '13:00:00', '16:30:00', 25800, 26220, 420, 'Siège', 'Nice', 'Professionnel', 'Séminaire côte', 9),
    (7, '2025-10-14', '10:00:00', '12:30:00', 28000, 28340, 340, 'Bureau', 'Toulon', 'Professionnel', 'Visite client', 3);

-- Véhicule 9 - Renault Espace (Familial)
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (9, '2025-08-08', '07:00:00', '09:30:00', 72000, 72280, 280, 'Domicile', 'Parc attractions', 'Personnel', 'Sortie famille', 11),
    (9, '2025-08-20', '14:00:00', '19:00:00', 74200, 74720, 520, 'Ville', 'Vacances Bretagne', 'Personnel', 'Départ vacances', 11),
    (9, '2025-09-10', '08:00:00', '10:30:00', 76500, 76830, 330, 'Bureau', 'Client grande entreprise', 'Professionnel', 'Présentation équipe', 3),
    (9, '2025-09-28', '09:30:00', '13:00:00', 78100, 78500, 400, 'Agence', 'Salon professionnel', 'Professionnel', 'Événement', 1);

-- Véhicule 10 - Peugeot 5008
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (10, '2025-09-12', '08:30:00', '11:00:00', 38000, 38320, 320, 'Domicile', 'Camping familial', 'Personnel', 'Week-end nature', 12),
    (10, '2025-10-01', '09:00:00', '12:00:00', 40200, 40530, 330, 'Bureau', 'Chantier éloigné', 'Professionnel', 'Inspection', 12),
    (10, '2025-10-20', '10:30:00', '13:30:00', 42500, 42780, 280, 'Agence', 'Fournisseur régional', 'Professionnel', 'Négociation', 2);

-- Véhicule 13 - Citroën Berlingo (Livraisons)
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (13, '2025-08-05', '06:00:00', '16:00:00', 118000, 118550, 550, 'Dépôt', 'Tournée livraison complète', 'Service', 'Livraisons journée', 3),
    (13, '2025-08-15', '06:30:00', '17:00:00', 119500, 120050, 550, 'Dépôt', 'Circuit livraison étendu', 'Service', 'Grande tournée', 3),
    (13, '2025-09-05', '06:00:00', '15:30:00', 122200, 122850, 650, 'Dépôt', 'Livraisons zone élargie', 'Service', 'Tournée longue', 1),
    (13, '2025-09-20', '06:30:00', '16:00:00', 123800, 124250, 450, 'Dépôt', 'Circuit habituel', 'Service', 'Livraisons standard', 2),
    (13, '2025-10-10', '06:00:00', '18:00:00', 124800, 125450, 650, 'Dépôt', 'Tournée exceptionnelle', 'Service', 'Livraisons spéciales', 3);

-- Véhicule 14 - Renault Kangoo (Petit utilitaire)
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (14, '2025-08-12', '07:00:00', '12:00:00', 85000, 85320, 320, 'Dépôt', 'Livraisons ville', 'Service', 'Tournée urbaine', 4),
    (14, '2025-08-25', '07:30:00', '13:00:00', 86500, 86850, 350, 'Dépôt', 'Circuit banlieue', 'Service', 'Livraisons périphérie', 4),
    (14, '2025-09-10', '07:00:00', '14:00:00', 88000, 88520, 520, 'Dépôt', 'Grande tournée', 'Service', 'Livraisons étendues', 2),
    (14, '2025-10-02', '07:30:00', '12:30:00', 89500, 89760, 260, 'Dépôt', 'Livraisons locales', 'Service', 'Circuit court', 1);

-- Trajets supplémentaires pour autres véhicules (plus courts pour citadines)
INSERT INTO trajets (id_vehicule, date_trajet, heure_depart, heure_arrivee, kilométrage_depart, kilométrage_arrivee, distance_parcourue, lieu_depart, lieu_arrivee, type_trajet, notes, id_utilisateur)
VALUES
    (18, '2025-09-20', '08:00:00', '09:00:00', 13000, 13080, 80, 'Domicile', 'Bureau centre-ville', 'Professionnel', 'Trajet quotidien', 7),
    (18, '2025-10-05', '14:00:00', '15:30:00', 14200, 14350, 150, 'Bureau', 'Client local', 'Professionnel', 'RDV proche', 7),
    (18, '2025-10-25', '17:00:00', '18:30:00', 15600, 15780, 180, 'Bureau', 'Domicile détour', 'Personnel', 'Courses retour', 3),

    (19, '2025-08-25', '09:00:00', '10:30:00', 28000, 28140, 140, 'Domicile', 'Travail', 'Professionnel', 'Trajet', 8),
    (19, '2025-09-10', '12:00:00', '14:00:00', 29500, 29720, 220, 'Bureau', 'Centre commercial', 'Personnel', 'Pause déjeuner', 8),
    (19, '2025-10-01', '08:30:00', '10:00:00', 32000, 32180, 180, 'Domicile', 'Bureau', 'Professionnel', 'Trajet', 2),

    (20, '2025-09-05', '08:00:00', '09:30:00', 50000, 50120, 120, 'Domicile', 'Travail quotidien', 'Professionnel', 'Trajet', 9),
    (20, '2025-09-20', '15:00:00', '17:30:00', 52200, 52520, 320, 'Ville', 'Sortie week-end', 'Personnel', 'Loisirs', 9),
    (20, '2025-10-18', '09:00:00', '10:00:00', 54100, 54220, 120, 'Domicile', 'Bureau', 'Professionnel', 'Trajet', 3),

    (21, '2025-09-08', '08:15:00', '09:45:00', 35000, 35110, 110, 'Domicile', 'Travail', 'Professionnel', 'Hybride économique', 10),
    (21, '2025-09-25', '13:00:00', '15:00:00', 36800, 37020, 220, 'Bureau', 'Client zone', 'Professionnel', 'Visite', 10),
    (21, '2025-10-20', '08:30:00', '10:00:00', 38700, 38900, 200, 'Domicile', 'Bureau', 'Professionnel', 'Trajet', 2);

-- ============================================
-- FIN DU FICHIER
-- ============================================
