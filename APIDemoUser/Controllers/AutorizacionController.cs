using APIDemoUser.Data;
using APIDemoUser.DTOs.Autorizacion;
using APIDemoUser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/autorizacion")]
[ApiController]
public class AutorizacionController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AutorizacionController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AutorizacionDto>>> GetAutorizaciones()
    {
        var autorizaciones = await _context.Autorizaciones.ToListAsync();
        return Ok(autorizaciones.Select(a => new AutorizacionDto
        {
            Id = a.Id,
            UsuarioId = a.UsuarioId,
            AreaId = a.AreaId,
            CategoriaId = a.CategoriaId,
            HoraSalida = a.HoraSalida,
            HoraEntrada = a.HoraEntrada,
            HorarioTrabajo = a.HorarioTrabajo,
            Lugar = a.Lugar,
            Asunto = a.Asunto,
            Fecha = a.Fecha,
            Estatus = a.Estatus
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AutorizacionDto>> GetAutorizacion(int id)
    {
        var autorizacion = await _context.Autorizaciones.FindAsync(id);
        if (autorizacion == null) return NotFound();
        return Ok(new AutorizacionDto
        {
            Id = autorizacion.Id,
            UsuarioId = autorizacion.UsuarioId,
            AreaId = autorizacion.AreaId,
            CategoriaId = autorizacion.CategoriaId,
            HoraSalida = autorizacion.HoraSalida,
            HoraEntrada = autorizacion.HoraEntrada,
            HorarioTrabajo = autorizacion.HorarioTrabajo,
            Lugar = autorizacion.Lugar,
            Asunto = autorizacion.Asunto,
            Fecha = autorizacion.Fecha,
            Estatus= autorizacion.Estatus
        });
    }

    [HttpPost]
    public async Task<ActionResult<AutorizacionDto>> CreateAutorizacion(CreateAutorizacionDto autorizacionDto)
    {
        var autorizacion = new Autorizacion
        {
            UsuarioId = autorizacionDto.UsuarioId,
            AreaId = autorizacionDto.AreaId,
            CategoriaId = autorizacionDto.CategoriaId,
            HoraSalida = autorizacionDto.HoraSalida,
            HoraEntrada = autorizacionDto.HoraEntrada,
            HorarioTrabajo = autorizacionDto.HorarioTrabajo,
            Lugar = autorizacionDto.Lugar,
            Asunto = autorizacionDto.Asunto,
            Fecha = autorizacionDto.Fecha,
            Estatus = autorizacionDto.Estatus
        };
        _context.Autorizaciones.Add(autorizacion);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAutorizacion), new { id = autorizacion.Id }, new AutorizacionDto
        {
            Id = autorizacion.Id,
            UsuarioId = autorizacion.UsuarioId,
            AreaId = autorizacion.AreaId,
            CategoriaId = autorizacion.CategoriaId,
            HoraSalida = autorizacion.HoraSalida,
            HoraEntrada = autorizacion.HoraEntrada,
            HorarioTrabajo = autorizacion.HorarioTrabajo,
            Lugar = autorizacion.Lugar,
            Asunto = autorizacion.Asunto,
            Fecha = autorizacion.Fecha,
            Estatus = autorizacion.Estatus
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAutorizacion(int id, UpdateAutorizacionDto autorizacionDto)
    {
        var autorizacion = await _context.Autorizaciones.FindAsync(id);
        if (autorizacion == null) return NotFound();

        autorizacion.UsuarioId = autorizacionDto.UsuarioId;
        autorizacion.AreaId = autorizacionDto.AreaId;
        autorizacion.CategoriaId = autorizacionDto.CategoriaId;
        autorizacion.HoraSalida = autorizacionDto.HoraSalida;
        autorizacion.HoraEntrada = autorizacionDto.HoraEntrada;
        autorizacion.HorarioTrabajo = autorizacionDto.HorarioTrabajo;
        autorizacion.Lugar = autorizacionDto.Lugar;
        autorizacion.Asunto = autorizacionDto.Asunto;
        autorizacion.Fecha = autorizacionDto.Fecha;
        autorizacion.Estatus = autorizacionDto.Estatus;

        _context.Entry(autorizacion).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAutorizacion(int id)
    {
        var autorizacion = await _context.Autorizaciones.FindAsync(id);
        if (autorizacion == null) return NotFound();

        _context.Autorizaciones.Remove(autorizacion);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}


