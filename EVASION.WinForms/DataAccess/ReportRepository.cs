using System.Data;

namespace EVASION.WinForms.DataAccess;

public class ReportRepository
{
    public DataTable TotalRentalsPerVehicle() => DbHelper.ExecuteDataTable("ReportTotalRentalsPerVehicle");

    public DataTable RevenuePerCategory() => DbHelper.ExecuteDataTable("ReportRevenuePerCategory");

    public DataTable MostRentedVehicles() => DbHelper.ExecuteDataTable("ReportMostRentedVehicles");

    public DataTable RentalsByClient() => DbHelper.ExecuteDataTable("ReportRentalsByClient");

    /// <summary>
    /// Récupère les locations en attente de retour avec statut d'urgence
    /// </summary>
    public DataTable GetPendingRentals() => DbHelper.ExecuteDataTable("ReportPendingRentals");

    /// <summary>
    /// Récupère le chiffre d'affaires par période
    /// </summary>
    public DataTable GetRevenueByPeriod(DateTime dateStart, DateTime dateEnd)
    {
        return DbHelper.ExecuteDataTable("ReportRevenueByPeriod", parameters =>
        {
            parameters.AddWithValue("@date_start", dateStart);
            parameters.AddWithValue("@date_end", dateEnd);
        });
    }

    /// <summary>
    /// Récupère les clients fidèles ayant au moins N locations
    /// </summary>
    public DataTable GetLoyalClients(int minRentals = 3)
    {
        return DbHelper.ExecuteDataTable("ReportLoyalClients", parameters =>
        {
            parameters.AddWithValue("@min_rentals", minRentals);
        });
    }

    /// <summary>
    /// Récupère le taux d'utilisation des véhicules pour les N derniers jours
    /// </summary>
    public DataTable GetVehicleUsageRate(int days = 90)
    {
        return DbHelper.ExecuteDataTable("ReportVehicleUsageRate", parameters =>
        {
            parameters.AddWithValue("@days", days);
        });
    }

    /// <summary>
    /// Récupère la performance par catégorie de véhicule
    /// </summary>
    public DataTable GetCategoryPerformance() => DbHelper.ExecuteDataTable("ReportCategoryPerformance");

    public int GetClientCount()
    {
        object? result = DbHelper.ExecuteScalar("GetClientCount");
        return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
    }

    public int GetVehicleCount()
    {
        object? result = DbHelper.ExecuteScalar("GetVehicleCount");
        return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
    }

    public int GetActiveRentalsCount()
    {
        object? result = DbHelper.ExecuteScalar("GetActiveRentalsCount");
        return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
    }

    public decimal GetTotalRevenue()
    {
        object? result = DbHelper.ExecuteScalar("GetTotalRevenue");
        return result == null || result == DBNull.Value ? 0 : Convert.ToDecimal(result);
    }
}
