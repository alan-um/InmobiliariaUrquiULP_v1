using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InmobiliariaUrquiULP_v1.Models;

[DisplayName("Contrato")]
public class Contrato
{
    [DisplayName("N°")]
    public int IdContrato { get; set; }



    [DisplayName("Inmueble")]
    [Required(ErrorMessage = "*Debe indicar el inmueble.")]
    public int IdInmueble { get; set; }
    [BindNever]
    public Inmueble? Inmueble { get; set; }



    [DisplayName("Inquilino")]
    [Required(ErrorMessage = "*Debe indicar el inquilino.")]
    public int IdInquilino { get; set; }
    [BindNever]
    public Inquilino? Inquilino { get; set; }


    [DisplayName("Fecha de inicio")]
    [Required(ErrorMessage = "*Debe indicar la fecha de inicio.")]
    public DateTime fechaInicio { get; set; }

    [DisplayName("Fecha de finalización")]
    [Required(ErrorMessage = "*Debe indicar la fecha de finalización.")]
    public DateTime fechaFin { get; set; }

    [DisplayName("Precio de alquiler mensual")]
    [Required(ErrorMessage = "*Debe indicar el precio del alquiler.")]
    public decimal? Precio { get; set; }

    [DisplayName("Contrato registrado por")] //Identifica qué USUARIO hizo el contrato
    public int? IdUsuarioAlta { get; set; }
    
//En caso de BAJA ANTICIPADA del contrato
    [DisplayName("Fecha de baja")]
    public DateTime? fechaBaja { get; set; }

    [DisplayName("Baja registrada por")] //Identifica qué USUARIO dió de baja el contrato
    public int? IdUsuarioBaja { get; set; }

    [DisplayName("Estado")]
    [BindNever]
    public string Condicion { get; set; } = "";

    public bool Estado { get; set; }



    /* public override string ToString()
    {
        return Dni + " - " + Apellido + ", " + Nombre;
    } */
}