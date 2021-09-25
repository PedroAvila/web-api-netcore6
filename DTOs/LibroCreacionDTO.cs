﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs;
public class LibroCreacionDTO
{
    [PrimeraLetraMayuscula]
    [StringLength(maximumLength: 250)]
    public string Titulo { get; set; }
    public DateTime FechaPublicacion { get; set; }
    public List<int> AutoresIds { get; set; }
}
