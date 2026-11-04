using AuthUserApi.Models;
using AuthUserApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthUserApi.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly AuthuserContext _context;

        public RolesController(AuthuserContext context)
        {
            _context = context;
        }

        [HttpPost("NewRole")]
        public async Task<IActionResult> NewRole([FromBody] NewRoleDTO newRoleDTO)
        {
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = newRoleDTO.Name,
                RegTime = DateTime.Now
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Felvéve", result = role });
        }

        [HttpGet("AllRole")]
        public async Task<IActionResult> AllRole()
        {
            var roles = await _context.Roles.ToListAsync();
            return StatusCode(200, new { message = "Lekérdezve", result = roles });
        }

        [HttpGet("ById")]
        public async Task<IActionResult> RoleById([FromQuery] Guid id)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == id);
            if (role is not null)
            {
                return StatusCode(200, new { message = "Lekérdezve", result = role });
            }
            return StatusCode(404, new { message = "Nem található szerepkör", result = role });
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromQuery] Guid id, [FromBody] UpdateRoleDTO updateRoleDTO)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == id);
            if (role is not null)
            {
                role.Name = updateRoleDTO.Name;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "Frissítve", result = role });
            }
            return StatusCode(404, new { message = "Nem található szerepkör", result = role });
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromQuery] Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role is not null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "Törölve", result = role });
            }
            return StatusCode(404, new { message = "Nem található szerepkör", result = role });
        }
    }
}
