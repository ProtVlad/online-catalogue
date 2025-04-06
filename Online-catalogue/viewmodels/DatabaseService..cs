using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;
using Online_catalogue.Models;

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

    public void InsertUser(string nume, string prenume, string rol, string email, string parola, DateTime createdAt)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO users (nume, prenume, rol, email, parola, created_at) " +
                           "VALUES (@nume, @prenume, @rol, @email, @parola, @createdAt)";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nume", nume);
                cmd.Parameters.AddWithValue("@prenume", prenume);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@parola", parola);
                cmd.Parameters.AddWithValue("@createdAt", createdAt);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<User> GetUsers()
    {
        List<User> users = new List<User>();

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT id, nume, prenume, rol, parola, email FROM users"; // Modificat pentru a include parola

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0), // id
                            Nume = reader.GetString(1), // last_name -> Nume
                            Prenume = reader.GetString(2), // first_name -> Prenume
                            Rol = reader.GetString(3), // role -> Rol
                            Parola = reader.GetString(4), // password -> Parola
                            Email = reader.GetString(5) // email -> Email
                        });
                    }
                }
            }
        }

        return users;
    }

    public void UpdateUser(User user)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "UPDATE users SET nume = @LastName, prenume = @FirstName, rol = @Role, email = @Email, parola = @Password WHERE id = @Id";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", user.Id);
                cmd.Parameters.AddWithValue("@LastName", user.Nume);
                cmd.Parameters.AddWithValue("@FirstName", user.Prenume);
                cmd.Parameters.AddWithValue("@Role", user.Rol);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Parola);

                cmd.ExecuteNonQuery();
            }
        }
    }


    public void DeleteUser(int userId)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "DELETE FROM users WHERE id = @id";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("id", userId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
