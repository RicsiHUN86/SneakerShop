using Microsoft.AspNetCore.Mvc;

namespace AuthUserApi.Models.DTOs
{
    public class NewUserDTO
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
