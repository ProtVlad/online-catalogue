using System;
using System.Windows;
using Npgsql;

public class DatabaseService
{
    private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1q2w3e;Database=online-catalogue";

    public void TestConnection()
    {
        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                MessageBox.Show("Conexiune reusita!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public string AuthenticateUser(string email, string password)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT parola, rol FROM users WHERE email = @Email";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedPassword = reader.GetString(0);
                        string role = reader.GetString(1);

                        if (password == storedPassword)
                        {
                            return role;
                        }
                    }
                }
            }
        }
        return null;
    }
}
