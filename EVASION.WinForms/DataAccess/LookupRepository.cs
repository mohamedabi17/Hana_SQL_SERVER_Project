using System.Data;
using Microsoft.Data.SqlClient;

namespace EVASION.WinForms.DataAccess;

public class LookupRepository
{
    public DataTable GetTariffCategories()
    {
        return ExecuteTextQuery("SELECT categorie FROM dbo.TARIF ORDER BY categorie;");
    }

    public DataTable GetClientLookup()
    {
        return ExecuteTextQuery("SELECT codeclient, nom + ' ' + prenom AS client FROM dbo.CLIENT ORDER BY nom, prenom;");
    }

    public DataTable GetVehicleLookup()
    {
        return ExecuteTextQuery("SELECT immatriculation, marque + ' ' + modele AS vehicule FROM dbo.VEHICULE ORDER BY marque, modele;");
    }

    private static DataTable ExecuteTextQuery(string sql)
    {
        using SqlConnection connection = Database.GetConnection();
        using SqlCommand command = new(sql, connection);
        using SqlDataAdapter adapter = new(command);
        DataTable table = new();
        adapter.Fill(table);
        return table;
    }
}