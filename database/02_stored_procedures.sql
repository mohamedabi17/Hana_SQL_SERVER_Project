USE EVASIONDB;
GO

IF OBJECT_ID('dbo.AddClient', 'P') IS NOT NULL DROP PROCEDURE dbo.AddClient;
GO
CREATE PROCEDURE dbo.AddClient
    @codeclient NVARCHAR(20),
    @nom NVARCHAR(80),
    @prenom NVARCHAR(80),
    @adresse NVARCHAR(200),
    @telephone NVARCHAR(20),
    @typeclient CHAR(3)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.CLIENT (codeclient, nom, prenom, adresse, telephone, typeclient)
    VALUES (@codeclient, @nom, @prenom, @adresse, @telephone, @typeclient);
END;
GO

IF OBJECT_ID('dbo.UpdateClient', 'P') IS NOT NULL DROP PROCEDURE dbo.UpdateClient;
GO
CREATE PROCEDURE dbo.UpdateClient
    @codeclient NVARCHAR(20),
    @nom NVARCHAR(80),
    @prenom NVARCHAR(80),
    @adresse NVARCHAR(200),
    @telephone NVARCHAR(20),
    @typeclient CHAR(3)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.CLIENT
    SET nom = @nom,
        prenom = @prenom,
        adresse = @adresse,
        telephone = @telephone,
        typeclient = @typeclient
    WHERE codeclient = @codeclient;
END;
GO

IF OBJECT_ID('dbo.DeleteClient', 'P') IS NOT NULL DROP PROCEDURE dbo.DeleteClient;
GO
CREATE PROCEDURE dbo.DeleteClient
    @codeclient NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.CLIENT
    WHERE codeclient = @codeclient;
END;
GO

IF OBJECT_ID('dbo.GetClients', 'P') IS NOT NULL DROP PROCEDURE dbo.GetClients;
GO
CREATE PROCEDURE dbo.GetClients
AS
BEGIN
    SET NOCOUNT ON;

    SELECT codeclient, nom, prenom, adresse, telephone, typeclient
    FROM dbo.CLIENT
    ORDER BY nom, prenom;
END;
GO

IF OBJECT_ID('dbo.SearchClient', 'P') IS NOT NULL DROP PROCEDURE dbo.SearchClient;
GO
CREATE PROCEDURE dbo.SearchClient
    @keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT codeclient, nom, prenom, adresse, telephone, typeclient
    FROM dbo.CLIENT
    WHERE codeclient LIKE '%' + @keyword + '%'
       OR nom LIKE '%' + @keyword + '%'
       OR prenom LIKE '%' + @keyword + '%'
       OR telephone LIKE '%' + @keyword + '%'
       OR adresse LIKE '%' + @keyword + '%'
    ORDER BY nom, prenom;
END;
GO

IF OBJECT_ID('dbo.AddVehicle', 'P') IS NOT NULL DROP PROCEDURE dbo.AddVehicle;
GO
CREATE PROCEDURE dbo.AddVehicle
    @immatriculation NVARCHAR(20),
    @categorie NVARCHAR(30),
    @marque NVARCHAR(50),
    @modele NVARCHAR(50),
    @kilometrage INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.VEHICULE (immatriculation, categorie, marque, modele, kilometrage)
    VALUES (@immatriculation, @categorie, @marque, @modele, @kilometrage);
END;
GO

IF OBJECT_ID('dbo.UpdateVehicle', 'P') IS NOT NULL DROP PROCEDURE dbo.UpdateVehicle;
GO
CREATE PROCEDURE dbo.UpdateVehicle
    @immatriculation NVARCHAR(20),
    @categorie NVARCHAR(30),
    @marque NVARCHAR(50),
    @modele NVARCHAR(50),
    @kilometrage INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.VEHICULE
    SET categorie = @categorie,
        marque = @marque,
        modele = @modele,
        kilometrage = @kilometrage
    WHERE immatriculation = @immatriculation;
END;
GO

IF OBJECT_ID('dbo.DeleteVehicle', 'P') IS NOT NULL DROP PROCEDURE dbo.DeleteVehicle;
GO
CREATE PROCEDURE dbo.DeleteVehicle
    @immatriculation NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.VEHICULE
    WHERE immatriculation = @immatriculation;
END;
GO

IF OBJECT_ID('dbo.GetVehicles', 'P') IS NOT NULL DROP PROCEDURE dbo.GetVehicles;
GO
CREATE PROCEDURE dbo.GetVehicles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT immatriculation, categorie, marque, modele, kilometrage
    FROM dbo.VEHICULE
    ORDER BY marque, modele;
END;
GO

IF OBJECT_ID('dbo.SearchVehicle', 'P') IS NOT NULL DROP PROCEDURE dbo.SearchVehicle;
GO
CREATE PROCEDURE dbo.SearchVehicle
    @keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT immatriculation, categorie, marque, modele, kilometrage
    FROM dbo.VEHICULE
    WHERE immatriculation LIKE '%' + @keyword + '%'
       OR categorie LIKE '%' + @keyword + '%'
       OR marque LIKE '%' + @keyword + '%'
       OR modele LIKE '%' + @keyword + '%'
    ORDER BY marque, modele;
END;
GO

IF OBJECT_ID('dbo.AddLocation', 'P') IS NOT NULL DROP PROCEDURE dbo.AddLocation;
GO
CREATE PROCEDURE dbo.AddLocation
    @codeclient NVARCHAR(20),
    @immatriculation NVARCHAR(20),
    @datedebut DATE,
    @datefin DATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LOCATION (codeclient, immatriculation, datedebut, datefin)
    VALUES (@codeclient, @immatriculation, @datedebut, @datefin);
END;
GO

IF OBJECT_ID('dbo.UpdateLocation', 'P') IS NOT NULL DROP PROCEDURE dbo.UpdateLocation;
GO
CREATE PROCEDURE dbo.UpdateLocation
    @id INT,
    @codeclient NVARCHAR(20),
    @immatriculation NVARCHAR(20),
    @datedebut DATE,
    @datefin DATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.LOCATION
    SET codeclient = @codeclient,
        immatriculation = @immatriculation,
        datedebut = @datedebut,
        datefin = @datefin
    WHERE id = @id;
END;
GO

IF OBJECT_ID('dbo.DeleteLocation', 'P') IS NOT NULL DROP PROCEDURE dbo.DeleteLocation;
GO
CREATE PROCEDURE dbo.DeleteLocation
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.LOCATION
    WHERE id = @id;
END;
GO

IF OBJECT_ID('dbo.GetLocations', 'P') IS NOT NULL DROP PROCEDURE dbo.GetLocations;
GO
CREATE PROCEDURE dbo.GetLocations
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.id,
        l.codeclient,
        c.nom + ' ' + c.prenom AS client,
        l.immatriculation,
        v.marque + ' ' + v.modele AS vehicule,
        l.datedebut,
        l.datefin,
        l.dateretour,
        l.kmparcourus,
        l.montant
    FROM dbo.LOCATION l
    INNER JOIN dbo.CLIENT c ON c.codeclient = l.codeclient
    INNER JOIN dbo.VEHICULE v ON v.immatriculation = l.immatriculation
    ORDER BY l.id DESC;
END;
GO

IF OBJECT_ID('dbo.SearchLocation', 'P') IS NOT NULL DROP PROCEDURE dbo.SearchLocation;
GO
CREATE PROCEDURE dbo.SearchLocation
    @keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.id,
        l.codeclient,
        c.nom + ' ' + c.prenom AS client,
        l.immatriculation,
        v.marque + ' ' + v.modele AS vehicule,
        l.datedebut,
        l.datefin,
        l.dateretour,
        l.kmparcourus,
        l.montant
    FROM dbo.LOCATION l
    INNER JOIN dbo.CLIENT c ON c.codeclient = l.codeclient
    INNER JOIN dbo.VEHICULE v ON v.immatriculation = l.immatriculation
    WHERE CAST(l.id AS NVARCHAR(20)) LIKE '%' + @keyword + '%'
       OR l.codeclient LIKE '%' + @keyword + '%'
       OR c.nom LIKE '%' + @keyword + '%'
       OR c.prenom LIKE '%' + @keyword + '%'
       OR l.immatriculation LIKE '%' + @keyword + '%'
       OR v.marque LIKE '%' + @keyword + '%'
       OR v.modele LIKE '%' + @keyword + '%'
    ORDER BY l.id DESC;
END;
GO

IF OBJECT_ID('dbo.ReturnVehicle', 'P') IS NOT NULL DROP PROCEDURE dbo.ReturnVehicle;
GO
CREATE PROCEDURE dbo.ReturnVehicle
    @id INT,
    @dateretour DATE,
    @kmparcourus INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @datedebut DATE;
    DECLARE @datefin DATE;
    DECLARE @categorie NVARCHAR(30);
    DECLARE @prix_jour DECIMAL(10, 2);
    DECLARE @prix_km DECIMAL(10, 2);
    DECLARE @nbjours INT;
    DECLARE @montant DECIMAL(12, 2);

    SELECT
        @datedebut = l.datedebut,
        @datefin = l.datefin,
        @categorie = v.categorie
    FROM dbo.LOCATION l
    INNER JOIN dbo.VEHICULE v ON v.immatriculation = l.immatriculation
    WHERE l.id = @id;

    SELECT
        @prix_jour = t.prix_jour,
        @prix_km = t.prix_km
    FROM dbo.TARIF t
    WHERE t.categorie = @categorie;

    SET @nbjours = DATEDIFF(DAY, @datedebut, ISNULL(@dateretour, @datefin)) + 1;
    IF @nbjours < 1 SET @nbjours = 1;

    SET @montant = (@nbjours * @prix_jour) + (@kmparcourus * @prix_km);

    UPDATE dbo.LOCATION
    SET dateretour = @dateretour,
        kmparcourus = @kmparcourus,
        montant = @montant
    WHERE id = @id;

    UPDATE v
    SET kilometrage = kilometrage + @kmparcourus
    FROM dbo.VEHICULE v
    INNER JOIN dbo.LOCATION l ON l.immatriculation = v.immatriculation
    WHERE l.id = @id;

    SELECT @montant AS montant_calcule;
END;
GO

IF OBJECT_ID('dbo.AuthenticateUser', 'P') IS NOT NULL DROP PROCEDURE dbo.AuthenticateUser;
GO
CREATE PROCEDURE dbo.AuthenticateUser
    @username NVARCHAR(50),
    @password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT id, username, [role]
    FROM dbo.UTILISATEUR
    WHERE username = @username
      AND [password] = @password;
END;
GO

-- ==================== PROCÉDURES POUR DISPONIBILITÉ VÉHICULE ====================

IF OBJECT_ID('dbo.GetAvailableVehicles', 'P') IS NOT NULL DROP PROCEDURE dbo.GetAvailableVehicles;
GO
CREATE PROCEDURE dbo.GetAvailableVehicles
    @datedebut DATE,
    @datefin DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT
        v.immatriculation,
        v.categorie,
        v.marque,
        v.modele,
        v.kilometrage,
        t.prix_jour,
        t.prix_km
    FROM dbo.VEHICULE v
    INNER JOIN dbo.TARIF t ON t.categorie = v.categorie
    WHERE v.disponible = 1
      AND v.immatriculation NOT IN (
          SELECT DISTINCT immatriculation
          FROM dbo.LOCATION
          WHERE dateretour IS NULL
            AND (
              (datedebut <= @datefin AND datefin >= @datedebut)
            )
      )
    ORDER BY v.categorie, v.marque, v.modele;
END;
GO

IF OBJECT_ID('dbo.IsVehicleAvailable', 'P') IS NOT NULL DROP PROCEDURE dbo.IsVehicleAvailable;
GO
CREATE PROCEDURE dbo.IsVehicleAvailable
    @immatriculation NVARCHAR(20),
    @datedebut DATE,
    @datefin DATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM dbo.LOCATION
        WHERE immatriculation = @immatriculation
          AND dateretour IS NULL
          AND (
            (datedebut <= @datefin AND datefin >= @datedebut)
          )
    )
    BEGIN
        SELECT 0 AS available;
    END
    ELSE IF NOT EXISTS (
        SELECT 1
        FROM dbo.VEHICULE
        WHERE immatriculation = @immatriculation AND disponible = 1
    )
    BEGIN
        SELECT 0 AS available;
    END
    ELSE
    BEGIN
        SELECT 1 AS available;
    END
END;
GO

IF OBJECT_ID('dbo.GetPendingRentals', 'P') IS NOT NULL DROP PROCEDURE dbo.GetPendingRentals;
GO
CREATE PROCEDURE dbo.GetPendingRentals
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.id,
        l.codeclient,
        c.nom + ' ' + c.prenom AS client,
        l.immatriculation,
        v.marque + ' ' + v.modele AS vehicule,
        l.datedebut,
        l.datefin,
        DATEDIFF(DAY, GETDATE(), l.datefin) AS jours_restants
    FROM dbo.LOCATION l
    INNER JOIN dbo.CLIENT c ON c.codeclient = l.codeclient
    INNER JOIN dbo.VEHICULE v ON v.immatriculation = l.immatriculation
    WHERE l.dateretour IS NULL
    ORDER BY l.datefin ASC;
END;
GO

-- ==================== PROCÉDURES DASHBOARD STATISTIQUES ====================

IF OBJECT_ID('dbo.GetClientCount', 'P') IS NOT NULL DROP PROCEDURE dbo.GetClientCount;
GO
CREATE PROCEDURE dbo.GetClientCount
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS total_clients
    FROM dbo.CLIENT;
END;
GO

IF OBJECT_ID('dbo.GetVehicleCount', 'P') IS NOT NULL DROP PROCEDURE dbo.GetVehicleCount;
GO
CREATE PROCEDURE dbo.GetVehicleCount
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS total_vehicles
    FROM dbo.VEHICULE;
END;
GO

IF OBJECT_ID('dbo.GetActiveRentalsCount', 'P') IS NOT NULL DROP PROCEDURE dbo.GetActiveRentalsCount;
GO
CREATE PROCEDURE dbo.GetActiveRentalsCount
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS active_rentals
    FROM dbo.LOCATION
    WHERE dateretour IS NULL;
END;
GO

IF OBJECT_ID('dbo.GetTotalRevenue', 'P') IS NOT NULL DROP PROCEDURE dbo.GetTotalRevenue;
GO
CREATE PROCEDURE dbo.GetTotalRevenue
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ISNULL(SUM(montant), 0) AS total_revenue
    FROM dbo.LOCATION;
END;
GO
