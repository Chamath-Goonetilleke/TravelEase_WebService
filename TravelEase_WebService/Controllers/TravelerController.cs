using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.DTO;
using TravelEase_WebService.Models;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{
    [ApiController]
    [Route("/api/v1/traveler")]
    public class TravelerController : ControllerBase
    {
        private readonly TravelerService _travelerService;
        private readonly TravelerAccountRequestService _requestService;

        public TravelerController(TravelerService travelerService, TravelerAccountRequestService requestService)
        {
            _travelerService = travelerService;
            _requestService = requestService;
        }

        [Route("auth")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> TravelerAuth(AuthUserDTO authUserDTO)
        {
            try
            {
                var traveler = await _travelerService.TravelerAuth(authUserDTO);
                return Ok(traveler);
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }


        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> CreateNewTraveler(UserDTO userDTO)
        {
            try
            {
                await _travelerService.CreateNewTraveler(userDTO);

                var request = new TravelerAccountRequest()
                {
                    TravelerNIC = userDTO.Nic,
                    RequestType = 0,
                    Time = DateTime.Now
                };

                await _requestService.CreateNewRequest(request);



                return Ok(new Message() { Res="Created Successfully"});
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
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

        [Route("getTraveler/{nic}")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetTravelerByNIC(string nic)
        {
            try
            {
                var travelr = await _travelerService.GetTravelerByNIC(nic);
                return Ok(travelr);
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }


        [Route("update")]
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateTraveler(UserDTO userDTO)
        {
            try
            {
                await _travelerService.UpdateTraveler(userDTO);
                return Ok("Successfully Updated");
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        [Route("activateAccount/{nic}")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ActivateTraveler(string nic)
        {
            try
            {
                await _travelerService.ActivateTravelerAccount(nic);
                await _requestService.DeleteRequest(nic);
                return Ok("Successfully Activated");
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        [Route("deactivateAccount/{nic}")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> DeactivateTraveler(string nic)
        {
            try
            {
                await _travelerService.DeactivateTravelerAccount(nic);
                return Ok("Successfully Deactivated");
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }


        [Route("delete/{nic}")]
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteTraveler(string nic)
        {
            try
            {
                await _travelerService.DeleteTraveler(nic);
                return Ok("Successfully Deleted");
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }
    }
}

