namespace InmobiliariaUrquiULP_v1.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class ValidaDni : ValidationAttribute
{
    //private readonly Regex _regex = new Regex(@"^\d{7,8}$");
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Permite que otros atributos como [Required] manejen los valores nulos.
        }
        int dni = (int)value;
        if (dni >= (1000000) && dni < (100000000))
        {
            return ValidationResult.Success; // La validación fue exitosa.
        }
        else
        {
            // La validación falló. El mensaje de error puede ser definido aquí o en la clase del modelo.
            return new ValidationResult(ErrorMessage ?? "*El DNI debe ser un número de 7 u 8 dígitos.");
        }
    }
}

public class ValidaTelefono : ValidationAttribute

{
    private readonly Regex _regex = new Regex(@"^\d{10}$");

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Permite que otros atributos como [Required] manejen los valores nulos.
        }
        string tel = value.ToString();
        if (_regex.IsMatch(tel))
        {
            return ValidationResult.Success; // La validación fue exitosa.
        }
        else
        {
            // La validación falló. El mensaje de error puede ser definido aquí o en la clase del modelo.
            return new ValidationResult(ErrorMessage ?? "*El teléfono debe ser un número de 10 dígitos.");
        }
    }
}