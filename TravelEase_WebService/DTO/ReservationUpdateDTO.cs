/*
------------------------------------------------------------------------------
 File: ReservationUpdateDTO.cs
 Purpose: This file contains the ReservationUpdateDTO class, which defines a data transfer
 object for updating reservation details in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

namespace TravelEase_WebService.DTO
{
    public class ReservationUpdateDTO
    {
        public string? Id { get; set; }
        public string? Date { get; set; }
        public bool? IsCancel { get; set; }
    }
}
