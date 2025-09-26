using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaUrquiULP_v1.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace InmobiliariaUrquiULP_v1.Controllers;

[Authorize]
public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    public INotyfService _notyf { get; }
    private RepositorioUsuario r;
    private readonly Msg msg;
    private readonly IWebHostEnvironment environment;

    public UsuarioController(ILogger<UsuarioController> logger, INotyfService notyf, IWebHostEnvironment environment)
    {
        _logger = logger;
        _notyf = notyf;
        this.environment = environment;
        r = new RepositorioUsuario();
        msg = new Msg();
    }

    //--------------------Métodos GET--------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    [Authorize(Policy ="Administrador")]
    public IActionResult Index()
    {
        List<Usuario> lista = new List<Usuario>();
        try
        {
            lista = r.Listar();
        }
        catch (Exception ex)
        {
            if (ex is MySqlException)
            {
                _notyf.Error(msg.ErrorDB);
            }
            else
            {
                _notyf.Error(msg.ErrorGral);
            }
        }
        return View(lista);
    }

    public IActionResult FormAM(int id)
    {
        Usuario? u = null;
        if (id > 0)
        {
            try
            {
                u = r.BuscarPorId(id);
                if (u == null) _notyf.Error(msg.NoEncontrado("usuario"));
            }
            catch (Exception ex)
            {
                if (ex is MySqlException)
                {
                    _notyf.Error(msg.ErrorDB);
                }
                else
                {
                    _notyf.Error(msg.ErrorGral);
                }
            }
            if (u == null) return RedirectToAction(nameof(Index));
        }
        return View(u);
    }

    public IActionResult MiPerfil()
    {
        Usuario? u = null;
        int id = Convert.ToInt32(User.FindFirst("id").Value);
        if (id > 0)
        {
            try
            {
                u = r.BuscarPorId(id);
                if (u == null) _notyf.Error(msg.NoEncontrado("usuario"));
            }
            catch (Exception ex)
            {
                if (ex is MySqlException)
                {
                    _notyf.Error(msg.ErrorDB);
                }
                else
                {
                    _notyf.Error(msg.ErrorGral);
                }
            }
            if (u == null) return RedirectToAction(nameof(Index));
        }
        return View("FormAM", u);
    }

    public IActionResult FormPass(int id)
    {
        ViewModelCambiarPass vm = new ViewModelCambiarPass
        {
            IdUsuario = id
        };
        /* int id = Convert.ToInt32(User.FindFirst("id").Value);
        if (id > 0)
        {
            try
            {
                u = r.BuscarPorId(id);
                if (u == null) _notyf.Error(msg.NoEncontrado("usuario"));
            }
            catch (Exception ex)
            {
                if (ex is MySqlException)
                {
                    _notyf.Error(msg.ErrorDB);
                }
                else
                {
                    _notyf.Error(msg.ErrorGral);
                }
            }
            if (u == null) return RedirectToAction(nameof(Index));
        } */
        return View(vm);
    }

    public IActionResult FormMiPass()
    {
        int id = Convert.ToInt32(User.FindFirst("id").Value);
        ViewModelCambiarPass vm = new ViewModelCambiarPass
        {
            IdUsuario = id
        };
        /* int id = Convert.ToInt32(User.FindFirst("id").Value);
        if (id > 0)
        {
            try
            {
                u = r.BuscarPorId(id);
                if (u == null) _notyf.Error(msg.NoEncontrado("usuario"));
            }
            catch (Exception ex)
            {
                if (ex is MySqlException)
                {
                    _notyf.Error(msg.ErrorDB);
                }
                else
                {
                    _notyf.Error(msg.ErrorGral);
                }
            }
            if (u == null) return RedirectToAction(nameof(Index));
        } */
        return View("FormPass", vm);
    }

    /* public IActionResult Detalles(int id)
    {
        Usuario? u = null;
        try
        {
            u = r.BuscarPorId(id);
            if (u == null) _notyf.Error(msg.NoEncontrado("usuario"));
        }
        catch (Exception ex)
        {
            if (ex is MySqlException)
            {
                _notyf.Error(msg.ErrorDB);
            }
            else
            {
                _notyf.Error(msg.ErrorGral);
            }
        }
        if (u == null) return RedirectToAction(nameof(Index));
        return View(u);
    }
 */
    //--------------------Métodos POST-------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Usuario u)
    {
        int res = -1;
        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            return View("FormAM", u);
        }
        //Si los datos son válidos CREA el usuario sin AVATAR
        try
        {
            res = r.Alta(u);
        }
        catch (Exception ex)
        {
            if (ex is MySqlException)
            {
                _notyf.Error(msg.ErrorDB);
            }
            else
            {
                _notyf.Error(msg.ErrorGral);
            }
        }
        if (res > 0)
        {
            u.IdUsuario = res;
            _notyf.Success(msg.OkAccion("creado", "usuario"));

            //Si se crea el usuario correctamente se crea/actualiza el AVATAR
            try
            {
                if (u.AvatarFile != null)
                {
                    u.Avatar = r.CargarAvatar(u.AvatarFile, u.IdUsuario, environment);
                }
                else
                {
                    u.Avatar = "";
                }
                res = r.Modificacion(u);
            }
            catch (Exception ex)
            {
                if (ex is MySqlException)
                {
                    _notyf.Error(msg.ErrorDB);
                }
                else
                {
                    _notyf.Error(msg.ErrorGral);
                }
            }
            if (res != 1)
            {
                _notyf.Error(msg.NoAccion("cargar", "avatar"));
            }
        }
        else
        {
            _notyf.Error(msg.NoAccion("crear", "usuario"));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        int res = -1;
        try
        {
            res = r.Baja(id);
        }
        catch (Exception ex)
        {
            if (ex is MySqlException)
            {
                _notyf.Error(msg.ErrorDB);
            }
            else
            {
                _notyf.Error(msg.ErrorGral);
            }
        }
        if (res == 1)
        {
            _notyf.Success(msg.OkAccion("eliminado", "usuario"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("eliminar", "usuario"));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Usuario u)
    {
        int res = -1;
        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            return View("FormAM", u);
        }
        //Si los datos son válidos realiza la consulta a la DB.
        //Si hay archivo actualizar el Avatar
        try
        {
            if (u.AvatarFile != null)
            {
                u.Avatar = r.CargarAvatar(u.AvatarFile, u.IdUsuario, environment);
            }
            res = r.Modificacion(u);
        }
        catch (Exception ex)
        {
            if (ex is MySqlException)
            {
                _notyf.Error(msg.ErrorDB);
            }
            else
            {
                _notyf.Error(msg.ErrorGral);
            }
        }
        if (res == 1)
        {
            _notyf.Success(msg.OkAccion("actualizado", "usuario"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("actualizar", "usuario"));
        }
        if (User.IsInRole("Administrador"))
        {
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(MiPerfil));
    }

    [HttpPost]
    //[Authorize(Policy = "Administrador")]
    public IActionResult CambiarPass(ViewModelCambiarPass vm)
    {
        int res = -1;
        Usuario? u = null;

        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            return View("FormPass", vm);
        }
        if (!vm.PassNueva.Equals(vm.PassRepetida))
        {
            _notyf.Error("La contraseña repetida es distinta.");
            return View("FormPass", vm);
        }


        try
        {
            u = r.BuscarPorId(vm.IdUsuario);
            if (u == null) _notyf.Error(msg.NoEncontrado("usuario"));
            if (!r.Hashear(vm.PassActual).Equals(u.Pass))
            {
                _notyf.Error("La contraseña actual es incorrecta.");
                return View("FormPass", vm);
            }
            res = r.Modificacion(vm.IdUsuario, r.Hashear(vm.PassNueva));
        }
        catch (Exception ex)
        {
            if (ex is MySqlException)
            {
                _notyf.Error(msg.ErrorDB);
            }
            else
            {
                _notyf.Error(msg.ErrorGral);
            }
        }
        if (u == null) return RedirectToAction(nameof(Index));
        if (res == 1)
        {
            _notyf.Success("Se ha modificado la contraseña.");
        }
        else
        {
            _notyf.Error("No se ha podido modificar la contraseña.");
        }
        return RedirectToAction(nameof(Index));
    }


    //------------------------Metodos return JSON--------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Id(int id)
    {
        Usuario? u = null;
        ErrorJson error;
        if (id > 0)
        {
            try
            {
                u = r.BuscarPorId(id);
                if (u == null)
                {
                    error = new ErrorJson { MsgError = msg.NoEncontrado("usuario") };
                    return Json(error);
                }
            }
            catch (Exception ex)
            {
                if (ex is MySqlException)
                {
                    error = new ErrorJson { MsgError = msg.ErrorDB };
                }
                else
                {
                    error = new ErrorJson { MsgError = msg.ErrorGral };
                }
                return Json(error);
            }
        }
        return Json(u);
    }

    public IActionResult Filtro(string exp)
    {
        //string exp = "ez";
        ErrorJson error;
        List<Usuario> lista = new List<Usuario>();
        try
        {
            lista = r.ListarPorEmailNombreApellido(exp);
        }
        catch (Exception ex)
        {
            if (ex is MySqlException)
            {
                error = new ErrorJson { MsgError = msg.ErrorDB };
            }
            else
            {
                error = new ErrorJson { MsgError = msg.ErrorGral };
            }
            return Json(error);
        }
        return Json(lista);
    }


    //-----------------------LOGEO-----------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(ViewModelLogin vm)
    {
        int res = -1;
        Usuario? u = null;

        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            return View("../Home/Login", vm);
            //return RedirectToAction("Login", "Home");
        }

        try
        {
            u = r.BuscarPorEmail(vm.Email);
            //Corrobora Usuario y Pass
            if (u == null || !r.Hashear(vm.Pass).Equals(u.Pass))
            {
                _notyf.Error("Usuario y/o contraseña incorrectos.");
                return RedirectToAction("Login", "Home");
            }
        }
        catch (Exception ex)
        {
            if (ex is MySqlException)
            {
                _notyf.Error(msg.ErrorDB);
            }
            else
            {
                _notyf.Error(msg.ErrorGral);
            }
        }

        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, u.Nombre+" "+u.Apellido),
                        new Claim(ClaimTypes.Role, (u.isAdmin) ? "Administrador" : "Empleado"),
                        new Claim("id", u.IdUsuario+""),
                        new Claim("avatar", (u.Avatar==null)?"":u.Avatar)
                    };

        var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        return RedirectToAction("Index", "Alquilar");
    }

    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Home");
    }
}
