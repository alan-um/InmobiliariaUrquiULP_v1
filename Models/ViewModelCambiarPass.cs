using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaUrquiULP_v1.Models;

//[ValidaContacto]
public class ViewModelCambiarPass
{
    public int IdUsuario { get; set; }

    [DisplayName("Ingrese su contraseña actual:")]
    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar la contraseña.")]
    [DataType(DataType.Password)]
    public string PassActual { get; set; } = "";

    [DisplayName("Ingrese su contraseña nueva:")]
    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar la contraseña.")]
    [DataType(DataType.Password)]
    public string PassNueva { get; set; } = "";

    [DisplayName("Repita la contraseña nueva:")]
    [BindProperty]
    [Required(ErrorMessage = "*Debe ingresar la contraseña.")]
    [DataType(DataType.Password)]
    public string PassRepetida { get; set; } = "";
}