using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaUrquiULP_v1.Models;

//[ValidaContacto]
public class ViewModelLogin
{
    [DisplayName("Ingrese su eMail:")]
    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el eMail.")]
    [EmailAddress(ErrorMessage = "*La dirección de email ingresada, no es correcta.")]
    public string Email { get; set; } = "";

    [DisplayName("Ingrese su contraseña:")]
    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar la contraseña.")]
    [DataType(DataType.Password)]
    public string Pass { get; set; } = "";
}