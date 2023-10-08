using System;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
    public interface ITrainScheduleService
    {
        Task<TrainSchedule> InsertTrainSchedule(TrainSchedule train);
    }
}

