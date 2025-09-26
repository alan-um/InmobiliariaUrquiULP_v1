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
public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;
    public INotyfService _notyf { get; }
    private RepositorioInmueble r;
    private readonly Msg msg;

    public InmuebleController(ILogger<InmuebleController> logger, INotyfService notyf)
    {
        _logger = logger;
        _notyf = notyf;
        r = new RepositorioInmueble();
        msg = new Msg();
    }

    //--------------------Métodos GET--------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Index()
    {
        List<Inmueble> lista = new List<Inmueble>();
        try
        {
            lista = r.ListarTodos();
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
        Inmueble? i = null;
        List<TipoInmueble> lista = new List<TipoInmueble>();
        List<Propietario> propietarios = new List<Propietario>();
        try
        {
            propietarios = new RepositorioPropietario().Listar();
            if (propietarios == null) _notyf.Error("No se han encontrado los propietarios disponibles");

            lista = new RepositorioTipoInmueble().Listar();
            if (lista == null) _notyf.Error("No se han encontrado tipos de inmueble disponibles");

            if (id > 0)
            {
                i = r.BuscarPorId(id);
                if (i == null)
                {
                    _notyf.Error(msg.NoEncontrado("inmueble"));
                    return RedirectToAction(nameof(Index));
                }
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
            return RedirectToAction(nameof(Index));
        }
        ViewBag.ListaPropietario = propietarios;
        ViewBag.ListaTipoInmueble = lista;
        return View(i);
    }

    public IActionResult Detalles(int id)
    {
        Inmueble? i = null;
        Propietario? p = null;
        try
        {
            i = r.BuscarPorId(id);
            if (i == null)
            {
                _notyf.Error(msg.NoEncontrado("Inmueble"));
                return RedirectToAction(nameof(Index));
            }

            p = new RepositorioPropietario().BuscarPorId(i.IdPropietario);
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
        if (i == null || p == null) return RedirectToAction(nameof(Index));
        ViewBag.Propietario = p;
        return View(i);
    }

    //--------------------Métodos POST-------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Inmueble i)
    {
        int res = -1;
        try
        {
            //Valida los datos ingresados en el formulario, según el modelo.
            if (!ModelState.IsValid)
            {
                List<Propietario> propietarios = new List<Propietario>();
                propietarios = new RepositorioPropietario().Listar();
                if (propietarios == null) _notyf.Error("No se han encontrado los propietarios disponibles");

                List<TipoInmueble> lista = new List<TipoInmueble>();
                lista = new RepositorioTipoInmueble().Listar();
                if (lista == null) _notyf.Error("No se han encontrado tipos de inmueble disponibles");

                ViewBag.ListaPropietario = propietarios;
                ViewBag.ListaTipoInmueble = lista;
                return View("FormAM", i);
            }
            //Si los datos son válidos realiza la consulta a la DB.
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
            i.IdInmueble = res;
            _notyf.Success(msg.OkAccion("creado", "inmueble"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("crear", "inmueble"));
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
            _notyf.Success(msg.OkAccion("eliminado", "inmueble"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("eliminar", "inmueble"));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Inmueble i)
    {
        int res = -1;
        //Valida los datos ingresados en el formulario, según el modelo.
        if (!ModelState.IsValid)
        {
            List<Propietario> propietarios = new List<Propietario>();
            propietarios = new RepositorioPropietario().Listar();
            if (propietarios == null) _notyf.Error("No se han encontrado los propietarios disponibles");

            List<TipoInmueble> lista = new List<TipoInmueble>();
            lista = new RepositorioTipoInmueble().Listar();
            if (lista == null) _notyf.Error("No se han encontrado tipos de inmueble disponibles");

            ViewBag.ListaPropietario = propietarios;
            ViewBag.ListaTipoInmueble = lista;
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
            _notyf.Success(msg.OkAccion("actualizado", "inmueble"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("actualizar", "inmueble"));
        }
        return RedirectToAction(nameof(Index));
    }

    //------------------------Metodos return JSON--------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Id(int id)
    {
        Inmueble? i = null;
        ErrorJson error;
        if (id > 0)
        {
            try
            {
                i = r.BuscarPorId(id);
                if (i == null)
                {
                    error = new ErrorJson { MsgError = msg.NoEncontrado("inmueble") };
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

    /* public IActionResult Filtro(string exp)
    {
        ErrorJson error;
        List<Inmueble> lista = new List<Inmueble>();
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
