/*
------------------------------------------------------------------------------
 File: TravelerDTO.cs
 Purpose: This file contains the TravelerDTO class, which defines a data transfer
 object for representing traveler details in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

using TravelEase_WebService.Models;
using static TravelEase_WebService.Models.Traveler;

namespace TravelEase_WebService.DTO
{
    public class TravelerDTO
    {
        public string? Id { get; set; }

        public string? Role { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nic { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ImageUrl { get; set; }
        public string? City { get; set; }
        public TravelerAccountState State { get; set; }
        public string? Token { get; set; }

        public void MapTraveler(Traveler traveler, String token)
        {
            Role = traveler.Role;
            Title = traveler.Title;
            FirstName = traveler.FirstName;
            LastName = traveler.LastName;
            Nic = traveler.Nic;
            Email = traveler.Email;
            PhoneNumber = traveler.PhoneNumber;
            ImageUrl = traveler.ImageUrl;
            City = traveler.City;
            State = traveler.State;
            Token = token;
        }
    }
}
