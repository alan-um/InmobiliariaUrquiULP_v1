using System.Data;
using InmobiliariaUrquiULP_v1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace InmobiliariaUrquiULP_v1.Models;

public class RepositorioPago : RepositorioBase
{
    //------------------------------ABM------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public int Alta(Pago p)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    INSERT INTO Pago (
                        {nameof(Pago.IdContrato)}, 
                        {nameof(Pago.Numero)}, 
                        {nameof(Pago.Fecha)},
                        {nameof(Pago.Precio)}, 
                        {nameof(Pago.Detalle)}, 
                        {nameof(Pago.IdUsuarioAlta)}, 
                        {nameof(Pago.Estado)}) 
                    VALUES (@IdContrato, @Numero, @Fecha, @Precio, @Detalle, @IdUsuarioAlta, true);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@IdContrato", p.IdContrato);
                    command.Parameters.AddWithValue("@Numero", p.Numero);
                    command.Parameters.AddWithValue("@Fecha", p.Fecha);
                    command.Parameters.AddWithValue("@Precio", p.Precio);
                    command.Parameters.AddWithValue("@Detalle", p.Detalle);
                    command.Parameters.AddWithValue("@IdUsuarioAlta", p.IdUsuarioAlta);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.IdPago = res;
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

    public int Baja(int id, int IdUsuarioBaja)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                string sql = @$"
                    UPDATE Pago 
                    SET 
                        {nameof(Pago.Estado)}=false,
                        {nameof(Pago.IdUsuarioBaja)}=@usuarioBaja
                    WHERE {nameof(Pago.IdPago)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@usuarioBaja", IdUsuarioBaja);
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

    public int Modificacion(Pago p)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                string sql = @$"
                    UPDATE Pago 
                    SET 
                        {nameof(Pago.Detalle)}=@Detalle
                    WHERE {nameof(Pago.IdPago)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Detalle", p.Detalle);
                    command.Parameters.AddWithValue("@id", p.IdPago);
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
    public List<Pago> Listar()
    {
        List<Pago> Pagos = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Pago.IdPago)}, 
                        {nameof(Pago.IdContrato)}, 
                        {nameof(Pago.Numero)}, 
                        {nameof(Pago.Fecha)}, 
                        {nameof(Pago.Precio)}, 
                        {nameof(Pago.Detalle)}, 
                        {nameof(Pago.IdUsuarioAlta)},
                        {nameof(Pago.IdUsuarioBaja)},
                        {nameof(Pago.Estado)}
                    FROM Pago";
                //WHERE {nameof(Pago.Estado)} = true";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pagos.Add(new Pago
                        {
                            IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            Numero = reader.GetInt32(nameof(Pago.Numero)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Precio = reader.GetDecimal(nameof(Pago.Precio)),
                            Detalle = reader.GetString(nameof(Pago.Detalle)),
                            IdUsuarioAlta = reader.GetInt32(nameof(Pago.IdUsuarioAlta)),
                            UsuarioAlta = new RepositorioUsuario().BuscarPorId(reader.GetInt32(nameof(Pago.IdUsuarioAlta))),
                            IdUsuarioBaja = (reader.IsDBNull(nameof(Pago.IdUsuarioBaja))) ? 0 : reader.GetInt32(nameof(Pago.IdUsuarioBaja)),
                            UsuarioBaja = (reader.IsDBNull(nameof(Pago.IdUsuarioBaja))) ? null :
                                new RepositorioUsuario().BuscarPorId(reader.GetInt32(nameof(Pago.IdUsuarioBaja))),
                            Estado = reader.GetBoolean(nameof(Pago.Estado))
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
        return Pagos;
    }

    public List<Pago> ListarPorContrato(int IdContrato)
    {
        List<Pago> Pagos = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Pago.IdPago)}, 
                        {nameof(Pago.IdContrato)}, 
                        {nameof(Pago.Numero)}, 
                        {nameof(Pago.Fecha)}, 
                        {nameof(Pago.Precio)},
                        {nameof(Pago.Detalle)}, 
                        {nameof(Pago.IdUsuarioAlta)},
                        {nameof(Pago.IdUsuarioBaja)},
                        {nameof(Pago.Estado)}
                    FROM Pago
                    WHERE {nameof(Pago.IdContrato)} = @idContrato";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idContrato", IdContrato);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pagos.Add(new Pago
                        {
                            IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            Numero = reader.GetInt32(nameof(Pago.Numero)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Precio = reader.GetDecimal(nameof(Pago.Precio)),
                            Detalle = reader.GetString(nameof(Pago.Detalle)),
                            IdUsuarioAlta = reader.GetInt32(nameof(Pago.IdUsuarioAlta)),
                            UsuarioAlta = new RepositorioUsuario().BuscarPorId(reader.GetInt32(nameof(Pago.IdUsuarioAlta))),
                            IdUsuarioBaja = (reader.IsDBNull(nameof(Pago.IdUsuarioBaja))) ? 0 : reader.GetInt32(nameof(Pago.IdUsuarioBaja)),
                            UsuarioBaja = (reader.IsDBNull(nameof(Pago.IdUsuarioBaja))) ? null :
                                new RepositorioUsuario().BuscarPorId(reader.GetInt32(nameof(Pago.IdUsuarioBaja))),
                            Estado = reader.GetBoolean(nameof(Pago.Estado))
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
        return Pagos;
    }


    //---------------------------BUSCAR------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public Pago? BuscarPorId(int id)
    {
        Pago? p = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Pago.IdPago)}, 
                        {nameof(Pago.IdContrato)}, 
                        {nameof(Pago.Numero)}, 
                        {nameof(Pago.Fecha)}, 
                        {nameof(Pago.Precio)},
                        {nameof(Pago.Detalle)}, 
                        {nameof(Pago.IdUsuarioAlta)},
                        {nameof(Pago.IdUsuarioBaja)},
                        {nameof(Pago.Estado)}
                    FROM Pago
                    WHERE {nameof(Pago.IdPago)}=@id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Pago
                        {
                            IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            Numero = reader.GetInt32(nameof(Pago.Numero)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Precio = reader.GetDecimal(nameof(Pago.Precio)),
                            Detalle = reader.GetString(nameof(Pago.Detalle)),
                            IdUsuarioAlta = reader.GetInt32(nameof(Pago.IdUsuarioAlta)),
                            UsuarioAlta = new RepositorioUsuario().BuscarPorId(reader.GetInt32(nameof(Pago.IdUsuarioAlta))),
                            IdUsuarioBaja = (reader.IsDBNull(nameof(Pago.IdUsuarioBaja))) ? 0 : reader.GetInt32(nameof(Pago.IdUsuarioBaja)),
                            UsuarioBaja = (reader.IsDBNull(nameof(Pago.IdUsuarioBaja))) ? null :
                                new RepositorioUsuario().BuscarPorId(reader.GetInt32(nameof(Pago.IdUsuarioBaja))),
                            Estado = reader.GetBoolean(nameof(Pago.Estado))
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
        return p;
    }

}