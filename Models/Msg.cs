using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaUrquiULP_v1.Models;

public class Msg
{
    public readonly string ErrorDB = "Error al conectarse con la Base de Datos. Intente nuevamente más tarde.";
    public readonly string ErrorGral = "No se puede realizar la tarea indicada. Si el problema persiste comuniquese con el desarrollador.";
    public readonly string NoCoincidencia = "Se encontraron coincidencias para su busqueda.";
    public string NoEncontrado(string tipo)
    {
        return $"No se ha encontrado el {tipo} solicitado.";
    }
    public string OkAccion(string accion,string tipo)
    {
        return $"Se han {accion} correctamente los datos del {tipo}.";
    }public string NoAccion(string accion,string tipo)
    {
        return $"No se ha podido {accion} el {tipo}.";
    }
}