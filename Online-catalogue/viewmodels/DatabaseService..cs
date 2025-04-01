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

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS users (
                    id SERIAL PRIMARY KEY,
                    nume VARCHAR(50) NOT NULL,
                    prenume VARCHAR(50) NOT NULL,
                    rol VARCHAR(20) NOT NULL,
                    parola TEXT NOT NULL,
                    email VARCHAR(100) UNIQUE NOT NULL
                );";

                using (var cmd = new NpgsqlCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                createTableQuery = @"
                CREATE TABLE IF NOT EXISTS curs (
                    id SERIAL PRIMARY KEY,
                    nume_curs VARCHAR(100) NOT NULL,
                    descriere TEXT
                );";

                using (var cmd = new NpgsqlCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                createTableQuery = @"
                CREATE TABLE IF NOT EXISTS user_curs (
                    id_user INT NOT NULL,
                    id_curs INT NOT NULL,
                    PRIMARY KEY (id_user, id_curs),
                    FOREIGN KEY (id_user) REFERENCES users(id) ON DELETE CASCADE,
                    FOREIGN KEY (id_curs) REFERENCES curs(id) ON DELETE CASCADE
                );";

                using (var cmd = new NpgsqlCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                createTableQuery = @"
                CREATE TABLE IF NOT EXISTS nota (
                    id SERIAL PRIMARY KEY,
                    id_user INT NOT NULL,
                    id_curs INT NOT NULL,
                    nota INT NOT NULL CHECK (nota >= 1 AND nota <= 10),
                    FOREIGN KEY (id_user) REFERENCES users(id) ON DELETE CASCADE,
                    FOREIGN KEY (id_curs) REFERENCES curs(id) ON DELETE CASCADE
                );";

                using (var cmd = new NpgsqlCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }

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
