using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.DTO;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{
    [ApiController]
    [Route("/api/v1/traveler")]
    public class TravelerController:ControllerBase
	{
        private readonly TravelerService _travelerService;

        public TravelerController(TravelerService travelerService)
        {
            _travelerService = travelerService;
        }

        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> CreateNewTraveler(UserDTO userDTO)
        {
            try
            {
                await _travelerService.CreateNewTraveler(userDTO);
                return Ok("Successfully Created.");
            }
            catch(Exception e)
            {
                return BadRequest("Error : "+ e.Message);
            }
        }

        [Route("getAllTravelers")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAllTravelers()
        {
            try
            {
                var travelrsList = await _travelerService.GetAllTravelers();
                return Ok(travelrsList);
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }
		
	}
}

