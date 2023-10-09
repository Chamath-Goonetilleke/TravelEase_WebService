using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{
    [ApiController]
    [Route("/api/v1/accountRequest")]
    public class TravelerAccountRequestController : ControllerBase
    {

        private readonly TravelerAccountRequestService _requestService;

        public TravelerAccountRequestController(TravelerAccountRequestService requestService)
        {
            _requestService = requestService;
        }

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

