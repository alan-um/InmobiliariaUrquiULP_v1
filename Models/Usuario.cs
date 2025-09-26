using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaUrquiULP_v1.Models;

[DisplayName("Usuario")]
//[ValidaContacto]
public class Usuario
{
    //[Key]
    [DisplayName("Código")]
    public int IdUsuario { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el nombre.")]
    public string Nombre { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el apellido.")]
    public string Apellido { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el eMail.")]
    [EmailAddress(ErrorMessage = "*La dirección de email ingresada, no es correcta.")]
    public string Email { get; set; } = "";

    [DisplayName("Contraseña")]
    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar la contraseña.")]
    [DataType(DataType.Password)]
    public string Pass { get; set; } = "";

    [BindProperty]
    public string? Avatar { get; set; } = "";

    [NotMapped]
    public IFormFile? AvatarFile { get; set; }

    [DisplayName("Rol")]
    public bool isAdmin { get; set; }

    public bool Estado { get; set; }


    /* public override string ToString()
    {
        return Dni + " - " + Apellido + ", " + Nombre;
    } */

    public string NombreApellido()
    {
        return $"{Nombre} {Apellido}";
    }
}