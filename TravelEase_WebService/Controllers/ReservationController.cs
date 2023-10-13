/*
------------------------------------------------------------------------------
 File: ReservationController.cs
 Purpose: This file contains the ReservationController class, which is a controller
 for handling reservation-related operations in the TravelEase_WebService project.
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
    [Route("/api/v1/reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        //------------------------------------------------------------------------------
        // Method: GetReservations
        // Purpose: Gets a list of reservations.
        //------------------------------------------------------------------------------
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
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        //------------------------------------------------------------------------------
        // Method: CreateReservation
        // Purpose: Creates a new reservation.
        //------------------------------------------------------------------------------
        [Route("addReservation")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateReservation(Reservation reservation)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                var userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserRole")?.Value;
                if (userRole == "TravelAgent")
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

        //------------------------------------------------------------------------------
        // Method: GetReservationByTravelerNIC
        // Purpose: Gets reservations by traveler NIC.
        //------------------------------------------------------------------------------
        [Route("reservationByTraveler/{nic}")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetReservationByTravelerNIC(string nic)
        {
            try
            {
                var reservationList = await _reservationService.GetReservationByTravelerNIC(nic);
                return Ok(reservationList);
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        //------------------------------------------------------------------------------
        // Method: GetReservationByTravelAgent
        // Purpose: Gets reservations by travel agent ID.
        //------------------------------------------------------------------------------
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

        //------------------------------------------------------------------------------
        // Method: GetReservationHistory
        // Purpose: Gets reservation history for a traveler.
        //------------------------------------------------------------------------------
        [Route("reservationHistory/{nic}")]
        [Authorize]
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

        //------------------------------------------------------------------------------
        // Method: UpdateOrCancelReservation
        // Purpose: Updates or cancels a reservation.
        //------------------------------------------------------------------------------
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
