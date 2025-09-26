using System.Data;
using InmobiliariaUrquiULP_v1.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace InmobiliariaUrquiULP_v1.Models;

public class RepositorioContrato : RepositorioBase
{
    //------------------------------ABM------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public int Alta(Contrato c)//Crea el Contrato sin PORTADA
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    INSERT INTO Contrato (
                        {nameof(Contrato.IdInmueble)}, 
                        {nameof(Contrato.IdInquilino)}, 
                        {nameof(Contrato.fechaInicio)}, 
                        {nameof(Contrato.fechaFin)}, 
                        {nameof(Contrato.Precio)}, 
                        {nameof(Contrato.IdUsuarioAlta)}, 
                        {nameof(Contrato.Estado)}) 
                    VALUES (@IdInmueble, @IdInquilino, @fechaInicio, @fechaFin, @Precio, @IdUsuarioAlta, true);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@IdInmueble", c.IdInmueble);
                    command.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
                    command.Parameters.AddWithValue("@fechaInicio", c.fechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", c.fechaFin);
                    command.Parameters.AddWithValue("@Precio", c.Precio);
                    command.Parameters.AddWithValue("@IdUsuarioAlta", c.IdUsuarioAlta);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.IdContrato = res;
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
                    UPDATE Contrato 
                    SET {nameof(Contrato.Estado)}=false
                    WHERE {nameof(Contrato.IdContrato)} = @id";
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

    public int Modificacion(Contrato c)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                string sql = @$"
                    UPDATE Contrato 
                    SET 
                        {nameof(Contrato.IdInmueble)}=@IdInmueble, 
                        {nameof(Contrato.IdInquilino)}=@IdInquilino, 
                        {nameof(Contrato.fechaInicio)}=@fechaInicio, 
                        {nameof(Contrato.fechaFin)}=@fechaFin, 
                        {nameof(Contrato.Precio)}=@Precio, 
                        {nameof(Contrato.IdUsuarioAlta)}=@IdUsuarioAlta,
                        {nameof(Contrato.fechaBaja)}=@fechaBaja, 
                        {nameof(Contrato.IdUsuarioBaja)}=@IdUsuarioBaja
                    WHERE {nameof(Contrato.IdContrato)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@IdInmueble", c.IdInmueble);
                    command.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
                    command.Parameters.AddWithValue("@fechaInicio", c.fechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", c.fechaFin);
                    command.Parameters.AddWithValue("@Precio", c.Precio);
                    command.Parameters.AddWithValue("@IdUsuarioAlta", c.IdUsuarioAlta);
                    command.Parameters.AddWithValue("@fechaBaja", c.fechaBaja);
                    command.Parameters.AddWithValue("@IdUsuarioBaja", c.IdUsuarioBaja);
                    command.Parameters.AddWithValue("@id", c.IdContrato);
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
    public List<Contrato> ListarTodos()
    {
        List<Contrato> Contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Contrato.IdContrato)}, 
                        {nameof(Contrato.IdInmueble)}, 
                        {nameof(Contrato.IdInquilino)}, 
                        {nameof(Contrato.fechaInicio)}, 
                        {nameof(Contrato.fechaFin)}, 
                        {nameof(Contrato.Precio)}, 
                        {nameof(Contrato.IdUsuarioAlta)}, 
                        {nameof(Contrato.fechaBaja)}, 
                        {nameof(Contrato.IdUsuarioBaja)}
                    FROM Contrato
                    WHERE {nameof(Contrato.Estado)} = true";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        /* DateTime? fechaBaja = null;
                        int? IdUsuarioBaja = null;
                        if (!reader.IsDBNull(nameof(Contrato.fechaBaja))) {
                            fechaBaja = reader.GetDateTime(nameof(Contrato.fechaBaja));
                            IdUsuarioBaja = reader.GetInt32(nameof(Contrato.IdUsuarioBaja));
                        } */
                        Contratos.Add(new Contrato
                        {
                            IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Inmueble = new RepositorioInmueble().BuscarPorId(reader.GetInt32(nameof(Contrato.IdInmueble))),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            Inquilino = new RepositorioInquilino().BuscarPorId(reader.GetInt32(nameof(Contrato.IdInquilino))),
                            fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio)),
                            fechaFin = reader.GetDateTime(nameof(Contrato.fechaFin)),
                            Precio = reader.GetDecimal(nameof(Contrato.Precio)),
                            IdUsuarioAlta = reader.GetInt32(nameof(Contrato.IdUsuarioAlta)),
                            //fechaBaja = fechaBaja,
                            fechaBaja = reader.IsDBNull(nameof(Contrato.fechaBaja)) ? null : reader.GetDateTime(nameof(Contrato.fechaBaja)),
                            IdUsuarioBaja = reader.IsDBNull(nameof(Contrato.IdUsuarioBaja)) ? null : reader.GetInt32(nameof(Contrato.IdUsuarioBaja))
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
        return Contratos;
    }

    public List<Contrato> ListarPorIdInmueble(int id)
    {
        List<Contrato> Contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Contrato.IdContrato)}, 
                        {nameof(Contrato.IdInmueble)}, 
                        {nameof(Contrato.IdInquilino)}, 
                        {nameof(Contrato.fechaInicio)}, 
                        {nameof(Contrato.fechaFin)}, 
                        {nameof(Contrato.Precio)}, 
                        {nameof(Contrato.IdUsuarioAlta)}, 
                        {nameof(Contrato.fechaBaja)}, 
                        {nameof(Contrato.IdUsuarioBaja)}
                    FROM Contrato
                    WHERE {nameof(Contrato.Estado)} = true
                    AND {nameof(Contrato.IdInmueble)}=@id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contratos.Add(new Contrato
                        {
                            IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Inmueble = new RepositorioInmueble().BuscarPorId(reader.GetInt32(nameof(Contrato.IdInmueble))),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            Inquilino = new RepositorioInquilino().BuscarPorId(reader.GetInt32(nameof(Contrato.IdInquilino))),
                            fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio)),
                            fechaFin = reader.GetDateTime(nameof(Contrato.fechaFin)),
                            Precio = reader.GetDecimal(nameof(Contrato.Precio)),
                            IdUsuarioAlta = reader.GetInt32(nameof(Contrato.IdUsuarioAlta)),
                            fechaBaja = reader.IsDBNull(nameof(Contrato.fechaBaja)) ? null : reader.GetDateTime(nameof(Contrato.fechaBaja)),
                            IdUsuarioBaja = reader.IsDBNull(nameof(Contrato.IdUsuarioBaja)) ? null : reader.GetInt32(nameof(Contrato.IdUsuarioBaja))
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
        return Contratos;
    }

    public List<Contrato> ListarVigentes()
    {
        List<Contrato> Contratos = new List<Contrato>();
        DateTime hoy = DateTime.Today;
        string stHoy = $"{hoy.Year}-{hoy.Month}-{hoy.Day}";
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Contrato.IdContrato)}, 
                        {nameof(Contrato.IdInmueble)}, 
                        {nameof(Contrato.IdInquilino)}, 
                        {nameof(Contrato.fechaInicio)}, 
                        {nameof(Contrato.fechaFin)}, 
                        {nameof(Contrato.Precio)}, 
                        {nameof(Contrato.IdUsuarioAlta)}, 
                        {nameof(Contrato.fechaBaja)}, 
                        {nameof(Contrato.IdUsuarioBaja)}
                    FROM Contrato
                    WHERE {nameof(Contrato.Estado)} = true
                    AND {nameof(Contrato.fechaInicio)} < '{stHoy}'
                    AND {nameof(Contrato.fechaFin)} > '{stHoy}'";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    //command.Parameters.AddWithValue("@hoy", DateOnly.FromDateTime(hoy));
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contratos.Add(new Contrato
                        {
                            IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Inmueble = new RepositorioInmueble().BuscarPorId(reader.GetInt32(nameof(Contrato.IdInmueble))),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            Inquilino = new RepositorioInquilino().BuscarPorId(reader.GetInt32(nameof(Contrato.IdInquilino))),
                            fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio)),
                            fechaFin = reader.GetDateTime(nameof(Contrato.fechaFin)),
                            Precio = reader.GetDecimal(nameof(Contrato.Precio)),
                            IdUsuarioAlta = reader.GetInt32(nameof(Contrato.IdUsuarioAlta)),
                            fechaBaja = reader.IsDBNull(nameof(Contrato.fechaBaja)) ? null : reader.GetDateTime(nameof(Contrato.fechaBaja)),
                            IdUsuarioBaja = reader.IsDBNull(nameof(Contrato.IdUsuarioBaja)) ? null : reader.GetInt32(nameof(Contrato.IdUsuarioBaja))
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
        return Contratos;
    }


    //---------------------------BUSCAR------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public Contrato? BuscarPorId(int id)
    {
        Contrato? c = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Contrato.IdContrato)}, 
                        {nameof(Contrato.IdInmueble)}, 
                        {nameof(Contrato.IdInquilino)}, 
                        {nameof(Contrato.fechaInicio)}, 
                        {nameof(Contrato.fechaFin)}, 
                        {nameof(Contrato.Precio)}, 
                        {nameof(Contrato.IdUsuarioAlta)}, 
                        {nameof(Contrato.fechaBaja)}, 
                        {nameof(Contrato.IdUsuarioBaja)},
                        {nameof(Contrato.Estado)}
                    FROM Contrato
                    WHERE {nameof(Contrato.IdContrato)}=@id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Inmueble = new RepositorioInmueble().BuscarPorId(reader.GetInt32(nameof(Contrato.IdInmueble))),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            Inquilino = new RepositorioInquilino().BuscarPorId(reader.GetInt32(nameof(Contrato.IdInquilino))),
                            fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio)),
                            fechaFin = reader.GetDateTime(nameof(Contrato.fechaFin)),
                            Precio = reader.GetDecimal(nameof(Contrato.Precio)),
                            IdUsuarioAlta = reader.GetInt32(nameof(Contrato.IdUsuarioAlta)),
                            fechaBaja = reader.IsDBNull(nameof(Contrato.fechaBaja)) ? null : reader.GetDateTime(nameof(Contrato.fechaBaja)),
                            IdUsuarioBaja = reader.IsDBNull(nameof(Contrato.IdUsuarioBaja)) ? null : reader.GetInt32(nameof(Contrato.IdUsuarioBaja)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado))
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
        return c;
    }


    //-----------------------------SERVICIOS------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public bool fechasDisponibles(int idc, int idi, DateTime fechaInicio, DateTime fechaFin)
    {
        List<Contrato> lista = new List<Contrato>();
        try
        {
            lista = ListarPorIdInmueble(idi);
            foreach (var item in lista)//Antes de mostrarlo en la vista carga la condición del contrato!!
            {
                if (idc != item.IdContrato)
                {
                    //if (item.fechaBaja != null) item.fechaFin = (DateTime)item.fechaBaja;
                    if (fechaInicio >= item.fechaInicio && fechaInicio < item.fechaFin) return false;
                    if (fechaFin > item.fechaInicio && fechaFin <= item.fechaFin) return false;
                }
            }
        }
        catch
        {
            throw;
        }
        return true;
    }
}