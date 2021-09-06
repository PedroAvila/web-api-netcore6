
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
        CreateMap<Autor, AutorDTOConLibros>()
            .ForMember(dest => dest.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));
        CreateMap<LibroDTO, Libro>()
            .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
        CreateMap<LibroCreacionDTO, Libro>().ReverseMap();
        CreateMap<ComentarioDTO, Comentario>().ReverseMap();
        CreateMap<Libro, LibroDTO>();
        CreateMap<Libro, LibroDTOConAutores>()
            .ForMember(x => x.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
        CreateMap<LibroPatchDTO, Libro>().ReverseMap();
    }

    private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
    {
        var resultado = new List<LibroDTO>();

        if(autor.AutoresLibros == null) { return resultado; }
        foreach (var autorlibro in autor.AutoresLibros)
        {
            resultado.Add(new LibroDTO()
            {
                Id = autorlibro.LibroId,
                Titulo = autorlibro.Libro.Titulo
            });
        }

        return resultado;
    }

    private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
    {
        var resultado = new List<AutorDTO>();

        if (libro.AutoresLibros == null) { return resultado; };
        foreach (var autorLibro in libro.AutoresLibros)
        {
            resultado.Add(new AutorDTO()
            {
                Id = autorLibro.AutorId,
                Nombre = autorLibro.Autor.Nombre
            });
        }
        
        return resultado;
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
