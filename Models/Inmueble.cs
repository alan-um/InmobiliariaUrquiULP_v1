using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaUrquiULP_v1.Models;

[DisplayName("Inmueble")]
public class Inmueble
{
    [DisplayName("N°")]
    public int IdInmueble { get; set; }

    [DisplayName("Nombre de fantasía")]
    //[BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el nombre.")]
    public string Nombre { get; set; } = "";

    [DisplayName("Propietario")]
    //[BindProperty]
    [Required(ErrorMessage = "*Debe indicar el propietario.")]
    public int? IdPropietario { get; set; }

    //[BindProperty]
    //[Required(ErrorMessage = "*Debe indicar el propietario.")]
    public Propietario? Propietario { get; set; }

    //[BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el dirección.")]
    public string Direccion { get; set; } = "";

    //[BindProperty]
    [Required(ErrorMessage = "*Debe seleccionar el uso del inmueble.")]
    public string Uso { get; set; } = "";

    //[BindProperty]
    [Required(ErrorMessage = "*Debe seleccionar el tipo del inmueble.")]
    public string Tipo { get; set; } = "";

    [DisplayName("Cant. de Ambientes")]
    //[BindProperty]
    [Required(ErrorMessage = "*Debe ingresar la cantidad de ambientes.")]
    public int? CantAmbientes { get; set; }

    [DisplayName("Precio de alquiler mensual")]
    //[BindProperty]
    [Required(ErrorMessage = "*Debe indicar el precio del alquiler.")]
    public decimal? Precio { get; set; }

    [DisplayName("Disponible")]
    //[BindProperty]
    [Required(ErrorMessage = "*Debe la disponibilidad del inmueble.")]
    public bool Habilitado { get; set; }

    public bool Estado { get; set; }



    /* public override string ToString()
    {
        return Dni + " - " + Apellido + ", " + Nombre;
    } */
}