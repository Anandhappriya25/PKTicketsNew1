using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKTickets.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Please Enter the Role Name !!!")]
        [StringLength(20, ErrorMessage = "Role Name length must be below 20 characters")]
        public string RoleName { get;set; }

        public bool IsActive { get; set; } = true;
    }
}
