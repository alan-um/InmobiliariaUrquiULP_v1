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
public class AlquilarController : Controller
{
    private readonly ILogger<AlquilarController> _logger;
    public INotyfService _notyf { get; }
    private RepositorioInmueble r;
    private readonly Msg msg;

    public AlquilarController(ILogger<AlquilarController> logger, INotyfService notyf)
    {
        _logger = logger;
        _notyf = notyf;
        r = new RepositorioInmueble();
        msg = new Msg();
    }

    //--------------------Métodos GET--------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    /* public IActionResult Index()
    {
        List<Inmueble> lista = new List<Inmueble>();
        try
        {
            lista = r.ListarHabilitados();
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
        ViewBag.desde = null;
        ViewBag.hasta = null;
        return View(lista);
    } */

    public IActionResult Index(DateTime? desde, DateTime? hasta)
    {
        List<Inmueble> lista = new List<Inmueble>();
        try
        {
            if (desde != null && hasta != null)
            {
                lista = r.ListarPorFechaDisponible((DateTime)desde, (DateTime)hasta);
            }
            else
            {
                lista = r.ListarHabilitados();
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
        ViewBag.desde = desde;
        ViewBag.hasta = hasta;
        return View("Index", lista);
    }

}
