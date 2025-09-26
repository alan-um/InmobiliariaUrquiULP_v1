using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InmobiliariaUrquiULP_v1.Models;

public class ViewModelAlquilar
{
    public List<Inmueble> Inmuebles;

    [DisplayName("Fecha de inicio")]
    [Required(ErrorMessage = "*Debe indicar la fecha de inicio.")]
    public DateTime? fechaInicio { get; set; }

    [DisplayName("Fecha de finalización")]
    [Required(ErrorMessage = "*Debe indicar la fecha de finalización.")]
    public DateTime? fechaFin { get; set; }
}