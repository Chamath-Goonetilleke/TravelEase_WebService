/*
------------------------------------------------------------------------------
 File: ReservationDTO.cs
 Purpose: This file contains the ReservationDTO class, which defines a data transfer
 object for representing reservation details in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

using TravelEase_WebService.Models;

namespace TravelEase_WebService.DTO
{
    public class ReservationDTO
    {
        public required string TravelerId { get; set; }
        public required string ScheduleId { get; set; }
        public required string From { get; set; }
        public required string To { get; set; }
        public required string Departs { get; set; }
        public required string Arrives { get; set; }
        public required string Time { get; set; }
        public required string Date { get; set; }
        public required string TrainName { get; set; }
        public required string TrainNo { get; set; }
        public required string TrainClass { get; set; }
        public required string ClassPrice { get; set; }
        public required string TotalPrice { get; set; }
        public required List<Passenger> Passengers { get; set; }

        public required bool IsTravelrCreated { get; set; }
        public string? TravelAgentId { get; set; }
    }
}
