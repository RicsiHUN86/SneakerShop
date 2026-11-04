using AuthUserApi.Models;
using AuthUserApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthUserApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly AuthuserContext _context;

        public UsersController(AuthuserContext context)
        {
            _context = context;
        }
        [HttpPost("NewUser")]
        public async Task<ActionResult> NewUser([FromBody] NewUserDTO newUserDTO)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = newUserDTO.Name,
                Email = newUserDTO.Email,
                Password = newUserDTO.Password,
                RegTime = DateTime.Now
            };

            if (user is not null)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return StatusCode(201, new {message = "Felvéve", result = user});
            }
            else
            {
                return StatusCode(400, new{ message = "Hiba", result = user});
            }

        }
        [HttpGet("AllUser")]
        public async Task<IActionResult> AllUser()
        {
            var users = await _context.Users.ToListAsync();
            if (users is not null)
            {
                return StatusCode(200, new { message = "Lekérdezve", result = users });

            }
                return StatusCode(400, new { message = "Hiba", result = users });
        }

        [HttpGet("ById")]
        public async Task<IActionResult> UserById([FromQuery]Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user is not null)
            {
                return StatusCode(200, new { message = "Lekérdezve", result = user });
            }
            return StatusCode(404, new { message = "Nem található felhasználó", result = user });
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromQuery] Guid id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user is not null)
            {
                user.Name = updateUserDTO.Name;
                user.Email = updateUserDTO.Email;
                user.Password = updateUserDTO.Password;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "Frissítve", result = user });
            }
            return StatusCode(404, new { message = "Nem található felhasználó", result = user });
        }
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is not null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "Törölve", result = user });
            }
            return StatusCode(404, new { message = "Nem található felhasználó", result = user });
        }
    }
}
