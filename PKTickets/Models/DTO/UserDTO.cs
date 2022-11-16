using Microsoft.AspNetCore.Mvc.Rendering;

namespace PKTickets.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }

        public string Location { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Role { get; set; }
        public List<SelectListItem> RoleIds { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
