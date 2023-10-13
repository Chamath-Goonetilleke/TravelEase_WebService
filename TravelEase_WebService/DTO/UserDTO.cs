/*
------------------------------------------------------------------------------
 File: UserDTO.cs
 Purpose: This file contains the UserDTO class, which defines a data transfer
 object for representing user details in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

namespace TravelEase_WebService.DTO
{
    public class UserDTO
    {
        public string? Role { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nic { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ImageUrl { get; set; }
        public string? EmployeeId { get; set; }
        public string? TravelAgentId { get; set; }
        public string? City { get; set; }
    }
}
