using System.Data;
using InmobiliariaUrquiULP_v1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace InmobiliariaUrquiULP_v1.Models;

public class RepositorioInquilino : RepositorioBase
{
    //------------------------------ABM------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public int Alta(Inquilino i)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    INSERT INTO Inquilino (
                        {nameof(Inquilino.Dni)}, 
                        {nameof(Inquilino.Nombre)}, 
                        {nameof(Inquilino.Apellido)}, 
                        {nameof(Inquilino.Telefono)}, 
                        {nameof(Inquilino.Email)}, 
                        {nameof(Inquilino.Estado)}) 
                    VALUES (@Dni, @Nombre, @Apellido, @Telefono, @Email, true);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Dni", i.Dni);
                    command.Parameters.AddWithValue("@Nombre", i.Nombre);
                    command.Parameters.AddWithValue("@Apellido", i.Apellido);
                    command.Parameters.AddWithValue("@Telefono", i.Telefono);
                    command.Parameters.AddWithValue("@Email", i.Email);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.IdInquilino = res;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySqlException: {ex.Number} - {ex.Message}");
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        return res;
    }

    public int Baja(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                string sql = @$"
                    UPDATE Inquilino 
                    SET {nameof(Inquilino.Estado)}=false
                    WHERE {nameof(Inquilino.IdInquilino)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySqlException: {ex.Number} - {ex.Message}");
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        return res;
    }

    public int Modificacion(Inquilino i)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                string sql = @$"
                    UPDATE Inquilino 
                    SET 
                        {nameof(Inquilino.Dni)}=@Dni, 
                        {nameof(Inquilino.Nombre)}=@Nombre, 
                        {nameof(Inquilino.Apellido)}=@Apellido, 
                        {nameof(Inquilino.Telefono)}=@Telefono, 
                        {nameof(Inquilino.Email)}=@Email
                    WHERE {nameof(Inquilino.IdInquilino)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Dni", i.Dni);
                    command.Parameters.AddWithValue("@Nombre", i.Nombre);
                    command.Parameters.AddWithValue("@Apellido", i.Apellido);
                    command.Parameters.AddWithValue("@Telefono", i.Telefono);
                    command.Parameters.AddWithValue("@Email", i.Email);
                    command.Parameters.AddWithValue("@id", i.IdInquilino);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySqlException: {ex.Number} - {ex.Message}");
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        return res;
    }

    //---------------------------LISTAS------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public List<Inquilino> Listar()
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Inquilino.IdInquilino)}, 
                        {nameof(Inquilino.Dni)}, 
                        {nameof(Inquilino.Nombre)}, 
                        {nameof(Inquilino.Apellido)}, 
                        {nameof(Inquilino.Telefono)}, 
                        {nameof(Inquilino.Email)}
                    FROM Inquilino
                    WHERE {nameof(Inquilino.Estado)} = true";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        inquilinos.Add(new Inquilino
                        {
                            IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
                            Dni = reader.GetInt32(nameof(Inquilino.Dni)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Email = reader.GetString(nameof(Inquilino.Email))
                        });
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySqlException: {ex.Number} - {ex.Message}");
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        return inquilinos;
    }

    public List<Inquilino> ListarPorDniNombreApellido(string exp)
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Inquilino.IdInquilino)}, 
                        {nameof(Inquilino.Dni)}, 
                        {nameof(Inquilino.Nombre)}, 
                        {nameof(Inquilino.Apellido)}, 
                        {nameof(Inquilino.Telefono)}, 
                        {nameof(Inquilino.Email)},
                        {nameof(Inquilino.Estado)}
                    FROM Inquilino
                    WHERE {nameof(Inquilino.Estado)} = true
                    AND ({nameof(Inquilino.Nombre)} LIKE '%{exp}%' 
                    OR {nameof(Inquilino.Apellido)} LIKE '%{exp}%' 
                    OR CAST({nameof(Inquilino.Dni)} AS CHAR) LIKE '%{exp}%')";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        inquilinos.Add(new Inquilino
                        {
                            IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
                            Dni = reader.GetInt32(nameof(Inquilino.Dni)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Estado = reader.GetBoolean(nameof(Inquilino.Estado))
                        });
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySqlException: {ex.Number} - {ex.Message}");
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        return inquilinos;
    }


    //---------------------------BUSCAR------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public Inquilino? BuscarPorId(int id)
    {
        Inquilino? i = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Inquilino.IdInquilino)}, 
                        {nameof(Inquilino.Dni)}, 
                        {nameof(Inquilino.Nombre)}, 
                        {nameof(Inquilino.Apellido)}, 
                        {nameof(Inquilino.Telefono)}, 
                        {nameof(Inquilino.Email)},
                        {nameof(Inquilino.Estado)}
                    FROM Inquilino
                    WHERE {nameof(Inquilino.IdInquilino)}=@id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
                            Dni = reader.GetInt32(nameof(Inquilino.Dni)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Estado = reader.GetBoolean(nameof(Inquilino.Estado))
                        };
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySqlException: {ex.Number} - {ex.Message}");
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        return i;
    }
}