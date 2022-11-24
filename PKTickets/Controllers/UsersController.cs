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


        [HttpGet("")]
        public IActionResult List()
        {
            return Ok(userRepository.GetAllUsers());
        }

        [HttpGet("{userId}")]

        public ActionResult GetById(int userId)
        {
            var user = userRepository.UserById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPost("")]

        public IActionResult Add(User user)
        {
            var result=userRepository.CreateUser(user);
            if(result.Success==false)
            {
                return BadRequest(result.Message);
            }
            return Created("https://localhost:7221/api/users/YourId", result.Message);
        }


        [HttpPut("")]
        public ActionResult Update(User user)
        {
            var result = userRepository.UpdateUser(user);
            if (result.Success == false)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }


        [HttpDelete("{userId}")]

        public IActionResult Remove(int userId)
        {
            var result = userRepository.DeleteUser(userId);
            if (result.Success == false)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
