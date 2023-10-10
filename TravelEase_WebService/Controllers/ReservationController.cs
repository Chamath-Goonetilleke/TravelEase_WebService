using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                await _reservationService.CreateNewReservation(reservation);
                return Ok("Successfully Add Reservation");
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }
    }
}

