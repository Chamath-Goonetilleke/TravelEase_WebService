using System;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
    public interface ITrainScheduleService
    {
        Task<List<TrainSchedule>> GetAllTrainSchedule();
        Task<TrainSchedule> GetTrainsScheduleById(int trainId);
        Task<TrainSchedule> InsertTrainSchedule(TrainSchedule train);
        Task<bool> UpdateReservationsBySchedule(string id, bool IsCancled);
        Task<bool> UpdateschedulesStatus(string id, bool isPublished);
        Task<bool> UpdateScheduleStatus(string id, bool isPublished);
        Task<bool> UpdatesOurchedulesIsCancledStatus(string id, bool isCancled);
        Task<bool> UpdatesOurchedulesStatus(string id, bool isPublished);
        Task UpdateTrainSchedule(string id, TrainSchedule schedule);
        Task UpdatetrainStatus(int trainId);
        Task<bool> UpdateTrainStatus(string id, bool status);
    }
}

