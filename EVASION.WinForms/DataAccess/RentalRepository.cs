using System.Data;
using EVASION.WinForms.Models;

namespace EVASION.WinForms.DataAccess;

public class RentalRepository
{
    public DataTable GetLocations() => DbHelper.ExecuteDataTable("GetLocations");

    public DataTable SearchLocation(string keyword) => DbHelper.ExecuteDataTable("SearchLocation", parameters =>
    {
        parameters.AddWithValue("@keyword", keyword);
    });

    public void AddLocation(Rental rental)
    {
        DbHelper.ExecuteNonQuery("AddLocation", parameters =>
        {
            parameters.AddWithValue("@codeclient", rental.CodeClient);
            parameters.AddWithValue("@immatriculation", rental.Immatriculation);
            parameters.AddWithValue("@datedebut", rental.DateDebut);
            parameters.AddWithValue("@datefin", rental.DateFin);
        });
    }

    public void UpdateLocation(Rental rental)
    {
        DbHelper.ExecuteNonQuery("UpdateLocation", parameters =>
        {
            parameters.AddWithValue("@id", rental.Id);
            parameters.AddWithValue("@codeclient", rental.CodeClient);
            parameters.AddWithValue("@immatriculation", rental.Immatriculation);
            parameters.AddWithValue("@datedebut", rental.DateDebut);
            parameters.AddWithValue("@datefin", rental.DateFin);
        });
    }

    public void DeleteLocation(int id)
    {
        DbHelper.ExecuteNonQuery("DeleteLocation", parameters =>
        {
            parameters.AddWithValue("@id", id);
        });
    }

    public decimal ReturnVehicle(int id, DateTime dateRetour, int kmParcourus)
    {
        object? value = DbHelper.ExecuteScalar("ReturnVehicle", parameters =>
        {
            parameters.AddWithValue("@id", id);
            parameters.AddWithValue("@dateretour", dateRetour);
            parameters.AddWithValue("@kmparcourus", kmParcourus);
        });

        return value == null || value == DBNull.Value ? 0 : Convert.ToDecimal(value);
    }

    /// <summary>
    /// Récupère les véhicules disponibles pour une période donnée
    /// </summary>
    public DataTable GetAvailableVehicles(DateTime dateDebut, DateTime dateFin)
    {
        return DbHelper.ExecuteDataTable("GetAvailableVehicles", parameters =>
        {
            parameters.AddWithValue("@datedebut", dateDebut);
            parameters.AddWithValue("@datefin", dateFin);
        });
    }

    /// <summary>
    /// Vérifie si un véhicule est disponible pour une période donnée
    /// </summary>
    public bool IsVehicleAvailable(string immatriculation, DateTime dateDebut, DateTime dateFin)
    {
        object? result = DbHelper.ExecuteScalar("IsVehicleAvailable", parameters =>
        {
            parameters.AddWithValue("@immatriculation", immatriculation);
            parameters.AddWithValue("@datedebut", dateDebut);
            parameters.AddWithValue("@datefin", dateFin);
        });

        return result != null && result != DBNull.Value && Convert.ToInt32(result) == 1;
    }

    /// <summary>
    /// Récupère les locations en attente de retour
    /// </summary>
    public DataTable GetPendingRentals() => DbHelper.ExecuteDataTable("GetPendingRentals");
}
