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
public class TipoInmuebleController : Controller
{
    private readonly ILogger<TipoInmuebleController> _logger;
    public INotyfService _notyf { get; }
    private RepositorioTipoInmueble r;
    private readonly Msg msg;

    public TipoInmuebleController(ILogger<TipoInmuebleController> logger, INotyfService notyf)
    {
        _logger = logger;
        _notyf = notyf;
        r = new RepositorioTipoInmueble();
        msg = new Msg();
    }

    //--------------------Métodos GET--------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Index()
    {
        List<TipoInmueble> lista = new List<TipoInmueble>();
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
        TipoInmueble? tipo = null;
        if (id > 0)
        {
            try
            {
                tipo = r.BuscarPorId(id);
                if (tipo == null) _notyf.Error(msg.NoEncontrado("TipoInmueble"));
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
            if (tipo == null) return RedirectToAction(nameof(Index));
        }
        return View(tipo);
    }

    /* public IActionResult Detalles(int id)
    {
        TipoInmueble? tipo = null;
        try
        {
            tipo = r.BuscarPorId(id);
            if (tipo == null) _notyf.Error(msg.NoEncontrado("TipoInmueble"));
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
        if (tipo == null) return RedirectToAction(nameof(Index));
        return View(tipo);
    } */

    //--------------------Métodos POST-------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(TipoInmueble tipo)
    {
        int res = -1;
        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            return View("FormAM", tipo);
        }
        //Si los datos son válidos realiza la consulta a la DB.
        try
        {
            res = r.Alta(tipo);
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
            tipo.IdTipoInmueble = res;
            _notyf.Success(msg.OkAccion("creado", "tipo de inmueble"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("crear", "tipo de inmueble"));
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
            _notyf.Success(msg.OkAccion("eliminado", "tipo de inmueble"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("eliminar", "tipo de inmueble"));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(TipoInmueble tipo)
    {
        int res = -1;
        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            return View("FormAM", tipo);
        }
        //Si los datos son válidos realiza la consulta a la DB.
        try
        {
            res = r.Modificacion(tipo);
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
            _notyf.Success(msg.OkAccion("actualizado", "tipo de inmueble"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("actualizar", "tipo de inmueble"));
        }
        return RedirectToAction(nameof(Index));
    }

    //------------------------Metodos return JSON--------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Id(int id)
    {
        TipoInmueble? tipo = null;
        ErrorJson error;
        if (id > 0)
        {
            try
            {
                tipo = r.BuscarPorId(id);
                if (tipo == null)
                {
                    error = new ErrorJson { MsgError = msg.NoEncontrado("tipo de inmueble") };
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
        return Json(tipo);
    }

    /* public IActionResult Filtro(string exp)
    {
        ErrorJson error;
        List<TipoInmueble> lista = new List<TipoInmueble>();
        try
        {
            lista = r.ListarPorDniNombreApellido(exp);
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
    } */
}
