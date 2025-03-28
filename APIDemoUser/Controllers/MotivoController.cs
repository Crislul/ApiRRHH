using APIDemoUser.Data;
using APIDemoUser.DTOs.Motivo;
using APIDemoUser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/motivo")]
[ApiController]
public class MotivoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MotivoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MotivoDto>>> GetMotivos()
    {
        var motivos = await _context.Motivos.ToListAsync();
        return Ok(motivos.Select(m => new MotivoDto { Id = m.Id, Nombre = m.Nombre }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MotivoDto>> GetMotivo(int id)
    {
        var motivo = await _context.Motivos.FindAsync(id);
        if (motivo == null) return NotFound();
        return Ok(new MotivoDto { Id = motivo.Id, Nombre = motivo.Nombre });
    }

    [HttpPost]
    public async Task<ActionResult<MotivoDto>> CreateMotivo(CreateMotivoDto motivoDto)
    {
        var motivo = new Motivo { Nombre = motivoDto.Nombre };
        _context.Motivos.Add(motivo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMotivo), new { id = motivo.Id }, new MotivoDto { Id = motivo.Id, Nombre = motivo.Nombre });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMotivo(int id, UpdateMotivoDto motivoDto)
    {
        var motivo = await _context.Motivos.FindAsync(id);
        if (motivo == null) return NotFound();

        motivo.Nombre = motivoDto.Nombre;
        _context.Entry(motivo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMotivo(int id)
    {
        var motivo = await _context.Motivos.FindAsync(id);
        if (motivo == null) return NotFound();

        _context.Motivos.Remove(motivo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
