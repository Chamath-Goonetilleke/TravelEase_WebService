/*
------------------------------------------------------------------------------
 File: TravelerController.cs
 Purpose: This file contains the TravelerController class, which is a controller
 for handling traveler-related operations in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

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
        private readonly ITravelerService _travelerService;
        private readonly ITravelerAccountRequestService _requestService;

        public TravelerController(ITravelerService travelerService, ITravelerAccountRequestService requestService)
        {
            _travelerService = travelerService;
            _requestService = requestService;
        }

        //------------------------------------------------------------------------------
        // Method: TravelerAuth
        // Purpose: Authenticates a traveler.
        //------------------------------------------------------------------------------
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

        //------------------------------------------------------------------------------
        // Method: CreateNewTraveler
        // Purpose: Creates a new traveler.
        //------------------------------------------------------------------------------
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

                return Ok(new Message() { Res = "Created Successfully" });
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        //------------------------------------------------------------------------------
        // Method: GetAllTravelers
        // Purpose: Gets a list of all travelers.
        //------------------------------------------------------------------------------
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

        //------------------------------------------------------------------------------
        // Method: GetTravelerByNIC
        // Purpose: Gets a traveler by NIC (National Identity Card) number.
        //------------------------------------------------------------------------------
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

        //------------------------------------------------------------------------------
        // Method: UpdateTraveler
        // Purpose: Updates traveler information.
        //------------------------------------------------------------------------------
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

        //------------------------------------------------------------------------------
        // Method: ActivateTraveler
        // Purpose: Activates a traveler's account.
        //------------------------------------------------------------------------------
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

        //------------------------------------------------------------------------------
        // Method: DeactivateTraveler
        // Purpose: Deactivates a traveler's account.
        //------------------------------------------------------------------------------
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

        //------------------------------------------------------------------------------
        // Method: DeleteTraveler
        // Purpose: Deletes a traveler's account.
        //------------------------------------------------------------------------------
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
