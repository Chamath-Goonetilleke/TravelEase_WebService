using System;
namespace TravelEase_WebService.DTO
{
	public class AuthUserDTO
	{
        public required string Role { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

    }
}

