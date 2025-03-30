using APIDemoUser.Data;
using APIDemoUser.DTOs.Autorizacion;
using APIDemoUser.DTOs.Incidencia;
using APIDemoUser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult<IEnumerable<AutorizacionDto>>> GetAutorizaciones([FromQuery] int? idUsuario)
    {
        var query = _context.Autorizaciones
            .Include(a => a.Usuario)
            .Include(a => a.Area)
            .Include(a => a.Categoria)
            .AsQueryable();

        if (idUsuario.HasValue)
        {
            query = query.Where(i => i.UsuarioId == idUsuario.Value);
        }

        var autorizaciones = await query
            .Select(a => new AutorizacionDto
            {
                Id = a.Id,
                UsuarioId = a.UsuarioId,
                UsuarioNombre = a.Usuario.Nombre,
                UsuarioApellidoP = a.Usuario.ApellidoP,
                UsuarioApellidoM = a.Usuario.ApellidoM,
                AreaId = a.AreaId,
                AreaNombre = a.Area.Nombre,
                CategoriaId = a.CategoriaId,
                CategoriaNombre = a.Categoria.Nombre,
                HoraSalida = a.HoraSalida,
                HoraEntrada = a.HoraEntrada,
                HorarioTrabajo = a.HorarioTrabajo,
                Lugar = a.Lugar,
                Asunto = a.Asunto,
                Fecha = a.Fecha,
                Estatus = a.Estatus
            })
            .ToListAsync();

        return Ok(autorizaciones);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<AutorizacionDto>> GetAutorizacion(int id)
    {
        var autorizacion = await _context.Autorizaciones
            .Include(a => a.Usuario)
            .Include(a => a.Area)
            .Include(a => a.Categoria)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (autorizacion == null) return NotFound();

        return Ok(new AutorizacionDto
        {
            Id = autorizacion.Id,
            UsuarioId = autorizacion.UsuarioId,
            UsuarioNombre = autorizacion.Usuario.Nombre,
            UsuarioApellidoP = autorizacion.Usuario.ApellidoP,
            UsuarioApellidoM = autorizacion.Usuario.ApellidoM,
            AreaId = autorizacion.AreaId,
            AreaNombre = autorizacion.Area.Nombre,
            CategoriaId = autorizacion.CategoriaId,
            CategoriaNombre = autorizacion.Categoria.Nombre,
            HoraSalida = autorizacion.HoraSalida,
            HoraEntrada = autorizacion.HoraEntrada,
            HorarioTrabajo = autorizacion.HorarioTrabajo,
            Lugar = autorizacion.Lugar,
            Asunto = autorizacion.Asunto,
            Fecha = autorizacion.Fecha,
            Estatus = autorizacion.Estatus
        });
    }


    [HttpPost]
    public async Task<ActionResult<AutorizacionDto>> CreateAutorizacion(CreateAutorizacionDto autorizacionDto)
    {


        // Asegurarse de que los IDs estén proporcionados
        if (autorizacionDto.UsuarioId == null)
            return BadRequest("El ID del usuario es obligatorio.");
        if (autorizacionDto.AreaId == null)
            return BadRequest("El ID del área es obligatorio.");
        if (autorizacionDto.CategoriaId == null)
            return BadRequest("El ID de la categoría es obligatorio.");
        

        // Buscar los usuarios, áreas, categorías y motivos por ID
        var usuario = await _context.Usuarios.FindAsync(autorizacionDto.UsuarioId);
        var area = await _context.Areas.FindAsync(autorizacionDto.AreaId);
        var categoria = await _context.Categorias.FindAsync(autorizacionDto.CategoriaId);
        

        // Validar que los datos existen
        if (usuario == null) return BadRequest("Usuario no encontrado.");
        if (area == null) return BadRequest("Área no encontrada.");
        if (categoria == null) return BadRequest("Categoría no encontrada.");
        
        
        
        
        var autorizacion = new Autorizacion
        {
            UsuarioId = usuario.Id,
            AreaId = area.Id,
            CategoriaId = categoria.Id,
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
            UsuarioNombre = usuario.Nombre,
            AreaId = autorizacion.AreaId,
            AreaNombre = area.Nombre,
            CategoriaId = autorizacion.CategoriaId,
            CategoriaNombre = categoria.Nombre,
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


