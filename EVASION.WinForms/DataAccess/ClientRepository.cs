using System.Data;
using EVASION.WinForms.Models;

namespace EVASION.WinForms.DataAccess;

public class ClientRepository
{
    public DataTable GetClients() => DbHelper.ExecuteDataTable("GetClients");

    public DataTable SearchClient(string keyword) => DbHelper.ExecuteDataTable("SearchClient", parameters =>
    {
        parameters.AddWithValue("@keyword", keyword);
    });

    public void AddClient(Client client)
    {
        DbHelper.ExecuteNonQuery("AddClient", parameters =>
        {
            parameters.AddWithValue("@codeclient", client.CodeClient);
            parameters.AddWithValue("@nom", client.Nom);
            parameters.AddWithValue("@prenom", client.Prenom);
            parameters.AddWithValue("@adresse", client.Adresse);
            parameters.AddWithValue("@telephone", client.Telephone);
            parameters.AddWithValue("@typeclient", client.TypeClient);
        });
    }

    public void UpdateClient(Client client)
    {
        DbHelper.ExecuteNonQuery("UpdateClient", parameters =>
        {
            parameters.AddWithValue("@codeclient", client.CodeClient);
            parameters.AddWithValue("@nom", client.Nom);
            parameters.AddWithValue("@prenom", client.Prenom);
            parameters.AddWithValue("@adresse", client.Adresse);
            parameters.AddWithValue("@telephone", client.Telephone);
            parameters.AddWithValue("@typeclient", client.TypeClient);
        });
    }

    public void DeleteClient(string codeClient)
    {
        DbHelper.ExecuteNonQuery("DeleteClient", parameters =>
        {
            parameters.AddWithValue("@codeclient", codeClient);
        });
    }
}