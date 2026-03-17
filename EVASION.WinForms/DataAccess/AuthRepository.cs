using EVASION.WinForms.Models;
using Microsoft.Data.SqlClient;

namespace EVASION.WinForms.DataAccess;

public class AuthRepository
{
    public User? Authenticate(string username, string password)
    {
        using SqlDataReader reader = DbHelper.ExecuteReader("AuthenticateUser", parameters =>
        {
            parameters.AddWithValue("@username", username);
            parameters.AddWithValue("@password", password);
        });

        if (!reader.Read())
        {
            return null;
        }

        return new User
        {
            Id = Convert.ToInt32(reader["id"]),
            Username = reader["username"].ToString() ?? string.Empty,
            Role = reader["role"].ToString() ?? string.Empty
        };
    }
}