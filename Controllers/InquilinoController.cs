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
public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;
    public INotyfService _notyf { get; }
    private RepositorioInquilino r;
    private readonly Msg msg;

    public InquilinoController(ILogger<InquilinoController> logger, INotyfService notyf)
    {
        _logger = logger;
        _notyf = notyf;
        r = new RepositorioInquilino();
        msg = new Msg();
    }

    //--------------------Métodos GET--------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Index()
    {
        List<Inquilino> lista = new List<Inquilino>();
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
        Inquilino? i = null;
        if (id > 0)
        {
            try
            {
                i = r.BuscarPorId(id);
                if (i == null) _notyf.Error(msg.NoEncontrado("inquilino"));
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
            if (i == null) return RedirectToAction(nameof(Index));
        }
        return View(i);
    }

    public IActionResult Detalles(int id)
    {
        Inquilino? i = null;
        try
        {
            i = r.BuscarPorId(id);
            if (i == null) _notyf.Error(msg.NoEncontrado("inquilino"));
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
        if (i == null) return RedirectToAction(nameof(Index));
        return View(i);
    }

    //--------------------Métodos POST-------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Inquilino i)
    {
        int res = -1;
        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            return View("FormAM", i);
        }
        //Si los datos son válidos realiza la consulta a la DB.
        try
        {
            res = r.Alta(i);
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
            i.IdInquilino = res;
            _notyf.Success(msg.OkAccion("creado", "inquilino"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("crear", "inquilino"));
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
            _notyf.Success(msg.OkAccion("eliminado", "inquilino"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("eliminar", "inquilino"));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Inquilino i)
    {
        int res = -1;
        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            return View("FormAM", i);
        }
        //Si los datos son válidos realiza la consulta a la DB.
        try
        {
            res = r.Modificacion(i);
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
            _notyf.Success(msg.OkAccion("actualizado", "inquilino"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("actualizar", "inquilino"));
        }
        return RedirectToAction(nameof(Index));
    }

    //------------------------Metodos return JSON--------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Id(int id)
    {
        Inquilino? i = null;
        ErrorJson error;
        if (id > 0)
        {
            try
            {
                i = r.BuscarPorId(id);
                if (i == null)
                {
                    error = new ErrorJson { MsgError = msg.NoEncontrado("inquilino") };
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
        return Json(i);
    }

    public IActionResult Filtro(string exp)
    {
        ErrorJson error;
        List<Inquilino> lista = new List<Inquilino>();
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
    }
}
