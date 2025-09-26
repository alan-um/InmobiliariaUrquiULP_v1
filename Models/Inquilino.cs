using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaUrquiULP_v1.Models;

[DisplayName("Inquilino")]
//[ValidaContacto]
public class Inquilino
{
    [DisplayName("Código")]
    public int IdInquilino { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el DNI.")]
    [ValidaDni]
    public int? Dni { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el nombre.")]
    public string Nombre { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el apellido.")]
    public string Apellido { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el teléfono.")]
    [ValidaTelefono]
    public string Telefono { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el eMail.")]
    [EmailAddress(ErrorMessage = "*La dirección de email ingresada, no es correcta.")]
    public string Email { get; set; } = "";
    public bool Estado { get; set; }



    public override string ToString()
    {
        return $"{Dni} - {Apellido}, {Nombre}";
    }

    public string NombreApellido()
    {
        return $"{Nombre} {Apellido}";
    }
}