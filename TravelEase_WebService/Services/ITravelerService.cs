/*
------------------------------------------------------------------------------
 File: ITravelerService.cs
 Purpose: This file defines the ITravelerService interface, which contains
 methods for handling traveler-related operations in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/
using TravelEase_WebService.DTO;

namespace TravelEase_WebService.Services
{
    public interface ITravelerService
	{
        Task<TravelerDTO> TravelerAuth(AuthUserDTO authUserDTO);
        Task CreateNewTraveler(UserDTO userDTO);
        Task<List<CurrentUserDTO>> GetAllTravelers();
        Task<CurrentUserDTO> GetTravelerByNIC(string nic);
        Task UpdateTraveler(UserDTO userDTO);
        Task ActivateTravelerAccount(string nic);
        Task DeactivateTravelerAccount(string nic);
        Task DeleteTraveler(string nic);
    }
}

