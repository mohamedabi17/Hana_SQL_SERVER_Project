using System.Configuration;
using Microsoft.Data.SqlClient;

namespace EVASION.WinForms.DataAccess;

public static class Database
{
    public static SqlConnection GetConnection()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["EVASIONDb"]?.ConnectionString
            ?? throw new InvalidOperationException("La chaîne de connexion EVASIONDb est introuvable dans App.config.");

        return new SqlConnection(connectionString);
    }
}