using System;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
	public interface ITrainService
	{
        Task<List<Trains>> GetAllTrains();
        Task<Trains> GetTrainsById(string id);
        Task<Trains> InsertTrain(Trains train);
        Task<bool> UpdateTrainStatus(string id, bool status);
    }
}

