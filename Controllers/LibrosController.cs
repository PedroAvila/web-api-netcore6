﻿
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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

    [HttpGet("{id:int}", Name = "obtenerLibro")]
    public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
    {
        //var libro = await context.Libros.Include(x=>x.Comentarios).FirstOrDefaultAsync(x => x.Id == id);
        var libro = await context.Libros
            .Include(x => x.AutoresLibros)
            .ThenInclude(x => x.Autor)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (libro == null)
        {
            return NotFound();
        }

        libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

        return mapper.Map<LibroDTOConAutores>(libro);
    }

    [HttpPost]
    public async Task<ActionResult> Post(LibroCreacionDTO libroDto)
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

        AsignarOrdenAutores(libro);

        context.Add(libro);
        await context.SaveChangesAsync();

        var _libroDto = mapper.Map<LibroDTO>(libro);

        return CreatedAtRoute("obtenerLibro", new { id = libro.Id }, _libroDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO)
    {
        var libroDB = await context.Libros
            .Include(x => x.AutoresLibros)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (libroDB == null)
        {
            return NotFound();
        }

        libroDB = mapper.Map(libroCreacionDTO, libroDB);
        AsignarOrdenAutores(libroDB);
        await context.SaveChangesAsync();
        return NoContent();
    }

    private void AsignarOrdenAutores(Libro libro)
    {
        if (libro.AutoresLibros != null)
        {
            for (int i = 0; i < libro.AutoresLibros.Count; i++)
            {
                libro.AutoresLibros[i].Orden = i;
            }
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
    {
        if (patchDocument == null)
        {
            return BadRequest();
               
        }

        var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
        if (libroDB == null)
        {
            return NotFound();
        }

        var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);
        patchDocument.ApplyTo(libroDTO, ModelState);
        var esValido = TryValidateModel(libroDTO);
        if (!esValido)
        {
            return BadRequest(ModelState);
        }
        mapper.Map(libroDTO, libroDB);
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existe = await context.Libros.AnyAsync(x => x.Id == id);
        if (!existe)
        {
            return NotFound();
        }

        context.Remove(new Libro() { Id = id });
        await context.SaveChangesAsync();
        return NoContent();
    }

}
