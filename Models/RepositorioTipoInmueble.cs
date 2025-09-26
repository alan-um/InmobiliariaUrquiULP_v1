using System.Data;
using InmobiliariaUrquiULP_v1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace InmobiliariaUrquiULP_v1.Models;

public class RepositorioTipoInmueble : RepositorioBase
{
    //------------------------------ABM------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public int Alta(TipoInmueble tipo)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    INSERT INTO TipoInmueble (
                        {nameof(TipoInmueble.Descripcion)}, 
                        {nameof(TipoInmueble.Estado)}) 
                    VALUES (@Descripcion, true);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Descripcion", tipo.Descripcion);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    tipo.IdTipoInmueble = res;
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
                    UPDATE TipoInmueble 
                    SET {nameof(TipoInmueble.Estado)}=false
                    WHERE {nameof(TipoInmueble.IdTipoInmueble)} = @id";
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

    public int Modificacion(TipoInmueble tipo)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                string sql = @$"
                    UPDATE TipoInmueble 
                    SET {nameof(TipoInmueble.Descripcion)}=@Descripcion
                    WHERE {nameof(TipoInmueble.IdTipoInmueble)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Descripcion", tipo.Descripcion);
                    command.Parameters.AddWithValue("@id", tipo.IdTipoInmueble);
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
    public List<TipoInmueble> Listar()
    {
        List<TipoInmueble> propietarios = new List<TipoInmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(TipoInmueble.IdTipoInmueble)}, 
                        {nameof(TipoInmueble.Descripcion)}
                    FROM TipoInmueble
                    WHERE {nameof(TipoInmueble.Estado)} = true";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        propietarios.Add(new TipoInmueble
                        {
                            IdTipoInmueble = reader.GetInt32(nameof(TipoInmueble.IdTipoInmueble)),
                            Descripcion = reader.GetString(nameof(TipoInmueble.Descripcion))
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
        return propietarios;
    }

    /* public List<TipoInmueble> ListarPorDniNombreApellido(string exp)
    {
        List<TipoInmueble> propietarios = new List<TipoInmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(TipoInmueble.IdTipoInmueble)}, 
                        {nameof(TipoInmueble.Dni)}, 
                        {nameof(TipoInmueble.Nombre)}, 
                        {nameof(TipoInmueble.Apellido)}, 
                        {nameof(TipoInmueble.Telefono)}, 
                        {nameof(TipoInmueble.Email)},
                        {nameof(TipoInmueble.Estado)}
                    FROM TipoInmueble
                    WHERE {nameof(TipoInmueble.Estado)} = true
                    AND ({nameof(TipoInmueble.Nombre)} LIKE '%{exp}%' 
                    OR {nameof(TipoInmueble.Apellido)} LIKE '%{exp}%' 
                    OR CAST({nameof(TipoInmueble.Dni)} AS CHAR) LIKE '%{exp}%')";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        propietarios.Add(new TipoInmueble
                        {
                            IdTipoInmueble = reader.GetInt32(nameof(TipoInmueble.IdTipoInmueble)),
                            Dni = reader.GetInt32(nameof(TipoInmueble.Dni)),
                            Nombre = reader.GetString(nameof(TipoInmueble.Nombre)),
                            Apellido = reader.GetString(nameof(TipoInmueble.Apellido)),
                            Telefono = reader.GetString(nameof(TipoInmueble.Telefono)),
                            Email = reader.GetString(nameof(TipoInmueble.Email)),
                            Estado = reader.GetBoolean(nameof(TipoInmueble.Estado))
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
    } */


    //---------------------------BUSCAR------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public TipoInmueble? BuscarPorId(int id)
    {
        TipoInmueble? tipo = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(TipoInmueble.IdTipoInmueble)}, 
                        {nameof(TipoInmueble.Descripcion)},
                        {nameof(TipoInmueble.Estado)}
                    FROM TipoInmueble
                    WHERE {nameof(TipoInmueble.IdTipoInmueble)}=@id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        tipo = new TipoInmueble
                        {
                            IdTipoInmueble = reader.GetInt32(nameof(TipoInmueble.IdTipoInmueble)),
                            Descripcion = reader.GetString(nameof(TipoInmueble.Descripcion)),
                            Estado = reader.GetBoolean(nameof(TipoInmueble.Estado))
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
        return tipo;
    }
}