using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaUrquiULP_v1.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace InmobiliariaUrquiULP_v1.Controllers;

[Authorize]
public class ContratoController : Controller
{
    private readonly ILogger<ContratoController> _logger;
    public INotyfService _notyf { get; }
    private RepositorioContrato r;
    private readonly Msg msg;

    public ContratoController(ILogger<ContratoController> logger, INotyfService notyf)
    {
        _logger = logger;
        _notyf = notyf;
        r = new RepositorioContrato();
        msg = new Msg();
    }

    //--------------------Métodos GET--------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Index()
    {
        List<Contrato> lista = new List<Contrato>();
        RepositorioInquilino repoInquilino = new RepositorioInquilino();
        DateTime hoy = DateTime.Now;
        try
        {
            lista = r.ListarTodos();
            foreach (var item in lista)//Antes de mostrarlo en la vista carga la condición del contrato!!
            {
                if (item.fechaBaja == null)
                {
                    if (item.fechaInicio > hoy)
                    {
                        item.Condicion = "Reservado";
                    }
                    else
                    {
                        if (item.fechaFin < hoy)
                        {
                            item.Condicion = "Finalizado";
                        }
                        else
                        {
                            item.Condicion = "Vigente";
                        }
                    }
                }
                else
                {
                    item.Condicion = "Baja anticipada";
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
        }
        return View(lista);
    }

    public IActionResult Nuevo(int id, DateTime? desde, DateTime? hasta)
    {
        DateTime hoy = DateTime.Now;
        List<Inquilino> inquilinos = new List<Inquilino>();
        Contrato c;
        try
        {
            Inmueble i = new RepositorioInmueble().BuscarPorId(id);
            c = new Contrato
            {
                IdContrato = 0,
                IdInmueble = id,
                Inmueble = i,
                IdInquilino = 0,
                fechaInicio = (DateTime)((desde == null) ? hoy : desde),
                fechaFin = (DateTime)((hasta == null) ? hoy : hasta),
                Precio = i.Precio
            };

            inquilinos = new RepositorioInquilino().Listar();
            if (inquilinos == null) _notyf.Error("No se han encontrado los inquilinos disponibles");
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
        ViewBag.ListaInquilino = inquilinos;
        return View("FormAM", c);
    }


    public IActionResult FormAM(int id)
    {
        Contrato? c = null;
        List<Inquilino> inquilinos = new List<Inquilino>();
        try
        {
            inquilinos = new RepositorioInquilino().Listar();
            if (inquilinos == null) _notyf.Error("No se han encontrado los inquilinos disponibles");

            if (id > 0)
            {
                c = r.BuscarPorId(id);
                if (c == null)
                {
                    _notyf.Error(msg.NoEncontrado("contrato"));
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
        ViewBag.ListaInquilino = inquilinos;
        return View(c);
    }

    public IActionResult Detalles(int id)
    {
        Contrato? c = null;
        DateTime hoy = DateTime.Now;
        try
        {
            c = r.BuscarPorId(id);
            if (c == null)
            {
                _notyf.Error(msg.NoEncontrado("Contrato"));
                return RedirectToAction(nameof(Index));
            }
            c.Inmueble = new RepositorioInmueble().BuscarPorId(c.IdInmueble);
            c.Inmueble.Propietario = new RepositorioPropietario().BuscarPorId(c.Inmueble.IdPropietario);
            c.Inquilino = new RepositorioInquilino().BuscarPorId(c.IdInquilino);

            if (c.fechaBaja == null)
            {
                if (c.fechaInicio > hoy)
                {
                    c.Condicion = "Reservado";
                }
                else
                {
                    if (c.fechaFin < hoy)
                    {
                        c.Condicion = "Finalizado";
                    }
                    else
                    {
                        c.Condicion = "Vigente";
                    }
                }
            }
            else
            {
                c.Condicion = "Baja anticipada";
            }
            ViewBag.UsuarioAlta = new RepositorioUsuario().BuscarPorId((int)c.IdUsuarioAlta).NombreApellido();
            if (c.fechaBaja != null)
            {
                ViewBag.fechaBaja = c.fechaBaja;
                ViewBag.UsuarioBaja = new RepositorioUsuario().BuscarPorId((int)c.IdUsuarioBaja).NombreApellido();
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
        if (c == null) return RedirectToAction(nameof(Index));
        //ViewBag.Propietario = p;
        return View(c);
    }

    public IActionResult BajaAnticipada(int id)
    {
        Contrato? c = null;
        DateTime hoy = DateTime.Now;
        try
        {
            c = r.BuscarPorId(id);
            if (c == null)
            {
                _notyf.Error(msg.NoEncontrado("Contrato"));
                return RedirectToAction(nameof(Index));
            }
            c.Inmueble = new RepositorioInmueble().BuscarPorId(c.IdInmueble);
            c.Inmueble.Propietario = new RepositorioPropietario().BuscarPorId(c.Inmueble.IdPropietario);
            c.Inquilino = new RepositorioInquilino().BuscarPorId(c.IdInquilino);

            if (c.fechaBaja == null)
            {
                if (c.fechaInicio > hoy)
                {
                    c.Condicion = "Reservado";
                }
                else
                {
                    if (c.fechaFin < hoy)
                    {
                        c.Condicion = "Finalizado";
                    }
                    else
                    {
                        c.Condicion = "Vigente";
                    }
                }
            }
            else
            {
                _notyf.Error("El contrato seleccionado ya ha sido dado de baja.");
                return RedirectToAction(nameof(Index));
            }

            //Calcular la multa que corresponde
            DateTime dtInicio = c.fechaInicio;
            DateTime dtFin = c.fechaFin;
            TimeSpan tiempoContrato = dtFin - dtInicio;
            TimeSpan tiempoTranscurrido = hoy - dtInicio;
            if (tiempoTranscurrido < tiempoContrato / 2)
            {
                ViewBag.multa = c.Precio * 2;
            }
            else
            {
                ViewBag.multa = c.Precio;
            }



            ViewBag.UsuarioAlta = new RepositorioUsuario().BuscarPorId((int)c.IdUsuarioAlta).NombreApellido();
            if (c.fechaBaja != null)
            {
                ViewBag.fechaBaja = c.fechaBaja;
                ViewBag.UsuarioBaja = new RepositorioUsuario().BuscarPorId((int)c.IdUsuarioBaja).NombreApellido();
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
        if (c == null) return RedirectToAction(nameof(Index));
        //ViewBag.Propietario = p;
        return View(c);
    }

    //--------------------Métodos POST-------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Contrato c)
    {
        int res = -1;
        try
        {
            //------COMPROBANDO LAS FECHAS!!---------------------
            //Comprobar que la fechaInicio sea hoy o mayor.
            /* if (c.fechaInicio < DateTime.Now)
            {
                ModelState.AddModelError("", "La fecha de inicio no puede ser anterior al día de hoy.");
            } */
            //Comprobar que la fechaFin sea mayor que la fechaInicio.
            if (c.fechaInicio >= c.fechaFin)
            {
                ModelState.AddModelError("", "La fecha de fin debe ser posterior a la fecha de inicio.");
            }
            //Comprabar que el período de alquiler esté disponible.
            if (!r.fechasDisponibles(c.IdContrato, c.IdInmueble, c.fechaInicio, c.fechaFin))
            {
                ModelState.AddModelError("", "La fechas fechas seleccionadas no están disponibles.");
            }

            //Valida los datos ingresados en el formulario, según el modelo.
            if (!ModelState.IsValid)
            {
                List<Inquilino> inquilinos = new List<Inquilino>();
                inquilinos = new RepositorioInquilino().Listar();
                if (inquilinos == null) _notyf.Error("No se han encontrado los propietarios disponibles");
                c.Inmueble = new RepositorioInmueble().BuscarPorId(c.IdInmueble);
                ViewBag.ListaInquilino = inquilinos;
                return View("FormAM", c);
            }
            //Si los datos son válidos realiza la consulta a la DB.
            res = r.Alta(c);
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
            c.IdContrato = res;
            _notyf.Success(msg.OkAccion("creado", "Contrato"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("crear", "Contrato"));
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
            _notyf.Success(msg.OkAccion("eliminado", "Contrato"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("eliminar", "Contrato"));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Contrato c)
    {
        int res = -1;
        try
        {
            //------COMPROBANDO LAS FECHAS!!---------------------
            //Comprobar que la fechaInicio sea hoy o mayor.
            /* if (c.fechaInicio < DateTime.Now)
            {
                ModelState.AddModelError("", "La fecha de inicio no puede ser anterior al día de hoy.");
            } */
            //Comprobar que la fechaFin sea mayor que la fechaInicio.
            if (c.fechaInicio >= c.fechaFin)
            {
                ModelState.AddModelError("", "La fecha de fin debe ser posterior a la fecha de inicio.");
            }
            //Comprabar que el período de alquiler esté disponible.
            if (!r.fechasDisponibles(c.IdContrato, c.IdInmueble, c.fechaInicio, c.fechaFin))
            {
                ModelState.AddModelError("", "La fechas fechas seleccionadas no están disponibles.");
            }

            //Valida los datos ingresados en el formulario, según el modelo.
            if (!ModelState.IsValid)
            {
                List<Inquilino> inquilinos = new List<Inquilino>();
                inquilinos = new RepositorioInquilino().Listar();
                if (inquilinos == null) _notyf.Error("No se han encontrado los propietarios disponibles");
                c.Inmueble = new RepositorioInmueble().BuscarPorId(c.IdInmueble);
                ViewBag.ListaInquilino = inquilinos;
                return View("FormAM", c);
            }
            //Si los datos son válidos realiza la consulta a la DB.
            res = r.Modificacion(c);
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
            _notyf.Success(msg.OkAccion("actualizado", "Contrato"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("actualizar", "Contrato"));
        }
        return RedirectToAction(nameof(Index));
    }



    public IActionResult DarDeBaja(int idc, int idu, decimal m)
    {
        Contrato c = null;
        int res = -1, resP = -1;
        try
        {
            c = r.BuscarPorId(idc);
            c.fechaBaja = DateTime.Now;
            c.IdUsuarioBaja = idu;
            res = r.Modificacion(c);
            Pago p = new Pago
            {
                IdContrato = idc,
                Numero = new RepositorioPago().ListarPorContrato(c.IdContrato).Count() + 1,
                Fecha = DateTime.Today,
                Precio = m/100m,//Lo divido por 100 porque toma los decimales como enteros.
                Detalle = "Multa por baja anticipada",
                IdUsuarioAlta = int.Parse(User.FindFirst("id").Value)
            };
            resP = new RepositorioPago().Alta(p);
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
        if (res == 1 && resP > 0)
        {
            _notyf.Success(msg.OkAccion("dado de baja", "Contrato"));
        }
        else
        {
            _notyf.Error(msg.NoAccion("dar de baja", "Contrato"));
        }
        return RedirectToAction(nameof(Index));
    }

    //------------------------Metodos return JSON--------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public IActionResult Id(int id)
    {
        Contrato? c = null;
        ErrorJson error;
        if (id > 0)
        {
            try
            {
                c = r.BuscarPorId(id);
                if (c == null)
                {
                    error = new ErrorJson { MsgError = msg.NoEncontrado("Contrato") };
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
        return Json(c);
    }

    /* public IActionResult Filtro(string exp)
    {
        ErrorJson error;
        List<Contrato> lista = new List<Contrato>();
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
