
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/libros")]
public class LibrosController: ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public LibrosController( ApplicationDbContext context, IMapper mapper )
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LibroDTO>> Get(int id)
    {
        //var libro = await context.Libros.Include(x=>x.Comentarios).FirstOrDefaultAsync(x => x.Id == id);
        var libro = await context.Libros
            .Include(x => x.AutoresLibros)
            .ThenInclude(x => x.Autor)
            .FirstOrDefaultAsync(x => x.Id == id);

        libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

        return mapper.Map<LibroDTO>(libro);
    }

    [HttpPost]
    public async Task<ActionResult> Post(LibroDTO libroDto)
    {
        if (libroDto.AutoresIds == null)
        {
            return BadRequest("No se puede crear un libro sin autores.");
        }

        var autoresIds = await context.Autores.Where(x => libroDto.AutoresIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();

        if (libroDto.AutoresIds.Count != autoresIds.Count)
        {
            return BadRequest("No existe uno de los autores enviados");
        }

        var libro = mapper.Map<Libro>(libroDto);

        if (libro.AutoresLibros != null)
        {
            for (int i = 0; i < libro.AutoresLibros.Count; i++)
            {
                libro.AutoresLibros[i].Orden = i;
            }
        }

        context.Add(libro);
        await context.SaveChangesAsync();
        return Ok();
    }

}
