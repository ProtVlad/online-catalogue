using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
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

                string checkUsersQuery = "SELECT COUNT(*) FROM users;";
                using (var checkCmd = new NpgsqlCommand(checkUsersQuery, conn))
                {
                    var userCount = (long)checkCmd.ExecuteScalar();
                    if (userCount == 0)
                    {
                        string insertAdminQuery = @"
                            INSERT INTO users (nume, prenume, rol, parola, email)
                            VALUES ('Admin', 'Principal', 'admin', 'admin123', 'admin@example.com');";

                        using (var insertCmd = new NpgsqlCommand(insertAdminQuery, conn))
                        {
                            insertCmd.ExecuteNonQuery();
                        }

                    }
                }

                MessageBox.Show("Conexiune reusita!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public User AuthenticateUser(string email, string password)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT id, nume, prenume, email, parola, rol FROM users WHERE email = @Email";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedPassword = reader.GetString(reader.GetOrdinal("parola"));

                        if (password == storedPassword)
                        {
                            return new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Nume = reader.GetString(reader.GetOrdinal("nume")),
                                Prenume = reader.GetString(reader.GetOrdinal("prenume")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                Parola = storedPassword,
                                Rol = reader.GetString(reader.GetOrdinal("rol"))
                            };
                        }
                    }
                }
            }
        }

        return null;
    }


    public void InsertUser(string nume, string prenume, string rol, string email, string parola)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO users (nume, prenume, rol, email, parola) " +
                           "VALUES (@nume, @prenume, @rol, @email, @parola)";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nume", nume);
                cmd.Parameters.AddWithValue("@prenume", prenume);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@parola", parola);

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

    public void UpdateUserPassword(int userId, string newPassword)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string query = "UPDATE users SET parola = @Password WHERE id = @UserId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Password", newPassword);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }
    }
    public List<Nota> GetNote(int userId, int courseId)
    {
        List<Nota> note = new List<Nota>();

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT id, id_user, id_curs, nota FROM nota WHERE id_user = @userId AND id_curs = @courseId";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@courseId", courseId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        note.Add(new Nota
                        {
                            Id = reader.GetInt32(0),
                            IdUser = reader.GetInt32(1),
                            IdCurs = reader.GetInt32(2),
                            NotaValoare = reader.GetInt32(3)
                        });
                    }
                }
            }
        }

        return note;
    }

    public List<Nota> GetNote(int courseId)
    {
        List<Nota> note = new List<Nota>();

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT id, id_user, id_curs, nota FROM nota WHERE id_curs = @courseId";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@courseId", courseId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        note.Add(new Nota
                        {
                            Id = reader.GetInt32(0),
                            IdUser = reader.GetInt32(1),
                            IdCurs = reader.GetInt32(2),
                            NotaValoare = reader.GetInt32(3)
                        });
                    }
                }
            }
        }

        return note;
    }


    public void AddNota(Nota nota)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO nota (id_user, id_curs, nota) VALUES (@idUser, @idCurs, @nota)";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idUser", nota.IdUser);
                cmd.Parameters.AddWithValue("@idCurs", nota.IdCurs);
                cmd.Parameters.AddWithValue("@nota", nota.NotaValoare);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<User> GetUsersByCourseId(int courseId)
    {
        List<User> users = new List<User>();

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = @"
            SELECT u.id, u.nume, u.prenume, u.rol, u.parola, u.email 
            FROM users u
            JOIN user_curs uc ON u.id = uc.id_user
            WHERE uc.id_curs = @courseId";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@courseId", courseId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Nume = reader.GetString(1),
                            Prenume = reader.GetString(2),
                            Rol = reader.GetString(3),
                            Parola = reader.GetString(4),
                            Email = reader.GetString(5)
                        });
                    }
                }
            }
        }

        return users;
    }

    public List<Curs> GetCourses()
    {
        List<Curs> courses = new List<Curs>();

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT id, nume_curs, descriere FROM curs";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(new Curs
                        {
                            Id = reader.GetInt32(0),
                            NumeCurs = reader.GetString(1),
                            Descriere = reader.IsDBNull(2) ? null : reader.GetString(2)
                        });
                    }
                }
            }
        }

        return courses;
    }


    public string GetCourseName(int cursId)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT NumeCurs FROM Curs WHERE Id = @Id";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", cursId);

                var result = cmd.ExecuteScalar();
                return result?.ToString();
            }
        }
    }


    public List<Nota> GetNotesForStudent(int studentId)
    {
        List<Nota> notes = new List<Nota>();

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT id, id_user, id_curs, nota FROM nota WHERE id_user = @studentId";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@studentId", studentId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notes.Add(new Nota
                        {
                            Id = reader.GetInt32(0),
                            IdUser = reader.GetInt32(1),
                            IdCurs = reader.GetInt32(2),
                            NotaValoare = reader.GetInt32(3)
                        });
                    }
                }
            }
        }

        return notes;
    }

    public string GetCourseNameById(int courseId)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT nume_curs FROM curs WHERE id = @id";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", courseId);
                return cmd.ExecuteScalar()?.ToString() ?? "N/A";
            }
        }
    }




}
