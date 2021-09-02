
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
        CreateMap<LibroDTO, Libro>()
            .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
        CreateMap<ComentarioDTO, Comentario>().ReverseMap();
    }

    private List<AutorLibro> MapAutoresLibros(LibroDTO libroCreacionDTO, Libro libro)
    {
        var resultado = new List<AutorLibro>();

        if(libroCreacionDTO.AutoresIds == null) { return resultado; }

        foreach (var autorId in libroCreacionDTO.AutoresIds)
        {
            resultado.Add(new AutorLibro() { AutorId = autorId });
        }

        return resultado;
    }
}
