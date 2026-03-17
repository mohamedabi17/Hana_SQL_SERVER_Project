-- ==================== TEST DASHBOARD STATISTICS ====================
-- Exécutez ce script dans SQL Server Management Studio (SSMS)
-- Base de données : EVASIONDB

USE EVASIONDB;
GO

-- Test 1 : Total clients
PRINT '=== Test 1 : Total Clients ===';
EXEC GetClientCount;
GO

-- Test 2 : Total véhicules
PRINT '=== Test 2 : Total Véhicules ===';
EXEC GetVehicleCount;
GO

-- Test 3 : Locations actives (non retournées)
PRINT '=== Test 3 : Locations Actives ===';
EXEC GetActiveRentalsCount;
GO

-- Test 4 : Chiffre d'affaires total
PRINT '=== Test 4 : Chiffre d''Affaires Total ===';
EXEC GetTotalRevenue;
GO

-- Résultats attendus (basé sur données de test) :
-- Test 1 : 8 clients
-- Test 2 : 12 véhicules  
-- Test 3 : 4 locations actives (celles sans dateretour)
-- Test 4 : ~4416.00 € (somme des montants complétés)

-- ==================== VALIDATION ====================

PRINT '';
PRINT '✓ Si vous voyez 4 résultats ci-dessus → Tout fonctionne !';
PRINT '✓ Sinon, vérifier que GetClientCount, GetVehicleCount, etc. existent';
PRINT '';

-- Vérifier que les procédures existent
PRINT '=== Vérification des procédures ===';
SELECT 
    name,
    type_desc
FROM sys.objects
WHERE type = 'P' 
  AND name IN ('GetClientCount', 'GetVehicleCount', 'GetActiveRentalsCount', 'GetTotalRevenue')
ORDER BY name;
GO
