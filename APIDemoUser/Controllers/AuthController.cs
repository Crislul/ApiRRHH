using APIDemoUser.Data;
using APIDemoUser.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDemoUser.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> AutenticarUsuario([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.ContrasenaHash))
                return BadRequest("Datos inválidos.");

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == request.Correo && u.ContrasenaHash == request.ContrasenaHash);

            if (usuario == null)
                return Ok(new { autenticado = false });

            return Ok(new { 

                autenticado = true, 
                tipoUsuario = usuario.TipoUsuario,
                areaUsuario = usuario.AreaId,
                apellidoP = usuario.ApellidoP,
                apellidoM = usuario.ApellidoM,
                nombre = usuario.Nombre, 
                id = usuario.Id 
            });
        }
    }
}
