using System.Data;
using InmobiliariaUrquiULP_v1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace InmobiliariaUrquiULP_v1.Models;

public class RepositorioPropietario : RepositorioBase
{
    //------------------------------ABM------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public int Alta(Propietario p)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    INSERT INTO Propietario (
                        {nameof(Propietario.Dni)}, 
                        {nameof(Propietario.Nombre)}, 
                        {nameof(Propietario.Apellido)}, 
                        {nameof(Propietario.Telefono)}, 
                        {nameof(Propietario.Email)}, 
                        {nameof(Propietario.Estado)}) 
                    VALUES (@Dni, @Nombre, @Apellido, @Telefono, @Email, true);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Dni", p.Dni);
                    command.Parameters.AddWithValue("@Nombre", p.Nombre);
                    command.Parameters.AddWithValue("@Apellido", p.Apellido);
                    command.Parameters.AddWithValue("@Telefono", p.Telefono);
                    command.Parameters.AddWithValue("@Email", p.Email);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.IdPropietario = res;
                    //connection.Close();
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
                    Console.WriteLine("Connection closed.");
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
                    UPDATE Propietario 
                    SET {nameof(Propietario.Estado)}=false
                    WHERE {nameof(Propietario.IdPropietario)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    //connection.Close();
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
                    Console.WriteLine("Connection closed.");
                }
            }
        }
        return res;
    }

    public int Modificacion(Propietario p)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                string sql = @$"
                    UPDATE Propietario 
                    SET 
                        {nameof(Propietario.Dni)}=@Dni, 
                        {nameof(Propietario.Nombre)}=@Nombre, 
                        {nameof(Propietario.Apellido)}=@Apellido, 
                        {nameof(Propietario.Telefono)}=@Telefono, 
                        {nameof(Propietario.Email)}=@Email
                    WHERE {nameof(Propietario.IdPropietario)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Dni", p.Dni);
                    command.Parameters.AddWithValue("@Nombre", p.Nombre);
                    command.Parameters.AddWithValue("@Apellido", p.Apellido);
                    command.Parameters.AddWithValue("@Telefono", p.Telefono);
                    command.Parameters.AddWithValue("@Email", p.Email);
                    command.Parameters.AddWithValue("@id", p.IdPropietario);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    //connection.Close();
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
                    Console.WriteLine("Connection closed.");
                }
            }
        }
        return res;
    }

    //---------------------------LISTAS------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public List<Propietario> Listar()
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Propietario.IdPropietario)}, 
                        {nameof(Propietario.Dni)}, 
                        {nameof(Propietario.Nombre)}, 
                        {nameof(Propietario.Apellido)}, 
                        {nameof(Propietario.Telefono)}, 
                        {nameof(Propietario.Email)}
                    FROM Propietario
                    WHERE {nameof(Propietario.Estado)} = true";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        propietarios.Add(new Propietario
                        {
                            IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
                            Dni = reader.GetInt32(nameof(Propietario.Dni)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Email = reader.GetString(nameof(Propietario.Email))
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
                    Console.WriteLine("Connection closed.");
                }
            }
        }
        return propietarios;
    }

    public List<Propietario> ListarPorDniNombreApellido(string exp)
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Propietario.IdPropietario)}, 
                        {nameof(Propietario.Dni)}, 
                        {nameof(Propietario.Nombre)}, 
                        {nameof(Propietario.Apellido)}, 
                        {nameof(Propietario.Telefono)}, 
                        {nameof(Propietario.Email)},
                        {nameof(Propietario.Estado)}
                    FROM Propietario
                    WHERE {nameof(Propietario.Estado)} = true
                    AND ({nameof(Propietario.Nombre)} LIKE '%{exp}%' 
                    OR {nameof(Propietario.Apellido)} LIKE '%{exp}%' 
                    OR CAST({nameof(Propietario.Dni)} AS CHAR) LIKE '%{exp}%')";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        propietarios.Add(new Propietario
                        {
                            IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
                            Dni = reader.GetInt32(nameof(Propietario.Dni)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Estado = reader.GetBoolean(nameof(Propietario.Estado))
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
                    Console.WriteLine("Connection closed.");
                }
            }
        }
        return propietarios;
    }


    //---------------------------BUSCAR------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public Propietario? BuscarPorId(int? id)
    {
        Propietario? p = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Propietario.IdPropietario)}, 
                        {nameof(Propietario.Dni)}, 
                        {nameof(Propietario.Nombre)}, 
                        {nameof(Propietario.Apellido)}, 
                        {nameof(Propietario.Telefono)}, 
                        {nameof(Propietario.Email)},
                        {nameof(Propietario.Estado)}
                    FROM Propietario
                    WHERE {nameof(Propietario.IdPropietario)}=@id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
                            Dni = reader.GetInt32(nameof(Propietario.Dni)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Estado = reader.GetBoolean(nameof(Propietario.Estado))
                        };
                    }
                    //connection.Close();
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
                    Console.WriteLine("Connection closed.");
                }
            }
        }
        return p;
    }
}