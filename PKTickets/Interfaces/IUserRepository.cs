using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Interfaces
{
    public interface IUserRepository
    {

        public List<User> GetAllUsers();
        public User UserById(int id);
        public User UserByPhone(string phone);
        public User UserByEmail(string email);
        public Messages CreateUser(UserDTO user);
        public Messages UpdateUser(User user);
        public Messages DeleteUser(int userId);
        public LoginResultDTO GetLoginDetail(string emailId, string password);
        public IEnumerable<UserDTO> UsersList();
        public Role GetRoleById(int id);
        public List<Role> GetAllRole();

    }
}
