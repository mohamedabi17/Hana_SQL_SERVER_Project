IF DB_ID('EVASIONDB') IS NULL
BEGIN
    CREATE DATABASE EVASIONDB;
END;
GO

USE EVASIONDB;
GO

IF OBJECT_ID('dbo.LOCATION', 'U') IS NOT NULL DROP TABLE dbo.LOCATION;
IF OBJECT_ID('dbo.VEHICULE', 'U') IS NOT NULL DROP TABLE dbo.VEHICULE;
IF OBJECT_ID('dbo.CLIENT', 'U') IS NOT NULL DROP TABLE dbo.CLIENT;
IF OBJECT_ID('dbo.UTILISATEUR', 'U') IS NOT NULL DROP TABLE dbo.UTILISATEUR;
IF OBJECT_ID('dbo.TARIF', 'U') IS NOT NULL DROP TABLE dbo.TARIF;
GO

CREATE TABLE dbo.CLIENT
(
    codeclient NVARCHAR(20) NOT NULL,
    nom NVARCHAR(80) NOT NULL,
    prenom NVARCHAR(80) NOT NULL,
    adresse NVARCHAR(200) NOT NULL,
    telephone NVARCHAR(20) NOT NULL,
    typeclient CHAR(3) NOT NULL,
    date_creation DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_CLIENT PRIMARY KEY (codeclient),
    CONSTRAINT CK_CLIENT_TYPE CHECK (typeclient IN ('IND', 'ORG')),
    CONSTRAINT CK_CLIENT_PHONE CHECK (LEN(telephone) >= 8),
    CONSTRAINT CK_CLIENT_NAME_NOT_EMPTY CHECK (LEN(TRIM(nom)) > 0)
);
GO

CREATE TABLE dbo.TARIF
(
    categorie NVARCHAR(30) NOT NULL,
    prix_jour DECIMAL(10, 2) NOT NULL,
    prix_km DECIMAL(10, 2) NOT NULL,
    CONSTRAINT PK_TARIF PRIMARY KEY (categorie),
    CONSTRAINT CK_TARIF_PRIX_JOUR CHECK (prix_jour >= 0),
    CONSTRAINT CK_TARIF_PRIX_KM CHECK (prix_km >= 0)
);
GO

CREATE TABLE dbo.VEHICULE
(
    immatriculation NVARCHAR(20) NOT NULL,
    categorie NVARCHAR(30) NOT NULL,
    marque NVARCHAR(50) NOT NULL,
    modele NVARCHAR(50) NOT NULL,
    kilometrage INT NOT NULL,
    disponible BIT NOT NULL DEFAULT 1,
    date_acquisition DATE NULL,
    CONSTRAINT PK_VEHICULE PRIMARY KEY (immatriculation),
    CONSTRAINT FK_VEHICULE_TARIF FOREIGN KEY (categorie) REFERENCES dbo.TARIF(categorie),
    CONSTRAINT CK_VEHICULE_KM CHECK (kilometrage >= 0)
);
GO

CREATE TABLE dbo.UTILISATEUR
(
    id INT IDENTITY(1,1) NOT NULL,
    username NVARCHAR(50) NOT NULL,
    [password] NVARCHAR(100) NOT NULL,
    [role] NVARCHAR(30) NOT NULL,
    CONSTRAINT PK_UTILISATEUR PRIMARY KEY (id),
    CONSTRAINT UQ_UTILISATEUR_USERNAME UNIQUE (username),
    CONSTRAINT CK_UTILISATEUR_ROLE CHECK ([role] IN ('ADMIN', 'AGENT'))
);
GO

CREATE TABLE dbo.LOCATION
(
    id INT IDENTITY(1,1) NOT NULL,
    codeclient NVARCHAR(20) NOT NULL,
    immatriculation NVARCHAR(20) NOT NULL,
    datedebut DATE NOT NULL,
    datefin DATE NOT NULL,
    dateretour DATE NULL,
    kmparcourus INT NULL,
    montant DECIMAL(12, 2) NULL,
    date_creation DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_LOCATION PRIMARY KEY (id),
    CONSTRAINT FK_LOCATION_CLIENT FOREIGN KEY (codeclient) REFERENCES dbo.CLIENT(codeclient) ON DELETE NO ACTION,
    CONSTRAINT FK_LOCATION_VEHICULE FOREIGN KEY (immatriculation) REFERENCES dbo.VEHICULE(immatriculation) ON DELETE NO ACTION,
    CONSTRAINT CK_LOCATION_DATES CHECK (datefin >= datedebut),
    CONSTRAINT CK_LOCATION_RETOUR CHECK (dateretour IS NULL OR dateretour >= datedebut),
    CONSTRAINT CK_LOCATION_KM CHECK (kmparcourus IS NULL OR kmparcourus >= 0),
    CONSTRAINT CK_LOCATION_MONTANT CHECK (montant IS NULL OR montant >= 0)
);
GO

INSERT INTO dbo.TARIF (categorie, prix_jour, prix_km)
VALUES
    ('ECONOMIQUE', 120.00, 1.20),
    ('COMPACTE', 180.00, 1.50),
    ('SUV', 280.00, 2.20),
    ('LUXE', 450.00, 3.50);
GO

INSERT INTO dbo.CLIENT (codeclient, nom, prenom, adresse, telephone, typeclient)
VALUES
    ('CL001', 'Benali', 'Yassine', '12 Rue Atlas, Casablanca', '0612345678', 'IND'),
    ('CL002', 'Amrani', 'Salma', '44 Avenue Hassan II, Rabat', '0623456789', 'IND'),
    ('CL003', 'Dakir', 'Mohammed', '25 Boulevard Sidi Belyout, Fès', '0655234567', 'IND'),
    ('CL004', 'Tazi', 'Leila', '78 Rue Mohammed V, Tanger', '0666345678', 'IND'),
    ('CL005', 'TechCorp', 'Contact', 'Zone Industrielle, Tanger', '0539123456', 'ORG'),
    ('CL006', 'GreenEnergy', 'Contact', 'Boulevard Mohammed V, Marrakech', '0524123456', 'ORG'),
    ('CL007', 'ConstructionPlus', 'Contact', 'Rue de l''Industrie, Casablanca', '0522987654', 'ORG'),
    ('CL008', 'Lemaire', 'Pierre', '99 Avenue de Paris, Shippagan', '0700123456', 'IND');
GO

INSERT INTO dbo.VEHICULE (immatriculation, categorie, marque, modele, kilometrage, disponible, date_acquisition)
VALUES
    ('AA-123-BB', 'ECONOMIQUE', 'Dacia', 'Sandero', 45200, 1, '2022-01-15'),
    ('AA-124-BB', 'ECONOMIQUE', 'Renault', 'Twingo', 32100, 1, '2022-06-10'),
    ('AA-125-BB', 'ECONOMIQUE', 'Hyundai', 'i10', 28900, 1, '2023-02-20'),
    ('CC-456-DD', 'COMPACTE', 'Renault', 'Clio', 38950, 1, '2021-09-05'),
    ('CC-457-DD', 'COMPACTE', 'Peugeot', '308', 41200, 1, '2021-11-12'),
    ('CC-458-DD', 'COMPACTE', 'Volkswagen', 'Golf', 35600, 0, '2022-03-22'),
    ('EE-789-FF', 'SUV', 'Hyundai', 'Tucson', 27400, 1, '2020-07-30'),
    ('EE-790-FF', 'SUV', 'Kia', 'Sportage', 33200, 1, '2021-05-18'),
    ('EE-791-FF', 'SUV', 'Toyota', 'RAV4', 29800, 1, '2022-09-14'),
    ('GG-321-HH', 'LUXE', 'BMW', 'Serie 5', 19800, 1, '2019-12-01'),
    ('GG-322-HH', 'LUXE', 'Mercedes', 'C-Class', 22500, 1, '2020-04-10'),
    ('GG-323-HH', 'LUXE', 'Audi', 'A4', 24300, 1, '2020-08-25');
GO

INSERT INTO dbo.LOCATION (codeclient, immatriculation, datedebut, datefin, dateretour, kmparcourus, montant)
VALUES
    ('CL001', 'AA-123-BB', '2026-03-01', '2026-03-03', '2026-03-03', 150, 510.00),
    ('CL003', 'EE-789-FF', '2026-03-05', '2026-03-08', '2026-03-08', 320, 1824.00),
    ('CL002', 'CC-456-DD', '2026-03-10', '2026-03-12', NULL, NULL, NULL),
    ('CL005', 'AA-124-BB', '2026-03-06', '2026-03-07', '2026-03-07', 95, 312.00),
    ('CL006', 'EE-790-FF', '2026-03-11', '2026-03-15', NULL, NULL, NULL),
    ('CL004', 'GG-321-HH', '2026-02-28', '2026-03-02', '2026-03-02', 280, 2160.00),
    ('CL007', 'CC-457-DD', '2026-03-08', '2026-03-10', '2026-03-10', 210, 720.00);
GO

-- ==================== PERFORMANCE INDEXES ====================
-- Indexes created for query optimization
-- Purpose: Improve performance on frequently searched columns
-- No impact on INSERT/UPDATE/DELETE logic

-- Index 1: CLIENT table - Search by name (nom)
-- Used by: SearchClient procedure and client lookup operations
-- Improves performance when filtering clients by name or first name
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_CLIENT_NOM')
BEGIN
    CREATE NONCLUSTERED INDEX IDX_CLIENT_NOM 
    ON dbo.CLIENT (nom, prenom)
    WITH (FILLFACTOR = 80);
END;
GO

-- Index 2: VEHICULE table - Search by category
-- Used by: Vehicle filtering by category, tariff association
-- Improves performance when querying vehicles by category (ECONOMIQUE, COMPACTE, SUV, LUXE)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_VEHICULE_CATEGORIE')
BEGIN
    CREATE NONCLUSTERED INDEX IDX_VEHICULE_CATEGORIE 
    ON dbo.VEHICULE (categorie)
    INCLUDE (marque, modele, prix_jour, prix_km)
    WITH (FILLFACTOR = 80);
END;
GO

-- Index 3: LOCATION table - Search by rental dates
-- Used by: GetAvailableVehicles, IsVehicleAvailable procedures
-- Improves performance when checking vehicle availability for date ranges
-- Critical for preventing double-booking and finding available vehicles
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_LOCATION_DATES')
BEGIN
    CREATE NONCLUSTERED INDEX IDX_LOCATION_DATES 
    ON dbo.LOCATION (datedebut, datefin)
    INCLUDE (immatriculation, dateretour)
    WITH (FILLFACTOR = 80);
END;
GO

-- Index 4: LOCATION table - Search by vehicle (immatriculation)
-- Used by: Rental history lookups, vehicle analytics queries
-- Improves performance when retrieving all rentals for a specific vehicle
-- Essential for vehicle usage reports and availability verification
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_LOCATION_IMMATRICULATION')
BEGIN
    CREATE NONCLUSTERED INDEX IDX_LOCATION_IMMATRICULATION 
    ON dbo.LOCATION (immatriculation)
    INCLUDE (codeclient, datedebut, datefin, dateretour, montant)
    WITH (FILLFACTOR = 80);
END;
GO

-- Index 5: LOCATION table - Search by client
-- Used by: Client rental history, analytics by client
-- Improves performance when retrieving all rentals for a specific client
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_LOCATION_CODECLIENT')
BEGIN
    CREATE NONCLUSTERED INDEX IDX_LOCATION_CODECLIENT 
    ON dbo.LOCATION (codeclient)
    INCLUDE (immatriculation, datedebut, datefin, montant)
    WITH (FILLFACTOR = 80);
END;
GO

-- Index 6: LOCATION table - Filter non-returned rentals
-- Used by: GetPendingRentals, ReportPendingRentals procedures
-- Improves performance when filtering locations where dateretour IS NULL
-- Critical for tracking active rentals and overdue vehicles
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_LOCATION_DATERETOUR')
BEGIN
    CREATE NONCLUSTERED INDEX IDX_LOCATION_DATERETOUR 
    ON dbo.LOCATION (dateretour)
    WHERE dateretour IS NULL
    INCLUDE (datedebut, datefin, immatriculation)
    WITH (FILLFACTOR = 80);
END;
GO