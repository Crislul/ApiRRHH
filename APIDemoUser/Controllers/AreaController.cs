using APIDemoUser.Data;
using APIDemoUser.DTOs.Area;
using APIDemoUser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDemoUser.Controllers
{
    [Route("api/area")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AreaController(ApplicationDbContext context)
        {
            _context = context;
        }


        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AreaDto>>> GetAreasTrabajo()
        {
            var areas = await _context.Areas.ToListAsync();
            return Ok(areas.Select(a => new AreaDto
            {
                Id = a.Id,
                Nombre = a.Nombre
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AreaDto>> GetAreaTrabajo(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null) return NotFound();
            return Ok(new AreaDto { Id = area.Id, Nombre = area.Nombre });
        }

        [HttpPost]
        public async Task<ActionResult<AreaDto>> CreateAreaTrabajo(CreateAreaDto areaDto)
        {
            var area = new Area { Nombre = areaDto.Nombre };
            _context.Areas.Add(area);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAreaTrabajo), new { id = area.Id }, new AreaDto { Id = area.Id, Nombre = area.Nombre });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAreaTrabajo(int id, UpdateAreaDto areaDto)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null) return NotFound();

            area.Nombre = areaDto.Nombre;
            _context.Entry(area).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAreaTrabajo(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null) return NotFound();

            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
