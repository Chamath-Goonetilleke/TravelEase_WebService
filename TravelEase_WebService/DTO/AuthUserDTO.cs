/*
------------------------------------------------------------------------------
 File: AuthUserDTO.cs
 Purpose: This file contains the AuthUserDTO class, which defines the
 data transfer object for user authentication in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

namespace TravelEase_WebService.DTO
{
    public class AuthUserDTO
    {
        public string? Role { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
