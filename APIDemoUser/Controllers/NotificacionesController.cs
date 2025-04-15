using APIDemoUser.Data;
using APIDemoUser.DTOs.Notifiacion;
using APIDemoUser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDemoUser.Controllers
{
    [ApiController]
    [Route("api/notificaciones")]

    public class NotificacionesController : Controller
    {
       

        private readonly ApplicationDbContext _context;
        public NotificacionesController(ApplicationDbContext context)
        {
            _context = context;
        }
        //todas las notis de solicitudes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notificacion>>> GetNotificaciones()
        {
            return await _context.Notificaciones
            .Where(n => n.Tipo != "respuesta")
            .OrderByDescending(n => n.Fecha)
            .ToListAsync();
        }






        [HttpPut("notificaciones/{id}/leida")]
        public async Task<IActionResult> MarcarComoLeida(int id)
        {

            var notificacion = await _context.Notificaciones.FindAsync(id);
            if (notificacion == null)
                return NotFound();

            notificacion.Estado = "leido";
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarNotificacion(int id)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);
            if (notificacion == null)
                return NotFound();

            _context.Notificaciones.Remove(notificacion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        //usuario notis respuesta
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Notificacion>>> GetNotificacionesPorUsuario(int usuarioId)
        {
            var notis = await _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId && n.Estado == "pendiente" && n.Tipo == "respuesta")
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();

            return Ok(notis);
        }


        [HttpPost]
        public async Task<IActionResult> CrearNotificacion(NotificacionDto dto)
        {
            var notificacion = new Notificacion
            {
                Mensaje = dto.Mensaje,
                Tipo = dto.Tipo
            };

            _context.Notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("marcar-leidas")]
        public async Task<IActionResult> MarcarComoLeidas()
        {
            var pendientes = _context.Notificaciones.Where(n => n.Estado == "pendiente");

            foreach (var noti in pendientes)
            {
                noti.Estado = "leido";
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
