/*
------------------------------------------------------------------------------
 File: IReservationService.cs
 Purpose: This file defines the IReservationService interface, which contains
 methods for handling reservation-related operations in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/
using TravelEase_WebService.DTO;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
    public interface IReservationService
	{
        Task<List<ScheduleCheckerDTO>> GetTrainsSchedules();
        Task CreateNewReservation(Reservation reservation);
        Task<List<Reservation>> GetReservationByTravelerNIC(string nic);
        Task<List<Reservation>> GetReservationHistory(string nic);
        Task<List<Reservation>> GetReservationByTravelAgent(string id);
        Task UpdateReservation(ReservationUpdateDTO updateDTO);
    }
}

