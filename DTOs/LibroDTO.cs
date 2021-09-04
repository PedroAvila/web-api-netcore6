﻿
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs;
public class LibroDTO
{
    public int Id { get; set; }
    [PrimeraLetraMayuscula]
    [StringLength(maximumLength: 250)]
    public string Titulo { get; set; }

    //public List<ComentarioDTO> Comentarios { get; set; }
    public List<int> AutoresIds { get; set; }
    public List<AutorDTO> Autores { get; set; }
}
