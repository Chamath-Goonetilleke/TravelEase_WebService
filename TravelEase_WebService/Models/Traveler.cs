using System;
namespace TravelEase_WebService.Models
{
	public class Traveler:User
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

