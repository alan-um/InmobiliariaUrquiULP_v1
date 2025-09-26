using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InmobiliariaUrquiULP_v1.Models;

[DisplayName("Pago")]
public class ViewModelPago
{
    [DisplayName("Código")]
    public int IdPago { get; set; }

    [DisplayName("Contrato")]
    public int IdContrato { get; set; }
    public Contrato Contrato { get; set; }
    public Inmueble Inmueble { get; set; }
    public Inquilino Inquilino { get; set; }


    [DisplayName("N° de Pago")]
    public int Numero { get; set; }

    [DisplayName("Fecha de pago")]
    public DateTime Fecha { get; set; }

    [DisplayName("Monto")]
    public decimal Precio { get; set; }

    [DisplayName("Detalle")]
    //[BindProperty]
    [Required(ErrorMessage = "*Debe ingresar el Detalle.")]
    public string Detalle { get; set; } = "";

    [DisplayName("Pago registrado por")] //Identifica qué USUARIO hizo el pago
    public int IdUsuarioAlta { get; set; }
    [BindNever]
    [DisplayName("Pago registrado por")] //Identifica qué USUARIO hizo el pago
    public Usuario? UsuarioAlta { get; set; }


    [DisplayName("Pago anulado por")] //Identifica qué USUARIO anuló el pago
    public int IdUsuarioBaja { get; set; }
    [BindNever]
    [DisplayName("Pago registrado por")] //Identifica qué USUARIO anuló el pago
    public Usuario? UsuarioBaja { get; set; }

    public bool Estado { get; set; }
}