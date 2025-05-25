using APIDemoUser.Data;
using APIDemoUser.DTOs.Incidencia;
using APIDemoUser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using static System.Runtime.InteropServices.JavaScript.JSType;

[Route("api/incidencia")]
[ApiController]
public class IncidenciaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public IncidenciaController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncidenciaDto>>> GetIncidencias([FromQuery] int? idUsuario)
    {
        var query = _context.Incidencias
            .Include(i => i.Usuario)
            .Include(i => i.Area)
            .Include(i => i.Categoria)
            .Include(i => i.Motivo)
            .AsQueryable();

        // Filtrar por idUsuario si se proporciona
        if (idUsuario.HasValue)
        {
            query = query.Where(i => i.UsuarioId == idUsuario.Value);
        }

        var incidencias = await query
            .Select(i => new IncidenciaDto
            {
                Id = i.Id,
                Descripcion = i.Descripcion,
                Fecha = i.Fecha,
                FechaInicio = i.FechaInicio,
                FechaFin = i.FechaFin,
                UsuarioId = i.UsuarioId,
                UsuarioNombre = i.Usuario.Nombre,
                UsuarioApellidoP = i.Usuario.ApellidoP,
                UsuarioApellidoM = i.Usuario.ApellidoM,
                AreaId = i.AreaId,
                AreaNombre = i.Area.Nombre,
                CategoriaId = i.CategoriaId,
                CategoriaNombre = i.Categoria.Nombre,
                MotivoId = i.MotivoId,
                MotivoNombre = i.Motivo.Nombre,
                EstatusDir = i.EstatusDir,
                EstatusAdmin = i.EstatusAdmin,
                NombreArchivo = i.NombreArchivo,
                
            })
            .ToListAsync();

        return Ok(incidencias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IncidenciaDto>> GetIncidencia(int id)
    {
        var incidencia = await _context.Incidencias
            .Include(i => i.Usuario)
            .Include(i => i.Area)
            .Include(i => i.Categoria)
            .Include(i => i.Motivo)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (incidencia == null) return NotFound();

        return Ok(new IncidenciaDto
        {
            Id = incidencia.Id,
            Descripcion = incidencia.Descripcion,
            Fecha = incidencia.Fecha,
            FechaInicio = incidencia.FechaInicio,
            FechaFin = incidencia.FechaFin,
            UsuarioId = incidencia.UsuarioId,
            UsuarioNombre = incidencia.Usuario.Nombre,
            UsuarioApellidoP = incidencia.Usuario.ApellidoP,
            UsuarioApellidoM = incidencia.Usuario.ApellidoM,
            AreaId = incidencia.AreaId,
            AreaNombre = incidencia.Area.Nombre,
            CategoriaId = incidencia.CategoriaId,
            CategoriaNombre = incidencia.Categoria.Nombre,
            MotivoId = incidencia.MotivoId,
            MotivoNombre = incidencia.Motivo.Nombre,
            EstatusDir = incidencia.EstatusDir,
            EstatusAdmin = incidencia.EstatusAdmin,
            NombreArchivo = incidencia.NombreArchivo,
            
        });
    }


    [HttpPost]
    public async Task<ActionResult<IncidenciaDto>> CreateIncidencia(CreateIncidenciaDto incidenciaDto)
    {
        // Asegurarse de que los IDs estén proporcionados
        if (incidenciaDto.UsuarioId == null)
            return BadRequest("El ID del usuario es obligatorio.");
        if (incidenciaDto.AreaId == null)
            return BadRequest("El ID del área es obligatorio.");
        if (incidenciaDto.CategoriaId == null)
            return BadRequest("El ID de la categoría es obligatorio.");
        if (incidenciaDto.MotivoId == null)
            return BadRequest("El ID del motivo es obligatorio.");

        // Buscar los usuarios, áreas, categorías y motivos por ID
        var usuario = await _context.Usuarios.FindAsync(incidenciaDto.UsuarioId);
        var area = await _context.Areas.FindAsync(incidenciaDto.AreaId);
        var categoria = await _context.Categorias.FindAsync(incidenciaDto.CategoriaId);
        var motivo = await _context.Motivos.FindAsync(incidenciaDto.MotivoId);

        // Validar que los datos existen
        if (usuario == null) return BadRequest("Usuario no encontrado.");
        if (area == null) return BadRequest("Área no encontrada.");
        if (categoria == null) return BadRequest("Categoría no encontrada.");
        if (motivo == null) return BadRequest("Motivo no encontrado.");

        // Crear la incidencia con los IDs
        var incidencia = new Incidencia
        {
            Descripcion = incidenciaDto.Descripcion,
            Fecha = incidenciaDto.Fecha,
            FechaInicio = incidenciaDto.FechaInicio,
            FechaFin = incidenciaDto.FechaFin,
            UsuarioId = usuario.Id,
            AreaId = area.Id,
            CategoriaId = categoria.Id,
            MotivoId = motivo.Id
            
        };

       
        _context.Incidencias.Add(incidencia);
        await _context.SaveChangesAsync();

        // Crear notificación para RRHH 
        var notiRRHH = new Notificacion
        {
            Mensaje = $"El usuario {usuario.Nombre} generó una nueva incidencia",
            Tipo = "incidencia",
            Estado = "pendiente",
            Fecha = DateTime.Now,
            PermisoId = incidencia.Id,
            TipoPermiso = "incidencia",
            Rol = 2
        };

        // Crear notificación para el Director 
        var notiDir = new Notificacion
        {
            Mensaje = $"El usuario {usuario.Nombre} generó una nueva incidencia",
            Tipo = "incidencia",
            Estado = "pendiente",
            Fecha = DateTime.Now,
            PermisoId = incidencia.Id,
            TipoPermiso = "incidencia",
            Rol = 3
        };

        _context.Notificaciones.AddRange(notiRRHH, notiDir);
        await _context.SaveChangesAsync();





        // Devolver la incidencia con los nombres en la respuesta
        return CreatedAtAction(nameof(GetIncidencia), new { id = incidencia.Id }, new IncidenciaDto
        {
            Id = incidencia.Id,
            Descripcion = incidencia.Descripcion,
            Fecha = incidencia.Fecha,
            FechaInicio = incidencia.FechaInicio,
            FechaFin = incidencia.FechaFin,
            UsuarioId = incidencia.UsuarioId,
            UsuarioNombre = usuario.Nombre,
            AreaId = incidencia.AreaId,
            AreaNombre = area.Nombre,
            CategoriaId = incidencia.CategoriaId,
            CategoriaNombre = categoria.Nombre,
            MotivoId = incidencia.MotivoId,
            MotivoNombre = motivo.Nombre,
            EstatusDir = incidencia.EstatusDir,
            EstatusAdmin = incidencia.EstatusAdmin
        });
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncidencia(int id, UpdateIncidenciaDto incidenciaDto)
    {
        var incidencia = await _context.Incidencias.FindAsync(id);
        if (incidencia == null) return NotFound();

        incidencia.Descripcion = incidenciaDto.Descripcion;
        incidencia.Fecha = incidenciaDto.Fecha;
        incidencia.FechaInicio = incidenciaDto.FechaInicio;
        incidencia.FechaFin = incidenciaDto.FechaFin;
        incidencia.UsuarioId = incidenciaDto.UsuarioId;
        incidencia.AreaId = incidenciaDto.AreaId;
        incidencia.CategoriaId = incidenciaDto.CategoriaId;
        incidencia.MotivoId = incidenciaDto.MotivoId;
        incidencia.EstatusDir = incidenciaDto.EstatusDir;
        incidencia.EstatusAdmin = incidenciaDto.EstatusAdmin;

        _context.Entry(incidencia).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        // Crear notificación para el usuario
        // Verifica si RRHH acaba de responder
        if (incidenciaDto.EstatusAdmin == 1 || incidenciaDto.EstatusAdmin == 2)
        {
            var yaExisteRRHH = await _context.Notificaciones.AnyAsync(n =>
                n.PermisoId == incidencia.Id &&
                n.Tipo == "respuesta" &&
                n.TipoPermiso == "incidencia" &&
                n.Mensaje.Contains("El departamento de Recursos Humanos"));

            if (!yaExisteRRHH)
            {
                var mensaje = incidenciaDto.EstatusAdmin == 1
                    ? $"El departamento de Recursos Humanos aceptó tu solicitud de incidencia : {incidencia.Id}."
                    : $"El departamento de Recursos Humanos rechazó tu solicitud de incidencia : {incidencia.Id}.";

                var notificacionRRHH = new Notificacion
                {
                    Mensaje = mensaje,
                    Tipo = "respuesta",
                    Estado = "pendiente",
                    Fecha = DateTime.Now,
                    PermisoId = incidencia.Id,
                    UsuarioId = incidenciaDto.UsuarioId,
                    TipoPermiso = "incidencia"
                };

                _context.Notificaciones.Add(notificacionRRHH);
                await _context.SaveChangesAsync();
            }
        }


        // Verifica si Dirección acaba de responder
        if (incidenciaDto.EstatusDir == 1 || incidenciaDto.EstatusDir == 2)
        {
            var yaExisteDir = await _context.Notificaciones.AnyAsync(n =>
                n.PermisoId == incidencia.Id &&
                n.Tipo == "respuesta" &&
                n.TipoPermiso == "incidencia" &&
                n.Mensaje.Contains("Dirección"));

            if (!yaExisteDir)
            {
                var mensaje = incidenciaDto.EstatusDir == 1
                    ? $"La Dirección de carrera aceptó tu solicitud de incidencia : {incidencia.Id}."
                    : $"La Dirección de carrera rechazó tu solicitud de incidencia : {incidencia.Id}.";

                var notificacionDir = new Notificacion
                {
                    Mensaje = mensaje,
                    Tipo = "respuesta",
                    Estado = "pendiente",
                    Fecha = DateTime.Now,
                    PermisoId = incidencia.Id,
                    UsuarioId = incidenciaDto.UsuarioId,
                    TipoPermiso = "incidencia"
                };

                _context.Notificaciones.Add(notificacionDir);
                await _context.SaveChangesAsync();
            }
        }




        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncidencia(int id)
    {
        var incidencia = await _context.Incidencias.FindAsync(id);
        if (incidencia == null) return NotFound();

        _context.Incidencias.Remove(incidencia);
        await _context.SaveChangesAsync();
        return NoContent();
    }


    [HttpPost("subir-archivo/{id}")]
    public async Task<IActionResult> SubirArchivo(int id, IFormFile archivo)
    {
        if (archivo == null || archivo.Length == 0)
            return BadRequest("No se seleccionó ningún archivo.");

        var incidencia = await _context.Incidencias.FindAsync(id);
        if (incidencia == null)
            return NotFound("Incidencia no encontrada.");

        using (var ms = new MemoryStream())
        {
            await archivo.CopyToAsync(ms);
            incidencia.Archivo = ms.ToArray();
            incidencia.NombreArchivo = archivo.FileName;
        }

        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Archivo subido correctamente." });
    }

    [HttpGet("descargar-archivo/{id}")]
    public async Task<IActionResult> DescargarArchivo(int id)
    {
        var incidencia = await _context.Incidencias.FindAsync(id);
        if (incidencia == null || incidencia.Archivo == null)
            return NotFound("Documento no encontrado.");

        var contentType = ObtenerContentType(incidencia.NombreArchivo);
        return File(incidencia.Archivo, contentType, incidencia.NombreArchivo);
    }

    private string ObtenerContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}


