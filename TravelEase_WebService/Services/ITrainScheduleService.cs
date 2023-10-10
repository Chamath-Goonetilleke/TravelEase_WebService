﻿using System;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
    public interface ITrainScheduleService
    {
        Task<List<TrainSchedule>> GetAllTrainSchedule();
        Task<TrainSchedule> GetTrainsScheduleById(int trainId);
        Task<TrainSchedule> InsertTrainSchedule(TrainSchedule train);
        Task UpdateTrainSchedule(string id, TrainSchedule schedule);
        Task UpdatetrainStatus(int trainId);
    }
}

