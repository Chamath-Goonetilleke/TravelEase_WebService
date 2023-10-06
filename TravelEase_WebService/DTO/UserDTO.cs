using System;
namespace TravelEase_WebService.DTO
{
	public class UserDTO
	{
        public required string Role { get; set; }
        public required string Title { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Nic { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public string? EmployeeId { get; set; }
        public string? TravelAgentId { get; set; }

    }
}

