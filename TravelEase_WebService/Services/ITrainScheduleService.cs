
/*
 * ITrainScheduleService Interface
 * 
 * Description:
 * This interface defines the contract for managing train schedules in the TravelEase_WebService project. It provides methods for retrieving, inserting, and updating train schedules, as well as managing various status fields related to train schedules.
 * 
 * Author:
 * Madushan S H K
 * IT20122614
 * SLIIT
 * it20122614@my.sliit.lk
 * 
 * Date:
 * 06/10/2023
 * 
 * Methods:
 * - Task<List<TrainSchedule>> GetAllTrainSchedule(): Retrieves a list of all train schedules.
 * - Task<TrainSchedule> GetTrainsScheduleById(int trainId): Retrieves a specific train schedule by its ID.
 * - Task<TrainSchedule> InsertTrainSchedule(TrainSchedule train): Inserts a new train schedule into the database.
 * - Task<bool> UpdateReservationsBySchedule(string id, bool IsCancled): Updates reservation cancellation status for a specific train schedule.
 * - Task<bool> UpdateschedulesStatus(string id, bool isPublished): Updates published status for a specific train schedule.
 * - Task<bool> UpdateScheduleStatus(string id, bool isPublished): Updates published status for a specific train schedule by its ID.
 * - Task<bool> UpdatesOurchedulesIsCancledStatus(string id, bool isCancled): Updates cancellation status for multiple train schedules.
 * - Task<bool> UpdatesOurchedulesStatus(string id, bool isPublished): Updates published status for multiple train schedules.
 * - Task UpdateTrainSchedule(string id, TrainSchedule schedule): Updates an existing train schedule by its ID.
 * - Task UpdatetrainStatus(int trainId): Updates overall status for a specific train schedule.
 * - Task<bool> UpdateTrainStatus(string id, bool status): Updates overall status for a specific train schedule by its ID.
 * 
 * Note:
 * - Ensure that proper error handling and input validation are implemented in the methods.
 * - Include appropriate documentation for each method, specifying input parameters, return values, and any potential exceptions.
 */

using System;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
    public interface ITrainScheduleService
    {
        Task<List<TrainSchedule>> GetAllTrainSchedule(); /// Retrieves a list of all train schedules.
        Task<TrainSchedule> GetTrainsScheduleById(int trainId);/// Retrieves a specific train schedule by its ID.
        Task<TrainSchedule> InsertTrainSchedule(TrainSchedule train);/// Inserts a new train schedule into the database.
        Task<bool> UpdateReservationsBySchedule(string id, bool IsCancled);/// Updates reservation cancellation status for a specific train schedule.
        Task<bool> UpdateschedulesStatus(string id, bool isPublished);/// Updates published status for a specific train schedule.
        Task<bool> UpdateScheduleStatus(string id, bool isPublished);/// Updates published status for a specific train schedule by its ID.
        Task<bool> UpdatesOurchedulesIsCancledStatus(string id, bool isCancled);/// Updates cancellation status for multiple train schedules.
        Task<bool> UpdatesOurchedulesStatus(string id, bool isPublished);/// Updates published status for multiple train schedules.
        Task UpdateTrainSchedule(string id, TrainSchedule schedule);/// Updates an existing train schedule by its ID.
        Task UpdatetrainStatus(int trainId);/// Updates overall status for a specific train schedule.
        Task<bool> UpdateTrainStatus(string id, bool status);/// Updates overall status for a specific train schedule by its ID.
    }
}

