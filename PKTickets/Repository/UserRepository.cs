using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly PKTicketsDbContext db;
        public UserRepository(PKTicketsDbContext db)
        {
            this.db = db;
        }

        public List<User> GetAllUsers()
        {
            return db.Users.Where(x => x.IsActive == true).ToList();
        }

        public User UserById(int id)
        {
            var userExist = db.Users.Where(x => x.IsActive == true).FirstOrDefault(x => x.UserId == id);
            return userExist;
        }

        public User UserByPhone(string phone)
        {
            var userExist = db.Users.Where(x => x.IsActive == true).FirstOrDefault(x => x.PhoneNumber == phone);
            return userExist;
        }

        public User UserByEmail(string email)
        {
            var userExist = db.Users.Where(x => x.IsActive == true).FirstOrDefault(x => x.EmailId == email);
            return userExist;
        }

        public Messages CreateUser(UserDTO user)
        {
            User users = new User();
            users.UserName = user.UserName;
            users.PhoneNumber = user.PhoneNumber;
            users.Location = user.Location;
            users.EmailId = user.EmailId;
            users.Password = user.Password;
            users.RoleId = user.RoleId;
            Messages messages = new Messages();
            messages.Success = false;
            messages.Role = user.Role;
            var phoneExist = db.Users.FirstOrDefault(x => x.PhoneNumber == user.PhoneNumber);
            var emailIdExist = db.Users.FirstOrDefault(x => x.EmailId == user.EmailId);
            if (phoneExist != null)
            {
                messages.Message = "PhoneNumber is already Registered.";
                return messages;
            }
            else if (emailIdExist != null)
            {
                messages.Message = "EmailId is already Registered.";
                return messages;
            }
            else
            {
                db.Users.Add(users);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = users.UserName + ", Succssfully Registered the Account";
                return messages;
            }
        }

        public Messages DeleteUser(int userId)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var user = UserById(userId);
            if (user == null)
            {
                messages.Message = "User Id is not found";
                return messages;
            }
            else
            {
                user.IsActive = false;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "User is succssfully deleted";
                return messages;
            }
        }
        public Role GetRoleById(int id)
        {
            return db.Roles.Find(id);
        }

        public List<Role> GetAllRole()
        {
            return db.Roles.ToList();
        }

        public Messages UpdateUser(UserDTO userDTO)
        {
            Messages messages = new Messages();
            messages.Success = false;
            messages.Role = userDTO.Role;
            User users = new User();
            users.UserName = userDTO.UserName;
            users.PhoneNumber = userDTO.PhoneNumber;
            users.Location = userDTO.Location;
            users.EmailId = userDTO.EmailId;
            users.Password = userDTO.Password;
            users.RoleId = userDTO.RoleId;
            var userExist = UserById(userDTO.UserId);
            var phoneExist = db.Users.FirstOrDefault(x => x.PhoneNumber == userDTO.PhoneNumber);
            var emailIdExist = db.Users.FirstOrDefault(x => x.EmailId == userDTO.EmailId);
            if (userExist == null)
            {
                messages.Message = "User Id is not found";
                return messages;
            }
            else if (phoneExist != null && phoneExist.UserId != userExist.UserId)
            {
                messages.Message = "Phone Number is already registered";
                return messages;
            }
            else if (emailIdExist != null && emailIdExist.UserId != userExist.UserId)
            {
                messages.Message = "EmailId is already registered";
                return messages;
            }
            else
            {
                userExist.UserName = userDTO.UserName;
                userExist.PhoneNumber = userDTO.PhoneNumber;
                userExist.EmailId = userDTO.EmailId;
                userExist.Password = userDTO.Password;
                userExist.Location = userDTO.Location;
                userExist.RoleId = userDTO.RoleId;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "User is succssfully updated";
                return messages;
            }
        }
        public LoginResultDTO GetLoginDetail(string emailId, string password)
        {
            var users = (from user in db.Users
                         join role in db.Roles on user.RoleId equals role.RoleId
                         where user.EmailId == emailId && user.Password == password
                         select new LoginResultDTO()
                         {
                             UserId = user.UserId,
                             PhoneNumber = user.PhoneNumber,
                             RoleName = role.RoleName,
                             EmailId = user.EmailId,
                             Name = user.UserName
                         }).FirstOrDefault();
            return users;
        }

        public IEnumerable<UserDTO> UsersList()
        {
            var user = (from users in db.Users
                        join role in db.Roles on users.RoleId equals role.RoleId
                        //where role.RoleId == customers.RoleId
                        select new UserDTO()
                        {
                            UserId = users.UserId,
                            UserName = users.UserName,
                            PhoneNumber = users.PhoneNumber,
                            Location = users.Location,
                            EmailId = users.EmailId,
                            RoleName = role.RoleName
                        }).ToList();
            return user;
        }

    }
}
