namespace AuthUserApi.Models.DTOs
{
    public class DeleteUserRoleDTO
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

    }
}