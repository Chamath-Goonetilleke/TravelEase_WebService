/*
------------------------------------------------------------------------------
 File: Traveler.cs
 Purpose: This file contains the Traveler class, which inherits from the User class
 and represents a traveler in the TravelEase_WebService project. It also defines
 the TravelerAccountState enumeration.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

namespace TravelEase_WebService.Models
{
    public class Traveler : User
    {
        public enum TravelerAccountState
        {
            ACTIVE,
            INACTIVE
        }

        public required string City { get; set; }
        public required TravelerAccountState State { get; set; }
    }
}
