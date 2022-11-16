using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;
using System.Linq;
using System.Xml.Linq;

namespace PKTickets.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly PKTicketsDbContext db;
        public RoleRepository(PKTicketsDbContext db)
        {
            this.db = db;
        }

        public List<Role> GetAllRoles()
        {
            return db.Roles.Where(x => x.IsActive == true).ToList();
        }
  
        public Role RoleById(int id)
        {
            var roleExist = db.Roles.Where(x => x.IsActive == true).FirstOrDefault(x => x.RoleId == id);
            return roleExist;
        }
        public Role RoleByName(string name)
        {
            var roleExist = db.Roles.Where(x => x.IsActive == true).FirstOrDefault(x => x.RoleName == name);
            return roleExist;
        }
        public Messages DeleteRole(int roleId)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var role = RoleById(roleId);
            if (role == null)
            {
                messages.Message = "Role Id is not found";
            }
            else
            {
                role.IsActive = false;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Role is succssfully deleted";
            }
            return messages;
        }

        public Messages CreateRole(Role role)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var roleExist = db.Roles.FirstOrDefault(x => x.RoleName == role.RoleName);
            if (roleExist != null)
            {
                messages.Message = "Role Name is already Registered.";
            }
            else
            {
                db.Roles.Add(role);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Role is succssfully added";
            }
            return messages;
        }

        public Messages Update(Role role)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var roleExist = RoleById(role.RoleId);
            var nameExist = db.Roles.FirstOrDefault(x => x.RoleName == role.RoleName);
            if (roleExist == null)
            {
                messages.Message = "Role Id is not found";
            }
             else if (nameExist != null )
            {
                messages.Message = "Role Name is already registered";
            }
            else
            {
                roleExist.RoleName= role.RoleName;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Role is succssfully Updated";
            }
            return messages;
        }

    }
}
