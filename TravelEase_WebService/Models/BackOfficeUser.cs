/*
------------------------------------------------------------------------------
 File: BackOfficeUser.cs
 Purpose: This file contains the BackOfficeUser class, which inherits from the User
 class and represents a back office user in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

namespace TravelEase_WebService.Models
{
    public class BackOfficeUser : User
    {
        public required string EmployeeId { get; set; }
    }
}
