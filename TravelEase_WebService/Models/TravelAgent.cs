/*
------------------------------------------------------------------------------
 File: TravelAgent.cs
 Purpose: This file contains the TravelAgent class, which inherits from the User
 class and represents a travel agent in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

namespace TravelEase_WebService.Models
{
    public class TravelAgent : User
    {
        public required string TravelAgentId { get; set; }
    }
}
