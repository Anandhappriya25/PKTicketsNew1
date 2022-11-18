using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }


        [HttpGet("GetUserList")]
        public IActionResult GetUserList()
        {
            var user = userRepository.GetAllUsers();
            if (user.Count() == 0)
            {
                throw new Exception("The User list is empty");
            }
            return Ok(user);
        }

        [HttpGet("UserById/{userId}")]

        public ActionResult UserById(int userId)
        {
            var user = userRepository.UserById(userId);
            if (user == null)
            {
                throw new Exception("The User Id is not found");
            }
            return Ok(user);
        }


        [HttpPost("CreateUser")]

        public IActionResult CreateUser(UserDTO user)
        {
            return Ok(userRepository.CreateUser(user));
        }


        [HttpPut("UpdateUser")]
        public ActionResult UpdateUser(UserDTO user)
        {
            return Ok(userRepository.UpdateUser(user));
        }


        [HttpPut("DeleteUser/{userId}")]

        public IActionResult DeleteUser(int userId)
        {
            return Ok(userRepository.DeleteUser(userId));
        }
    }
}
