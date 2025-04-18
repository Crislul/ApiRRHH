using APIDemoUser.Data;
using APIDemoUser.DTOs.Expendiente;
using APIDemoUser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDemoUser.Controllers
{
    [Route("api/expediente")]
    [ApiController]
    public class ExpedienteController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ExpedienteController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }



        [HttpPost("subir")]
        public async Task<IActionResult> SubirExpediente([FromForm] ExpedienteCreateDto dto)
        {
            if (dto.Archivo == null || dto.Archivo.Length == 0)
                return BadRequest("Archivo inválido");

            // Validar extensión
            var extensionesPermitidas = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.Archivo.FileName).ToLower();
            if (!extensionesPermitidas.Contains(extension))
                return BadRequest("Formato no permitido");

            // Crear carpeta del usuario si no existe
            var carpetaUsuario = Path.Combine(_env.WebRootPath, "expedientes", dto.UsuarioId.ToString());
            if (!Directory.Exists(carpetaUsuario))
                Directory.CreateDirectory(carpetaUsuario);

            // Guardar archivo
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var rutaArchivo = Path.Combine(carpetaUsuario, nombreArchivo);
            using (var stream = new FileStream(rutaArchivo, FileMode.Create))
            {
                await dto.Archivo.CopyToAsync(stream);
            }

            // Guardar en BD
            var expediente = new Expediente
            {
                UsuarioId = dto.UsuarioId,
                Documento = dto.Documento,
                Archivo = Path.Combine("expedientes", dto.UsuarioId.ToString(), nombreArchivo),
            };

            _context.Expedientes.Add(expediente);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Documento subido exitosamente", expediente });
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerExpedientes(int usuarioId)
        {
            var documentos = await _context.Expedientes
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync();

            return Ok(documentos);
        }

        [HttpPut("usuario/{usuarioId}/{id}")]
        public async Task<IActionResult> ActualizarDocumento(int usuarioId, int id, [FromForm] IFormFile archivo, [FromForm] string documento)
        {
            var expedienteExistente = await _context.Expedientes.FindAsync(id);

            if (expedienteExistente == null || expedienteExistente.UsuarioId != usuarioId)
            {
                return NotFound(new { mensaje = "Documento no encontrado para este usuario" });
            }

            // Ruta del nuevo archivo
            var rutaCarpeta = Path.Combine("expedientes", usuarioId.ToString());
            var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
            var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

            // Crear carpeta si no existe
            var rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", rutaCarpeta);
            if (!Directory.Exists(rutaFisica))
            {
                Directory.CreateDirectory(rutaFisica);
            }

            // Guardar nuevo archivo en disco
            var rutaFisicaArchivo = Path.Combine(rutaFisica, nombreArchivo);
            using (var stream = new FileStream(rutaFisicaArchivo, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            // Eliminar archivo anterior si existe
            var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", expedienteExistente.Archivo ?? "");
            if (System.IO.File.Exists(rutaAnterior))
            {
                System.IO.File.Delete(rutaAnterior);
            }

            // Actualizar los datos
            expedienteExistente.Documento = documento;
            expedienteExistente.Archivo = Path.Combine(rutaCarpeta, nombreArchivo);
            expedienteExistente.FechaSubida = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Documento actualizado correctamente" });
        }


        [HttpDelete("usuario/{usuarioId}/{id}")]
        public async Task<IActionResult> EliminarDocumento(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null)
                return NotFound();

            _context.Expedientes.Remove(expediente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

       

}
