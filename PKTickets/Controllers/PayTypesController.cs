using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayTypesController : ControllerBase
    {

        private readonly IPayTypeRepository payTypeRepository;

        public PayTypesController(IPayTypeRepository _payTypeRepository)
        {
            payTypeRepository = _payTypeRepository;
        }


        [HttpGet("GetPayTypesList")]
        public IActionResult GetPayTypesList()
        {
            var payTypes = payTypeRepository.GetAllPayTypes();
            if (payTypes.Count() == 0)
            {
                throw new Exception("The PayType list is empty");
            }
            return Ok(payTypes);
        }

        [HttpGet("PayTypeById/{payTypeId}")]

        public ActionResult PayTypeById(int payTypeId)
        {
            var payType = payTypeRepository.PayTypeById(payTypeId);
            if (payType == null)
            {
                throw new Exception("The PayType Id is not found");
            }
            return Ok(payType);
        }


        [HttpGet("PayTypeByName/{name}")]

        public ActionResult PayTypeByName(string name)
        {
            var payType = payTypeRepository.PayTypeByName(name);
            if (payType == null)
            {
                throw new Exception("The PayType Name is not found");
            }
            return Ok(payType);
        }

        [HttpPost("AddPayType")]

        public IActionResult AddPayType(PayType payType)
        {
            return Ok(payTypeRepository.CreatePayType(payType));
        }


        [HttpPut("UpdatePayType")]
        public ActionResult UpdatePayType(PayType payType)
        {
            return Ok(payTypeRepository.UpdatePayType(payType));
        }


        [HttpPut("RemovePayType/{id}")]

        public IActionResult RemovePayType(int id)
        {
            return Ok(payTypeRepository.DeletePayType(id));
        }
    }
}
