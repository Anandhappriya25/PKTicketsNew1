using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly IRoleRepository roleRepository;

        public RolesController(IRoleRepository _roleRepository)
        {
            roleRepository = _roleRepository;
        }


        [HttpGet("GetRoleList")]
        public IActionResult GetRoleList()
        {
            var role = roleRepository.GetAllRoles();
            if (role.Count() == 0)
            {
                throw new Exception("The Role list is empty");
            }
            return Ok(role);
        }

        [HttpGet("RoleById/{roleId}")]

        public ActionResult RoleById(int roleId)
        {
            var role = roleRepository.RoleById(roleId);
            if (role == null)
            {
                throw new Exception("The Role Id is not found");
            }
            return Ok(role);
        }


        [HttpGet("RoleByName/{roleName}")]

        public ActionResult RoleByName(string roleName)
        {
            var role = roleRepository.RoleByName(roleName);
            if (role == null)
            {
                throw new Exception("The Role Name is not found");
            }
            return Ok(role);
        }

        [HttpPost("Add")]

        public IActionResult Add(Role role)
        {
            return Ok(roleRepository.CreateRole(role));
        }


        [HttpPut("Update")]
        public ActionResult Update(Role role)
        {
            return Ok(roleRepository.Update(role));
        }


        [HttpPut("RemoveById/{roleId}")]

        public IActionResult RemoveById(int roleId)
        {
            return Ok(roleRepository.DeleteRole(roleId));
        }
    }
}
