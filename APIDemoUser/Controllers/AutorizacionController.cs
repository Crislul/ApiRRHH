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
                Asunto = a.Asunto,
                Fecha = a.Fecha,
                EstatusDir = a.EstatusDir,
                EstatusAdmin = a.EstatusAdmin
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
            Asunto = autorizacion.Asunto,
            Fecha = autorizacion.Fecha,
            EstatusDir = autorizacion.EstatusDir,
            EstatusAdmin = autorizacion.EstatusAdmin

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
            
            HoraSalida = autorizacionDto.HoraSalida,
            HoraEntrada = autorizacionDto.HoraEntrada,
            HorarioTrabajo = autorizacionDto.HorarioTrabajo,
            Asunto = autorizacionDto.Asunto,
            Fecha = autorizacionDto.Fecha,
            EstatusDir = autorizacionDto.EstatusDir,
            EstatusAdmin = autorizacionDto.EstatusAdmin,
            UsuarioId = usuario.Id,
            AreaId = area.Id,
            CategoriaId = categoria.Id
        };

        _context.Autorizaciones.Add(autorizacion);
        // Crear la notificación
        await _context.SaveChangesAsync();

        // Crear la notificación para el admin
        var notificacion = new Notificacion
        {
            Mensaje = $"El usuario {usuario.Nombre} generó una solicitud de salida",
            Tipo = "salida",
            Estado = "pendiente",
            Fecha = DateTime.Now,
            PermisoId = autorizacion.Id,
            UsuarioId = usuario.Id,
            TipoPermiso = "salida"
        };

        _context.Notificaciones.Add(notificacion);
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
            Asunto = autorizacion.Asunto,
            Fecha = autorizacion.Fecha,
            EstatusDir = autorizacion.EstatusDir,
            EstatusAdmin = autorizacion.EstatusAdmin
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
        autorizacion.Asunto = autorizacionDto.Asunto;
        autorizacion.Fecha = autorizacionDto.Fecha;
        autorizacion.EstatusDir = autorizacionDto.EstatusDir;
        autorizacion.EstatusAdmin = autorizacionDto.EstatusAdmin;

        _context.Entry(autorizacion).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        // Crear notificación para el usuario
        if (autorizacionDto.EstatusAdmin == 1 || autorizacionDto.EstatusAdmin == 2)
        {
            var mensaje = autorizacionDto.EstatusAdmin == 1
                ? $"Tu solicitud de autorización de salida {autorizacion.Id}, ha sido : Aceptada"
                : $"Tu solicitud de autorización de salida {autorizacion.Id}, ha sido : Rechazada";

            var notificacion = new Notificacion
            {
                Mensaje = mensaje,
                Tipo = "respuesta",
                Estado = "pendiente",
                Fecha = DateTime.Now,
                PermisoId = autorizacion.Id,
                UsuarioId = autorizacion.UsuarioId,
                TipoPermiso = "salida"
            };

            _context.Notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();
        }
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


