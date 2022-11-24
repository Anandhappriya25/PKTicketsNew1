﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models.DTO;
using PKTickets.Models;
using PKTickets.Repository;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimesController : ControllerBase
    {
        private readonly IShowTimeRepository showTimeRepository;

        public ShowTimesController(IShowTimeRepository _showTimeRepository)
        {
            showTimeRepository = _showTimeRepository;
        }

        [HttpGet("")]
        public IActionResult List()
        {
            return Ok(showTimeRepository.GetAllShowTimes());
        }

        [HttpGet("{showTimeId}")]

        public ActionResult GetById(int showTimeId)
        {
            var showTime = showTimeRepository.ShowTimeasStringById(showTimeId);
            if (showTime == null)
            {
                return NotFound();
            }
            return Ok(showTime);
        }


        [HttpPost("")]
        public IActionResult Add(ShowTimeDTO showTime)
        {
            var result = showTimeRepository.CreateShowTime(showTime);
            if (result.Success == false)
            {
                return Conflict(result.Message);
            }
            return Created("https://localhost:7221/api/ShowTimes/" + showTime.ShowTimeId + "", result.Message);
        }


        [HttpPut("")]
        public ActionResult Update(ShowTimeDTO showTime)
        {
            if (showTime.ShowTimeId == 0)
            {
                return BadRequest("Enter the ShowTime Id field");
            }
            var result = showTimeRepository.UpdateShowTime(showTime);
            if (result.Message == "ShowTime Id is not found")
            {
                return NotFound(result.Message);
            }
            else if (result.Success == false)
            {
                return Conflict(result.Message);
            }
            return Ok(result.Message);
        }


        [HttpDelete("{showTimeId}")]
        public IActionResult Remove(int showTimeId)
        {
            var result = showTimeRepository.DeleteShowTime(showTimeId);
            if (result.Success == false)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Message);
        }

    }
}
