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
public class PagoController : Controller
{
    private readonly ILogger<PagoController> _logger;
    public INotyfService _notyf { get; }
    private RepositorioPago r;
    private readonly Msg msg;

    public PagoController(ILogger<PagoController> logger, INotyfService notyf)
    {
        _logger = logger;
        _notyf = notyf;
        r = new RepositorioPago();
        msg = new Msg();
    }

    //--------------------Métodos GET--------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Index()
    {
        List<Pago> lista = new List<Pago>();
        List<ViewModelPago> vm = new List<ViewModelPago>();
        try
        {
            lista = r.Listar();
            foreach (var item in lista)
            {
                Contrato c = new RepositorioContrato().BuscarPorId(item.IdContrato);
                Inquilino i = new RepositorioInquilino().BuscarPorId(c.IdInquilino);
                vm.Add(new ViewModelPago
                {
                    IdPago = item.IdPago,
                    IdContrato = item.IdContrato,
                    Contrato = c,
                    Inquilino = i,
                    Numero = item.Numero,
                    Fecha = item.Fecha,
                    Detalle = item.Detalle,
                    Precio = item.Precio,
                    Estado =item.Estado
                });
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
        return View(vm);
    }

    public IActionResult Nuevo(int id)
    {
        Pago? p = null;
        Contrato c = null;
        if (id > 0)
        {
            try
            {
                c = new RepositorioContrato().BuscarPorId(id);
                if (c == null) _notyf.Error(msg.NoEncontrado("contrato"));
                p = new Pago
                {
                    IdContrato = c.IdContrato,
                    Numero = r.ListarPorContrato(c.IdContrato).Count() + 1,
                    Fecha = DateTime.Today,
                    Precio = (decimal)c.Precio,
                    IdUsuarioAlta = int.Parse(User.FindFirst("id").Value)

                };
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
            if (p == null) return RedirectToAction(nameof(Index), "Contrato");
        }
        return View("FormAM", p);
    }

    public IActionResult FormAM(int id)
    {
        Pago? p = null;
        if (id > 0)
        {
            try
            {
                p = r.BuscarPorId(id);
                if (p == null) _notyf.Error(msg.NoEncontrado("Pago"));
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
        Pago p = null;
        ViewModelPago vm=null;
        try
        {
            p = r.BuscarPorId(id);
            if (p == null) _notyf.Error(msg.NoEncontrado("Pago"));

            Contrato c = new RepositorioContrato().BuscarPorId(p.IdContrato);
            Inmueble inmu = new RepositorioInmueble().BuscarPorId(c.IdInmueble);
            Inquilino i = new RepositorioInquilino().BuscarPorId(c.IdInquilino);
            vm = new ViewModelPago
            {
                IdPago = p.IdPago,
                IdContrato = p.IdContrato,
                Contrato = c,
                Inmueble = inmu,
                Inquilino = i,
                Numero = p.Numero,
                Fecha = p.Fecha,
                Precio = p.Precio,
                Detalle = p.Detalle,
                IdUsuarioAlta = p.IdUsuarioAlta,
                UsuarioAlta = new RepositorioUsuario().BuscarPorId(p.IdUsuarioAlta),
                IdUsuarioBaja = p.IdUsuarioBaja,
                UsuarioBaja = (p.IdUsuarioBaja >0) ? new RepositorioUsuario().BuscarPorId(p.IdUsuarioBaja) : null,
                Estado=p.Estado
            };
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
        if (vm == null) return RedirectToAction(nameof(Index));
        return View(vm);
    }

    //--------------------Métodos POST-------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Pago p)
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
            p.IdPago = res;
            _notyf.Success(msg.OkAccion("creado", "Pago"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("crear", "Pago"));
        }
        return RedirectToAction(nameof(Index), "Contrato");
    }

    [HttpPost]
    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        int res = -1;
        try
        {
            var IdUsuarioBaja = int.Parse(User.FindFirst("id").Value);
            res = r.Baja(id, IdUsuarioBaja);
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
            _notyf.Success(msg.OkAccion("eliminado", "Pago"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("eliminar", "Pago"));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Pago p)
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
            _notyf.Success(msg.OkAccion("actualizado", "Pago"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("actualizar", "Pago"));
        }
        return RedirectToAction(nameof(Index));
    }

    //------------------------Metodos return JSON--------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Id(int id)
    {
        Pago? p = null;
        ErrorJson error;
        if (id > 0)
        {
            try
            {
                p = r.BuscarPorId(id);
                if (p == null)
                {
                    error = new ErrorJson { MsgError = msg.NoEncontrado("Pago") };
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

    /* public IActionResult Filtro(string exp)
    {
        ErrorJson error;
        List<Pago> lista = new List<Pago>();
        try
        {
            lista = r.ListarPorInquilino(exp);
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
    } */
}
