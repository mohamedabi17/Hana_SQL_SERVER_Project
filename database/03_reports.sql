USE EVASIONDB;
GO

IF OBJECT_ID('dbo.ReportTotalRentalsPerVehicle', 'P') IS NOT NULL DROP PROCEDURE dbo.ReportTotalRentalsPerVehicle;
GO
CREATE PROCEDURE dbo.ReportTotalRentalsPerVehicle
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.immatriculation,
        v.marque,
        v.modele,
        COUNT(*) AS total_locations
    FROM dbo.LOCATION l
    INNER JOIN dbo.VEHICULE v ON v.immatriculation = l.immatriculation
    GROUP BY l.immatriculation, v.marque, v.modele
    ORDER BY total_locations DESC, v.marque;
END;
GO

IF OBJECT_ID('dbo.ReportRevenuePerCategory', 'P') IS NOT NULL DROP PROCEDURE dbo.ReportRevenuePerCategory;
GO
CREATE PROCEDURE dbo.ReportRevenuePerCategory
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.categorie,
        COUNT(l.id) AS total_locations,
        SUM(ISNULL(l.montant, 0)) AS chiffre_affaires
    FROM dbo.VEHICULE v
    LEFT JOIN dbo.LOCATION l ON l.immatriculation = v.immatriculation
    GROUP BY v.categorie
    ORDER BY chiffre_affaires DESC;
END;
GO

IF OBJECT_ID('dbo.ReportMostRentedVehicles', 'P') IS NOT NULL DROP PROCEDURE dbo.ReportMostRentedVehicles;
GO
CREATE PROCEDURE dbo.ReportMostRentedVehicles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 10
        l.immatriculation,
        v.marque,
        v.modele,
        COUNT(*) AS nombre_locations,
        SUM(ISNULL(l.montant, 0)) AS revenu_total
    FROM dbo.LOCATION l
    INNER JOIN dbo.VEHICULE v ON v.immatriculation = l.immatriculation
    GROUP BY l.immatriculation, v.marque, v.modele
    ORDER BY nombre_locations DESC, revenu_total DESC;
END;
GO

IF OBJECT_ID('dbo.ReportRentalsByClient', 'P') IS NOT NULL DROP PROCEDURE dbo.ReportRentalsByClient;
GO
CREATE PROCEDURE dbo.ReportRentalsByClient
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.codeclient,
        c.nom,
        c.prenom,
        c.typeclient,
        COUNT(l.id) AS total_locations,
        SUM(ISNULL(l.montant, 0)) AS montant_total
    FROM dbo.CLIENT c
    LEFT JOIN dbo.LOCATION l ON l.codeclient = c.codeclient
    GROUP BY c.codeclient, c.nom, c.prenom, c.typeclient
    ORDER BY total_locations DESC, montant_total DESC;
END;
GO

-- ==================== RAPPORTS AVANCÉS ====================

IF OBJECT_ID('dbo.ReportPendingRentals', 'P') IS NOT NULL DROP PROCEDURE dbo.ReportPendingRentals;
GO
CREATE PROCEDURE dbo.ReportPendingRentals
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.id,
        c.nom + ' ' + c.prenom AS client,
        v.marque + ' ' + v.modele AS vehicule,
        l.datedebut,
        l.datefin,
        DATEDIFF(DAY, GETDATE(), l.datefin) AS jours_restants,
        CASE 
            WHEN DATEDIFF(DAY, GETDATE(), l.datefin) < 0 THEN 'RETARD'
            WHEN DATEDIFF(DAY, GETDATE(), l.datefin) <= 1 THEN 'URGENT'
            ELSE 'OK'
        END AS statut
    FROM dbo.LOCATION l
    INNER JOIN dbo.CLIENT c ON c.codeclient = l.codeclient
    INNER JOIN dbo.VEHICULE v ON v.immatriculation = l.immatriculation
    WHERE l.dateretour IS NULL
    ORDER BY l.datefin ASC;
END;
GO

IF OBJECT_ID('dbo.ReportRevenueByPeriod', 'P') IS NOT NULL DROP PROCEDURE dbo.ReportRevenueByPeriod;
GO
CREATE PROCEDURE dbo.ReportRevenueByPeriod
    @date_start DATE,
    @date_end DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CAST(l.datedebut AS DATE) AS date_location,
        COUNT(*) AS nombre_locations,
        SUM(ISNULL(l.montant, 0)) AS chiffre_affaires,
        AVG(ISNULL(l.montant, 0)) AS montant_moyen
    FROM dbo.LOCATION l
    WHERE l.dateretour IS NOT NULL
      AND l.dateretour BETWEEN @date_start AND @date_end
    GROUP BY CAST(l.datedebut AS DATE)
    ORDER BY date_location DESC;
END;
GO

IF OBJECT_ID('dbo.ReportLoyalClients', 'P') IS NOT NULL DROP PROCEDURE dbo.ReportLoyalClients;
GO
CREATE PROCEDURE dbo.ReportLoyalClients
    @min_rentals INT = 3
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 20
        c.codeclient,
        c.nom + ' ' + c.prenom AS client,
        c.typeclient,
        COUNT(l.id) AS nombre_locations,
        SUM(ISNULL(l.montant, 0)) AS total_depense,
        MAX(l.dateretour) AS derniere_location,
        DATEDIFF(DAY, MAX(l.dateretour), GETDATE()) AS jours_depuis_location
    FROM dbo.CLIENT c
    INNER JOIN dbo.LOCATION l ON l.codeclient = c.codeclient
    WHERE l.dateretour IS NOT NULL
    GROUP BY c.codeclient, c.nom, c.prenom, c.typeclient
    HAVING COUNT(l.id) >= @min_rentals
    ORDER BY nombre_locations DESC, total_depense DESC;
END;
GO

IF OBJECT_ID('dbo.ReportVehicleUsageRate', 'P') IS NOT NULL DROP PROCEDURE dbo.ReportVehicleUsageRate;
GO
CREATE PROCEDURE dbo.ReportVehicleUsageRate
    @days INT = 90
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.immatriculation,
        v.marque + ' ' + v.modele AS vehicule,
        v.categorie,
        COUNT(l.id) AS nombre_locations,
        CONVERT(DECIMAL(5,2), (COUNT(l.id) * 100.0 / @days)) AS taux_utilisation_percent,
        SUM(ISNULL(l.montant, 0)) AS revenu_total,
        v.kilometrage AS km_total,
        CONVERT(DECIMAL(8,2), AVG(ISNULL(l.montant, 0))) AS montant_moyen_location
    FROM dbo.VEHICULE v
    LEFT JOIN dbo.LOCATION l ON l.immatriculation = v.immatriculation
      AND l.dateretour >= DATEADD(DAY, -@days, GETDATE())
    GROUP BY v.immatriculation, v.marque, v.modele, v.categorie, v.kilometrage
    ORDER BY taux_utilisation_percent DESC, revenu_total DESC;
END;
GO

IF OBJECT_ID('dbo.ReportCategoryPerformance', 'P') IS NOT NULL DROP PROCEDURE dbo.ReportCategoryPerformance;
GO
CREATE PROCEDURE dbo.ReportCategoryPerformance
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.categorie,
        COUNT(DISTINCT v.immatriculation) AS nombre_vehicules,
        COUNT(l.id) AS total_locations,
        SUM(ISNULL(l.montant, 0)) AS chiffre_affaires,
        CONVERT(DECIMAL(8,2), AVG(ISNULL(l.montant, 0))) AS montant_moyen,
        CONVERT(DECIMAL(8,2), SUM(ISNULL(l.montant, 0)) / NULLIF(COUNT(DISTINCT v.immatriculation), 0)) AS revenu_par_vehicule
    FROM dbo.VEHICULE v
    LEFT JOIN dbo.LOCATION l ON l.immatriculation = v.immatriculation
      AND l.dateretour IS NOT NULL
    GROUP BY v.categorie
    ORDER BY chiffre_affaires DESC;
END;
GO
