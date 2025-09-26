using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaUrquiULP_v1.Models;

[DisplayName("TipoInmueble")]
//[ValidaContacto]
public class TipoInmueble
{
    [DisplayName("Código")]
    public int IdTipoInmueble { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el descripcion.")]
    public string Descripcion { get; set; } = "";
    public bool Estado { get; set; }



    public override string ToString()
    {
        return IdTipoInmueble + " - " + Descripcion;
    }
}