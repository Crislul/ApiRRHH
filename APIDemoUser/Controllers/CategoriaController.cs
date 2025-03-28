using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDemoUser.Data;
using APIDemoUser.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDemoUser.DTOs.Categoria;

[Route("api/categoria")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoriaController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
    {
        var categorias = await _context.Categorias.ToListAsync();
        return Ok(categorias.Select(c => new CategoriaDto { Id = c.Id, Nombre = c.Nombre }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaDto>> GetCategoria(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();
        return Ok(new CategoriaDto { Id = categoria.Id, Nombre = categoria.Nombre });
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> CreateCategoria(CreateCategoriaDto categoriaDto)
    {
        var categoria = new Categoria { Nombre = categoriaDto.Nombre };
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, new CategoriaDto { Id = categoria.Id, Nombre = categoria.Nombre });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategoria(int id, UpdateCategoriaDto categoriaDto)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();

        categoria.Nombre = categoriaDto.Nombre;
        _context.Entry(categoria).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
