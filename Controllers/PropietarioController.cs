using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaUrquiULP_v1.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaUrquiULP_v1.Controllers;

[Authorize]
public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    public INotyfService _notyf { get; }
    private RepositorioPropietario r;
    private readonly Msg msg;

    public PropietarioController(ILogger<PropietarioController> logger, INotyfService notyf)
    {
        _logger = logger;
        _notyf = notyf;
        r = new RepositorioPropietario();
        msg = new Msg();
    }

    //--------------------Métodos GET--------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Index()
    {
        List<Propietario> lista = new List<Propietario>();
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
        Propietario? p = null;
        if (id > 0)
        {
            try
            {
                p = r.BuscarPorId(id);
                if (p == null) _notyf.Error(msg.NoEncontrado("propietario"));
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
            if (p == null) return RedirectToAction(nameof(Index));
        }
        return View(p);
    }

    public IActionResult Detalles(int id)
    {
        Propietario? p = null;
        try
        {
            p = r.BuscarPorId(id);
            if (p == null) _notyf.Error(msg.NoEncontrado("propietario"));
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
        if (p == null) return RedirectToAction(nameof(Index));
        return View(p);
    }

    //--------------------Métodos POST-------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Propietario p)
    {
        int res = -1;
        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            return View("FormAM", p);
        }
        //Si los datos son válidos realiza la consulta a la DB.
        try
        {
            res = r.Alta(p);
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
            p.IdPropietario = res;
            _notyf.Success(msg.OkAccion("creado", "propietario"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("crear", "propietario"));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Policy ="Administrador")]
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
            _notyf.Success(msg.OkAccion("eliminado", "propietario"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("eliminar", "propietario"));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Propietario p)
    {
        int res = -1;
        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            return View("FormAM", p);
        }
        //Si los datos son válidos realiza la consulta a la DB.
        try
        {
            res = r.Modificacion(p);
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
            _notyf.Success(msg.OkAccion("actualizado", "propietario"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("actualizar", "propietario"));
        }
        return RedirectToAction(nameof(Index));
    }

    //------------------------Metodos return JSON--------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Id(int id)
    {
        Propietario? p = null;
        ErrorJson error;
        if (id > 0)
        {
            try
            {
                p = r.BuscarPorId(id);
                if (p == null)
                {
                    error = new ErrorJson { MsgError = msg.NoEncontrado("propietario") };
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
        return Json(p);
    }

    public IActionResult Filtro(string exp)
    {
        //string exp = "ez";
        ErrorJson error;
        List<Propietario> lista = new List<Propietario>();
        try
        {
            lista = r.ListarPorDniNombreApellido(exp);
            /* if (lista == null)
            {
                _notyf.Information(msg.NoCoincidencia);
            } */
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
        //return View("Index",lista);
        return Json(lista);
    }
}
