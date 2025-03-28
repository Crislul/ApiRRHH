using APIDemoUser.Data;
using APIDemoUser.DTOs.User;
using APIDemoUser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDemoUser.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Crear un nuevo usuario
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] CreateUsuarioDto usuarioDto)
        {
            if (usuarioDto == null)
                return BadRequest("Datos inválidos.");

            var usuario = new Usuario
            {
                Nombre = usuarioDto.Nombre,
                ApellidoP = usuarioDto.ApellidoP,
                ApellidoM = usuarioDto.ApellidoM,
                Correo = usuarioDto.Correo,
                ContrasenaHash = usuarioDto.ContrasenaHash,
                TipoUsuario = usuarioDto.TipoUsuario
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerUsuarios), new { id = usuario.Id }, new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                ApellidoP = usuario.ApellidoP,
                ApellidoM = usuario.ApellidoM,
                Correo = usuario.Correo,
                TipoUsuario = usuario.TipoUsuario
            });
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                ApellidoP = u.ApellidoP,
                ApellidoM = u.ApellidoM,
                Correo = u.Correo,
                ContrasenaHash = u.ContrasenaHash,
                TipoUsuario = u.TipoUsuario
            }));
        }

        // Actualizar un usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UpdateUsuarioDto usuarioActualizadoDto)
        {
            if (usuarioActualizadoDto == null || id != usuarioActualizadoDto.Id)
                return BadRequest("Datos inválidos.");

            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
                return NotFound();

            usuarioExistente.Nombre = usuarioActualizadoDto.Nombre;
            usuarioExistente.ApellidoP = usuarioActualizadoDto.ApellidoP;
            usuarioExistente.ApellidoM = usuarioActualizadoDto.ApellidoM;
            usuarioExistente.Correo = usuarioActualizadoDto.Correo;
            usuarioExistente.ContrasenaHash = usuarioActualizadoDto.ContrasenaHash;
            usuarioExistente.TipoUsuario = usuarioActualizadoDto.TipoUsuario;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Eliminar un usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
