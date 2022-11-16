using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Interfaces
{
    public interface IRoleRepository
    {
        public List<Role> GetAllRoles();
        public Role RoleById(int id);
        public Role RoleByName(string name);
        public Messages DeleteRole(int roleId);
        public Messages CreateRole(Role role);
        public Messages Update(Role role);

    }
}
