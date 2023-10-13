/*
------------------------------------------------------------------------------
 File: TravelerAccountRequestController.cs
 Purpose: This file contains the TravelerAccountRequestController class, which is a controller
 for handling traveler account request-related operations in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{
    [ApiController]
    [Route("/api/v1/accountRequest")]
    public class TravelerAccountRequestController : ControllerBase
    {
        private readonly ITravelerAccountRequestService _requestService;

        public TravelerAccountRequestController(ITravelerAccountRequestService requestService)
        {
            _requestService = requestService;
        }

        //------------------------------------------------------------------------------
        // Method: GetAllRequests
        // Purpose: Gets a list of all traveler account requests.
        //------------------------------------------------------------------------------
        [Route("getAllRequests")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAllRequests()
        {
            try
            {
                var reqList = await _requestService.GetAllRequests();
                return Ok(reqList);
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }
    }
}
