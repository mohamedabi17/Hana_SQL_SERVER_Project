using System.Data;
using Microsoft.Data.SqlClient;

namespace EVASION.WinForms.DataAccess;

public static class DbHelper
{
    public static DataTable ExecuteDataTable(string storedProcedure, Action<SqlParameterCollection>? addParameters = null)
    {
        using SqlConnection connection = Database.GetConnection();
        using SqlCommand command = CreateStoredProcedureCommand(connection, storedProcedure, addParameters);
        using SqlDataAdapter adapter = new(command);
        DataTable table = new();
        adapter.Fill(table);
        return table;
    }

    public static int ExecuteNonQuery(string storedProcedure, Action<SqlParameterCollection>? addParameters = null)
    {
        using SqlConnection connection = Database.GetConnection();
        using SqlCommand command = CreateStoredProcedureCommand(connection, storedProcedure, addParameters);
        connection.Open();
        return command.ExecuteNonQuery();
    }

    public static object? ExecuteScalar(string storedProcedure, Action<SqlParameterCollection>? addParameters = null)
    {
        using SqlConnection connection = Database.GetConnection();
        using SqlCommand command = CreateStoredProcedureCommand(connection, storedProcedure, addParameters);
        connection.Open();
        return command.ExecuteScalar();
    }

    public static SqlDataReader ExecuteReader(string storedProcedure, Action<SqlParameterCollection>? addParameters = null)
    {
        SqlConnection connection = Database.GetConnection();
        SqlCommand command = CreateStoredProcedureCommand(connection, storedProcedure, addParameters);
        connection.Open();
        return command.ExecuteReader(CommandBehavior.CloseConnection);
    }

    private static SqlCommand CreateStoredProcedureCommand(SqlConnection connection, string storedProcedure, Action<SqlParameterCollection>? addParameters)
    {
        SqlCommand command = new(storedProcedure, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        addParameters?.Invoke(command.Parameters);
        return command;
    }
}