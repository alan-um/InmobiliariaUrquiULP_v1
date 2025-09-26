using System.Data;
using InmobiliariaUrquiULP_v1.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace InmobiliariaUrquiULP_v1.Models;

public class RepositorioUsuario : RepositorioBase
{
    //------------------------------ABM------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public int Alta(Usuario u)//Crea el usuario sin AVATAR
    {
        int res = -1;
        u.Pass = Hashear(u.Pass);
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    INSERT INTO Usuario (
                        {nameof(Usuario.Nombre)}, 
                        {nameof(Usuario.Apellido)}, 
                        {nameof(Usuario.Email)}, 
                        {nameof(Usuario.Pass)}, 
                        {nameof(Usuario.isAdmin)}, 
                        {nameof(Usuario.Estado)}) 
                    VALUES (@Nombre, @Apellido, @Email, @Pass, @isAdmin, true);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Nombre", u.Nombre);
                    command.Parameters.AddWithValue("@Apellido", u.Apellido);
                    command.Parameters.AddWithValue("@Email", u.Email);
                    command.Parameters.AddWithValue("@Pass", u.Pass);
                    command.Parameters.AddWithValue("@isAdmin", u.isAdmin);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    u.IdUsuario = res;
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
                    UPDATE Usuario 
                    SET {nameof(Usuario.Estado)}=false
                    WHERE {nameof(Usuario.IdUsuario)} = @id";
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

    public int Modificacion(Usuario u) //NO MODIFICA PASS!
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                string sql = @$"
                    UPDATE Usuario 
                    SET 
                        {nameof(Usuario.Nombre)}=@Nombre, 
                        {nameof(Usuario.Apellido)}=@Apellido, 
                        {nameof(Usuario.Email)}=@Email,
                        {nameof(Usuario.Avatar)}=@Avatar,
                        {nameof(Usuario.isAdmin)}=@isAdmin
                    WHERE {nameof(Usuario.IdUsuario)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Nombre", u.Nombre);
                    command.Parameters.AddWithValue("@Apellido", u.Apellido);
                    command.Parameters.AddWithValue("@Email", u.Email);
                    command.Parameters.AddWithValue("@Avatar", u.Avatar);
                    command.Parameters.AddWithValue("@isAdmin", u.isAdmin);
                    command.Parameters.AddWithValue("@id", u.IdUsuario);
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

    public int Modificacion(int id, string pass) //SOLO MODIFICA PASS!
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                string sql = @$"
                    UPDATE Usuario 
                    SET {nameof(Usuario.Pass)}=@Pass 
                    WHERE {nameof(Usuario.IdUsuario)} = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Pass", pass);
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

    //---------------------------LISTAS------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public List<Usuario> Listar()
    {
        List<Usuario> usuarios = new List<Usuario>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Usuario.IdUsuario)}, 
                        {nameof(Usuario.Nombre)}, 
                        {nameof(Usuario.Apellido)}, 
                        {nameof(Usuario.Email)},
                        {nameof(Usuario.Avatar)},
                        {nameof(Usuario.isAdmin)}
                    FROM Usuario
                    WHERE {nameof(Usuario.Estado)} = true";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            //Avatar = reader.GetString(nameof(Usuario.Avatar)),
                            Avatar = (reader.IsDBNull(nameof(Usuario.Avatar))) ? null : reader.GetString(nameof(Usuario.Avatar)),
                            isAdmin = reader.GetBoolean(nameof(Usuario.isAdmin))
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
        return usuarios;
    }

    public List<Usuario> ListarPorEmailNombreApellido(string exp)
    {
        List<Usuario> usuarios = new List<Usuario>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Usuario.IdUsuario)}, 
                        {nameof(Usuario.Nombre)}, 
                        {nameof(Usuario.Apellido)}, 
                        {nameof(Usuario.Email)},
                        {nameof(Usuario.Avatar)},
                        {nameof(Usuario.isAdmin)}
                    FROM Usuario
                    WHERE {nameof(Usuario.Estado)} = true
                    AND ({nameof(Usuario.Nombre)} LIKE '%{exp}%' 
                    OR {nameof(Usuario.Apellido)} LIKE '%{exp}%' 
                    OR {nameof(Usuario.Email)} LIKE '%{exp}%')";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            //Avatar = reader.GetString(nameof(Usuario.Avatar)),
                            Avatar = (reader.IsDBNull(nameof(Usuario.Avatar))) ? null : reader.GetString(nameof(Usuario.Avatar)),
                            isAdmin = reader.GetBoolean(nameof(Usuario.isAdmin))
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
        return usuarios;
    }


    //---------------------------BUSCAR------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public Usuario? BuscarPorId(int id)
    {
        Usuario? u = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Usuario.IdUsuario)}, 
                        {nameof(Usuario.Nombre)}, 
                        {nameof(Usuario.Apellido)}, 
                        {nameof(Usuario.Email)},
                        {nameof(Usuario.Pass)},
                        {nameof(Usuario.Avatar)},
                        {nameof(Usuario.isAdmin)},
                        {nameof(Usuario.Estado)}
                    FROM Usuario
                    WHERE {nameof(Usuario.IdUsuario)}=@id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        u = new Usuario
                        {
                            IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Pass = reader.GetString(nameof(Usuario.Pass)),
                            //Avatar = reader.GetString(nameof(Usuario.Avatar)),
                            Avatar = (reader.IsDBNull(nameof(Usuario.Avatar))) ? null : reader.GetString(nameof(Usuario.Avatar)),
                            isAdmin = reader.GetBoolean(nameof(Usuario.isAdmin)),
                            Estado = reader.GetBoolean(nameof(Usuario.Estado))
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
        return u;
    }

    public Usuario? BuscarPorEmail(string Email)
    {
        Usuario? u = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                var sql = $@"
                    SELECT 
                        {nameof(Usuario.IdUsuario)}, 
                        {nameof(Usuario.Nombre)}, 
                        {nameof(Usuario.Apellido)}, 
                        {nameof(Usuario.Email)},
                        {nameof(Usuario.Pass)},
                        {nameof(Usuario.Avatar)},
                        {nameof(Usuario.isAdmin)},
                        {nameof(Usuario.Estado)}
                    FROM Usuario
                    WHERE {nameof(Usuario.Email)} LIKE @Email";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", Email);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        u = new Usuario
                        {
                            IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Pass = reader.GetString(nameof(Usuario.Pass)),
                            //Avatar = reader.GetString(nameof(Usuario.Avatar)),
                            Avatar = (reader.IsDBNull(nameof(Usuario.Avatar))) ? null : reader.GetString(nameof(Usuario.Avatar)),
                            isAdmin = reader.GetBoolean(nameof(Usuario.isAdmin)),
                            Estado = reader.GetBoolean(nameof(Usuario.Estado))
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
        return u;
    }

    //-----------------------------SERVICIOS------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public string CargarAvatar(IFormFile avatarFile, int id, IWebHostEnvironment environment)
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
            //Crea el nombre del archivo combinando "avatar_" + ID del Usuario + extensión el archivo
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
    }

    public string Hashear(string pass)
    {
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: pass,
                        salt: System.Text.Encoding.ASCII.GetBytes("Salt"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
        return hashed;
    }
}