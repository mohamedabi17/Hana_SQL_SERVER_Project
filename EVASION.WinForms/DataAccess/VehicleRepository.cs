using System.Data;
using EVASION.WinForms.Models;

namespace EVASION.WinForms.DataAccess;

public class VehicleRepository
{
    public DataTable GetVehicles() => DbHelper.ExecuteDataTable("GetVehicles");

    public DataTable SearchVehicle(string keyword) => DbHelper.ExecuteDataTable("SearchVehicle", parameters =>
    {
        parameters.AddWithValue("@keyword", keyword);
    });

    public void AddVehicle(Vehicle vehicle)
    {
        DbHelper.ExecuteNonQuery("AddVehicle", parameters =>
        {
            parameters.AddWithValue("@immatriculation", vehicle.Immatriculation);
            parameters.AddWithValue("@categorie", vehicle.Categorie);
            parameters.AddWithValue("@marque", vehicle.Marque);
            parameters.AddWithValue("@modele", vehicle.Modele);
            parameters.AddWithValue("@kilometrage", vehicle.Kilometrage);
        });
    }

    public void UpdateVehicle(Vehicle vehicle)
    {
        DbHelper.ExecuteNonQuery("UpdateVehicle", parameters =>
        {
            parameters.AddWithValue("@immatriculation", vehicle.Immatriculation);
            parameters.AddWithValue("@categorie", vehicle.Categorie);
            parameters.AddWithValue("@marque", vehicle.Marque);
            parameters.AddWithValue("@modele", vehicle.Modele);
            parameters.AddWithValue("@kilometrage", vehicle.Kilometrage);
        });
    }

    public void DeleteVehicle(string immatriculation)
    {
        DbHelper.ExecuteNonQuery("DeleteVehicle", parameters =>
        {
            parameters.AddWithValue("@immatriculation", immatriculation);
        });
    }
}