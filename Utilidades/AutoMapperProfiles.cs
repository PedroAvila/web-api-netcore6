
using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades;
public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AutorCreacionDTO, Autor>();
        CreateMap<Autor, AutorDTO>();
        CreateMap<LibroDTO, Libro>().ReverseMap();
        CreateMap<ComentarioDTO, Comentario>().ReverseMap();
    }
}
