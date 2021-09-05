﻿
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/autores")]
public class AutoresController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public AutoresController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]               // api/autores
    public async Task<List<AutorDTO>> Get()
    {
        var autores = await context.Autores.ToListAsync();
        return mapper.Map<List<AutorDTO>>(autores);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
    {
        var autor = await context.Autores
            .Include(x => x.AutoresLibros)
            .ThenInclude(x => x.Libro)
            .FirstOrDefaultAsync(autorDB => autorDB.Id == id);
        if (autor == null)
        {
            return NotFound();
        }
        return mapper.Map<AutorDTOConLibros>(autor);
    }

    [HttpGet("{nombre}")] // FromRoute viene como su propiio nombre lo indica del Route
    public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
    {
        var autores = await context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();

        return mapper.Map<List<AutorDTO>>(autores);
    }

    [HttpPost]// FromBody viene del cuerpo.
    public async Task<ActionResult<Autor>> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
    {
        var exist = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);

        if (exist)
        {
            return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");
        }

        var autor = mapper.Map<Autor>(autorCreacionDTO);
        context.Add(autor);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put( Autor autor, int id )
    {
        if (autor.Id != id)
        {
            return BadRequest("El id del autor no coincide con el id de la URL");
        }

        var existe = await context.Autores.AnyAsync(x => x.Id == id);
        if (!existe)
        {
            return NotFound();
        }

        context.Update(autor);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existe = await context.Autores.AnyAsync(x => x.Id == id);
        if (!existe)
        {
            return NotFound();
        }

        context.Remove(new Autor() { Id = id });
        await context.SaveChangesAsync();
        return Ok();
    }
}
