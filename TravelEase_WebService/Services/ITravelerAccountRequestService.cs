/*
------------------------------------------------------------------------------
 File: ITravelerAccountRequestService.cs
 Purpose: This file defines the ITravelerAccountRequestService interface, which
 contains methods for handling traveler account requests in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
    public interface ITravelerAccountRequestService
	{
        Task CreateNewRequest(TravelerAccountRequest request);
        Task DeleteRequest(string nic);
        Task<List<TravelerAccountRequest>> GetAllRequests();

    }
}

