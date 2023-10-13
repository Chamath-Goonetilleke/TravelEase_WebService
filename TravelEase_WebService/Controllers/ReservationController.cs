using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravelEase_WebService.DTO;
using TravelEase_WebService.Models;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{
    [ApiController]
    [Route("/api/v1/reservation")]
    public class ReservationController: ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
		{
			_reservationService = reservationService;
		}

        [Route("getReservations")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetReservations()
        {
            try
            {
                var s = await _reservationService.GetTrainsSchedules();
                return Ok(s);
            }
            catch(Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        [Route("addReservation")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateReservation(Reservation reservation)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                var userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserRole")?.Value;
                if(userRole == "TravelAgent")
                {
                    reservation.TravelAgentId = userId;
                    await _reservationService.CreateNewReservation(reservation);
                    return Ok("Successfully Add Reservation");
                }
                throw new Exception("Travel Agent or Traveler allowed to add reservations.");
                
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        [Route("reservationByTraveler/{nic}")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetReservationByTravelerId(string nic)
        {
            try
            {
                var reservationList = await _reservationService.GetReservationByTravelerId(nic);
                return Ok(reservationList);
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        [Route("reservationByTravelAgent/{agentId}")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetReservationByTravelAgent(string agentId)
        {
            try
            {
                var reservationList = await _reservationService.GetReservationByTravelAgent(agentId);
                return Ok(reservationList);
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        [Route("reservationHistory/{nic}")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetReservationHistory(string nic)
        {
            try
            {
                var reservationList = await _reservationService.GetReservationHistory(nic);
                return Ok(reservationList);
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        [Route("updateReservation")]
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateOrCancelReservation(ReservationUpdateDTO updateDTO)
        {
            try
            {
                 await _reservationService.UpdateReservation(updateDTO);
                if (updateDTO.IsCancel == true)
                {
                    return Ok("Reservation Cancelled");
                }
                return Ok("Reservation Updated");
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }
    }
 
}

