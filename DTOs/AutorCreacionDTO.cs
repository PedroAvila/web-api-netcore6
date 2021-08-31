﻿
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs;
public class AutorCreacionDTO
{
    [Required(ErrorMessage = "El campo nombre es requerido")]
    [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener más de {1} carácteres")]
    [PrimeraLetraMayuscula]
    public string Nombre { get; set; }
    
}
