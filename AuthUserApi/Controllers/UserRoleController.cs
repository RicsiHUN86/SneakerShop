using AuthUserApi.Models;
using AuthUserApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthUserApi.Controllers
{
    [Route("userrole")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public UserRoleController(AuthuserContext context)
        {
            _context = context;
        }


        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] NewUserRoleDTO newUserRoleDto)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id == newUserRoleDto.UserId);

                var role = await _context.Roles.FindAsync(newUserRoleDto.RoleId);

                if (user == null || role == null)
                {
                    return NotFound(new { message = "Hibás user vagy role ID" });
                }
                bool iscreated = user.Roles.Any(r => r.Id == newUserRoleDto.RoleId);
                if (iscreated)
                {
                    return Conflict(new { message = "A felhasználó már rendelkezik ezzel a szerepkörrel" });
                }

                user.Roles.Add(role);
                await _context.SaveChangesAsync();
                return StatusCode(201, new { message = "Sikeres szerep hozzárendelés" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] DeleteUserRoleDTO userRoleDto)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id == userRoleDto.UserId);

                var role = await _context.Roles.FindAsync(userRoleDto.RoleId);

                if (user == null || role == null)
                {
                    return NotFound(new { message = "Hibás user vagy role ID" });
                }

                if (!user.Roles.Contains(role))
                {
                    return NotFound(new { message = "A felhasználó nem rendelkezik az adott szerepkörrel" });
                }

                user.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Sikeres szerep eltávolítás" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AllUsersAllRoled")]
        public async Task<IActionResult> GetAllUsersAndRoles()
        {
            var users = await _context.Users
                .Include(u => u.Roles)
                .Select(u => new
                {
                    Users = u.Name,
                    UserRoles = u.Roles,
                })
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet("userByIdAndRoles")]
        public async Task<IActionResult> UserByIdAndRoles([FromQuery] Guid id)
        {
            var user = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    UserRoles = u.Roles
                    .Select(r => r.Name).ToList()
                })
                .SingleOrDefaultAsync(u => u.Id == id);

            return Ok(user);
        }
    }
}
