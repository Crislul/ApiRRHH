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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notificacion>>> GetNotificacionesPendientes()
        {
            return await _context.Notificaciones
                .Where(n => n.Estado == "pendiente")
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
