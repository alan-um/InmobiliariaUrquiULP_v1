using System.Data;
using InmobiliariaUrquiULP_v1.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace InmobiliariaUrquiULP_v1.Models;

public class RepositorioInmueble : RepositorioBase
{
    //------------------------------ABM------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public int Alta(Inmueble i)//Crea el Inmueble sin PORTADA
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    INSERT INTO Inmueble (
                        {nameof(Inmueble.Nombre)}, 
                        {nameof(Inmueble.IdPropietario)}, 
                        {nameof(Inmueble.Direccion)}, 
                        {nameof(Inmueble.Uso)}, 
                        {nameof(Inmueble.Tipo)}, 
                        {nameof(Inmueble.CantAmbientes)}, 
                        {nameof(Inmueble.Precio)}, 
                        {nameof(Inmueble.Habilitado)}, 
                        {nameof(Inmueble.Estado)}) 
                    VALUES (@Nombre, @IdPropietario, @Direccion, @Uso, @Tipo, @CantAmbientes, @Precio, @Habilitado, true);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Nombre", i.Nombre);
                    command.Parameters.AddWithValue("@IdPropietario", i.IdPropietario);
                    command.Parameters.AddWithValue("@Direccion", i.Direccion);
                    command.Parameters.AddWithValue("@Uso", i.Uso);
                    command.Parameters.AddWithValue("@Tipo", i.Tipo);
                    command.Parameters.AddWithValue("@CantAmbientes", i.CantAmbientes);
                    command.Parameters.AddWithValue("@Precio", i.Precio);
                    command.Parameters.AddWithValue("@Habilitado", i.Habilitado);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.IdInmueble = res;
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
                    UPDATE Inmueble 
                    SET {nameof(Inmueble.Estado)}=false
                    WHERE {nameof(Inmueble.IdInmueble)} = @id";
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

    public int Modificacion(Inmueble i)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                string sql = @$"
                    UPDATE Inmueble 
                    SET 
                        {nameof(Inmueble.Nombre)}=@Nombre, 
                        {nameof(Inmueble.IdPropietario)}=@IdPropietario, 
                        {nameof(Inmueble.Direccion)}=@Direccion, 
                        {nameof(Inmueble.Uso)}=@Uso, 
                        {nameof(Inmueble.Tipo)}=@Tipo, 
                        {nameof(Inmueble.CantAmbientes)}=@CantAmbientes, 
                        {nameof(Inmueble.Precio)}=@Precio, 
                        {nameof(Inmueble.Habilitado)}=@Habilitado
                    WHERE {nameof(Inmueble.IdInmueble)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Nombre", i.Nombre);
                    command.Parameters.AddWithValue("@IdPropietario", i.IdPropietario);
                    command.Parameters.AddWithValue("@Direccion", i.Direccion);
                    command.Parameters.AddWithValue("@Uso", i.Uso);
                    command.Parameters.AddWithValue("@Tipo", i.Tipo);
                    command.Parameters.AddWithValue("@CantAmbientes", i.CantAmbientes);
                    command.Parameters.AddWithValue("@Precio", i.Precio);
                    command.Parameters.AddWithValue("@Habilitado", i.Habilitado);
                    command.Parameters.AddWithValue("@id", i.IdInmueble);
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
    public List<Inmueble> ListarTodos()
    {
        List<Inmueble> Inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Inmueble.IdInmueble)}, 
                        {nameof(Inmueble.Nombre)}, 
                        {nameof(Inmueble.IdPropietario)}, 
                        {nameof(Inmueble.Direccion)}, 
                        {nameof(Inmueble.Uso)}, 
                        {nameof(Inmueble.Tipo)}, 
                        {nameof(Inmueble.CantAmbientes)}, 
                        {nameof(Inmueble.Precio)}, 
                        {nameof(Inmueble.Habilitado)}
                    FROM Inmueble
                    WHERE {nameof(Inmueble.Estado)} = true";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmuebles.Add(new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            Nombre = reader.GetString(nameof(Inmueble.Nombre)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                            CantAmbientes = reader.GetInt32(nameof(Inmueble.CantAmbientes)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Habilitado = reader.GetBoolean(nameof(Inmueble.Habilitado))
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
        return Inmuebles;
    }

    public List<Inmueble> ListarHabilitados()
    {
        List<Inmueble> Inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Inmueble.IdInmueble)}, 
                        {nameof(Inmueble.Nombre)}, 
                        {nameof(Inmueble.IdPropietario)}, 
                        {nameof(Inmueble.Direccion)}, 
                        {nameof(Inmueble.Uso)}, 
                        {nameof(Inmueble.Tipo)}, 
                        {nameof(Inmueble.CantAmbientes)}, 
                        {nameof(Inmueble.Precio)},
                        {nameof(Inmueble.Habilitado)}
                    FROM Inmueble
                    WHERE {nameof(Inmueble.Estado)} = true
                    AND {nameof(Inmueble.Habilitado)} = true";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmuebles.Add(new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            Nombre = reader.GetString(nameof(Inmueble.Nombre)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                            CantAmbientes = reader.GetInt32(nameof(Inmueble.CantAmbientes)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Habilitado = reader.GetBoolean(nameof(Inmueble.Habilitado))
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
        return Inmuebles;
    }

    public List<Inmueble> ListarPorFechaDisponible(DateTime desde, DateTime hasta)
    {
        List<Inmueble> iHabilitados = new List<Inmueble>();
        List<Inmueble> iDisponibles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                iHabilitados = ListarHabilitados();
                foreach (var item in iHabilitados)
                {
                    if (new RepositorioContrato().fechasDisponibles(0, item.IdInmueble, desde, hasta))
                    {
                        iDisponibles.Add(item);
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
        return iDisponibles;
    }

    /* public List<Inmueble> ListarPorEmailNombreApellido(string exp)
    {
        List<Inmueble> Inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Inmueble.IdInmueble)}, 
                        {nameof(Inmueble.Nombre)}, 
                        {nameof(Inmueble.Apellido)}, 
                        {nameof(Inmueble.Email)},
                        {nameof(Inmueble.Avatar)},
                        {nameof(Inmueble.isAdmin)}
                    FROM Inmueble
                    WHERE {nameof(Inmueble.Estado)} = true
                    AND ({nameof(Inmueble.Nombre)} LIKE '%{exp}%' 
                    OR {nameof(Inmueble.Apellido)} LIKE '%{exp}%' 
                    OR {nameof(Inmueble.Email)} LIKE '%{exp}%')";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmuebles.Add(new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            Nombre = reader.GetString(nameof(Inmueble.Nombre)),
                            Apellido = reader.GetString(nameof(Inmueble.Apellido)),
                            Email = reader.GetString(nameof(Inmueble.Email)),
                            //Avatar = reader.GetString(nameof(Inmueble.Avatar)),
                            Avatar = (reader.IsDBNull(nameof(Inmueble.Avatar))) ? null : reader.GetString(nameof(Inmueble.Avatar)),
                            isAdmin = reader.GetBoolean(nameof(Inmueble.isAdmin))
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
        return Inmuebles;
    } */


    //---------------------------BUSCAR------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public Inmueble? BuscarPorId(int id)
    {
        Inmueble? i = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Inmueble.IdInmueble)}, 
                        {nameof(Inmueble.Nombre)}, 
                        {nameof(Inmueble.IdPropietario)}, 
                        {nameof(Inmueble.Direccion)}, 
                        {nameof(Inmueble.Uso)}, 
                        {nameof(Inmueble.Tipo)}, 
                        {nameof(Inmueble.CantAmbientes)}, 
                        {nameof(Inmueble.Precio)}, 
                        {nameof(Inmueble.Habilitado)},
                        {nameof(Inmueble.Estado)}
                    FROM Inmueble
                    WHERE {nameof(Inmueble.IdInmueble)}=@id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            Nombre = reader.GetString(nameof(Inmueble.Nombre)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                            CantAmbientes = reader.GetInt32(nameof(Inmueble.CantAmbientes)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Habilitado = reader.GetBoolean(nameof(Inmueble.Habilitado)),
                            Estado = reader.GetBoolean(nameof(Inmueble.Estado))
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

    /* public Inmueble? BuscarPorEmail(string Email)
    {
        Inmueble? i = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Inmueble.IdInmueble)}, 
                        {nameof(Inmueble.Nombre)}, 
                        {nameof(Inmueble.Apellido)}, 
                        {nameof(Inmueble.Email)},
                        {nameof(Inmueble.Pass)},
                        {nameof(Inmueble.Avatar)},
                        {nameof(Inmueble.isAdmin)},
                        {nameof(Inmueble.Estado)}
                    FROM Inmueble
                    WHERE {nameof(Inmueble.Email)} LIKE @Email";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", Email);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            Nombre = reader.GetString(nameof(Inmueble.Nombre)),
                            Apellido = reader.GetString(nameof(Inmueble.Apellido)),
                            Email = reader.GetString(nameof(Inmueble.Email)),
                            Pass = reader.GetString(nameof(Inmueble.Pass)),
                            //Avatar = reader.GetString(nameof(Inmueble.Avatar)),
                            Avatar = (reader.IsDBNull(nameof(Inmueble.Avatar))) ? null : reader.GetString(nameof(Inmueble.Avatar)),
                            isAdmin = reader.GetBoolean(nameof(Inmueble.isAdmin)),
                            Estado = reader.GetBoolean(nameof(Inmueble.Estado))
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
                    Console.WriteLine("Connection closed.");
                }
            }
        }
        return i;
    } */

    //-----------------------------SERVICIOS------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    /* public string CargarAvatar(IFormFile avatarFile, int id, IWebHostEnvironment environment)
    {
        string avatar = "";
        if (avatarFile != null && id > 0)
        {
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //Crea el nombre del archivo combinando "avatar_" + ID del Inmueble + extensión el archivo
            string fileName = "avatar_" + id + Path.GetExtension(avatarFile.FileName);

            //Guarda el archivo en la carpeta /Uploads
            string filePath = Path.Combine(path, fileName);
            // Esta operación guarda la foto en el disco en la ruta que necesitamos
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                avatarFile.CopyTo(stream);
            }

            avatar = Path.Combine("/Uploads", fileName);
        }
        return avatar;
    } */

}