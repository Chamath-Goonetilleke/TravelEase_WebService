using System;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
	public interface ITrainService
	{
        Task<Trains> InsertTrain(Trains train);

    }
}

