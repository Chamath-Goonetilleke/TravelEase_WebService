using System;
using System.Data;

namespace TravelEase_WebService.DTO
{
    public class CurrentUserDTO
    {
        public string? Role { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nic { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmployeeId { get; set; }
        public string? TravelAgentId { get; set; }
        public string? ImageUrl { get; set; }
        public string? City { get; set; }


        public void MapUser(Models.User user, string role, string? workId = null, string? city = null)
        {
            Role = user.Role;
            Title = user.Title;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Nic = user.Nic;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            ImageUrl = user.ImageUrl;


            if (role == "TravelAgent")
            {
                TravelAgentId = workId;
            }
            else if (role == "BackOfficeUser")
            {
                EmployeeId = workId;
            }
            else if (role == "Traveler")
            {
                City = city;
            }





        }

    }


}

