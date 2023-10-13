/*
 * TrainScheduleController
 * 
 * Description:
 * This class defines the API endpoints related to train schedules and status updates for the TravelEase_WebService project. It includes methods for creating a new train schedule, retrieving train schedules, updating various status fields related to train schedules, and more.
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
 * Usage:
 * - This controller handles incoming HTTP requests related to train schedules and status updates.
 * - Ensure that the necessary dependencies (MongoDB driver, services, etc.) are injected and configured properly in your project.
 * - Customize the routes and endpoint methods as per the project requirements.
 * 
 * Endpoints:
 * - POST /api/v1/schedule/new-schedule: Creates a new train schedule. Expects a JSON object representing the schedule in the request body.
 * - GET /api/v1/schedule/schedules: Retrieves a list of all train schedules.
 * - GET /api/v1/schedule/schedule/{trainId}: Retrieves a specific train schedule by its ID.
 * - PATCH /api/v1/schedule/update-schedule/{id}: Updates an existing train schedule by its ID. Expects a JSON object representing the updated schedule in the request body.
 * - PATCH /api/v1/schedule/update-schedule-status/{id}/{isPublished}: Updates the published status of a train schedule by its ID.
 * - PATCH /api/v1/schedule/update-reservation-status/{id}/{IsCancled}: Updates the reservation cancellation status of a train schedule by its ID.
 * - PATCH /api/v1/schedule/update-train-status/{id}/{status}: Updates the overall status of a train schedule by its ID.
 * - PATCH /api/v1/schedule/update-schedules-status/{id}/{IsPublished}: Updates the published status of multiple train schedules by a common ID.
 * - PATCH /api/v1/schedule/update-schedules-status-cancled/{id}/{IsCancled}: Updates the cancellation status of multiple train schedules by a common ID.
 * 
 * Dependencies:
 * - .NET Core
 * - Microsoft.AspNetCore.Mvc (included in Microsoft.AspNetCore.App package)
 * - MongoDB.Driver (install via NuGet Package Manager: Install-Package MongoDB.Driver)
 * - [List any other external libraries or packages required]
 * 
 * Note:
 * - Ensure that proper error handling and input validation are implemented in the endpoint methods.
 * - Include appropriate security measures, such as authentication and authorization, based on your project's requirements.
 */


using System;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.Models;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{
    [Route("api/v1/schedule")]
    [ApiController]
    public class TrainScheduleController:ControllerBase
	{
        private readonly ITrainScheduleService _iTrainScheduler;
        public TrainScheduleController(ITrainScheduleService iTrainScheduler)
		{
            _iTrainScheduler = iTrainScheduler;

        }

        [Route("new-schedule")]
        [HttpPost]
        public async Task<ActionResult<TrainSchedule>> PostTrain(TrainSchedule train)/// Inserts a new train schedule into the database.
        {
            var insertedTrainSchedule = await _iTrainScheduler.InsertTrainSchedule(train);
            return insertedTrainSchedule;
        }

        
        [Route("schedules")]
        [HttpGet]
        public async Task<List<TrainSchedule>> GetAllTrainSchedule()/// Retrieves a list of all train schedules.
        {
            // Log a message to the console.
            Console.WriteLine($"Getting train with ID");

            // Get the train from the database.
            var train = await _iTrainScheduler.GetAllTrainSchedule();

            // Return the train.
            return train;
        }
        [Route("schedule/{trainId}")]
        [HttpGet]
        public async Task<TrainSchedule> GetTrain(int trainId)/// Retrieves a specific train schedule by its ID.
        {
            var train = await _iTrainScheduler.GetTrainsScheduleById(trainId);

            // Return the train.
            return train;
        }

        [Route("train-status/{trainId}")]
        [HttpGet]
        public async Task<ActionResult> UpdatetrainStatus(int trainId)
        {
            await _iTrainScheduler.UpdatetrainStatus(trainId);

            // Return the train.
            return Ok("");
        }

        [Route("update-schedule/{id}")]
        [HttpPost]
        public async Task UpdateSchedule(string id, TrainSchedule schedule)/// Updates an existing train schedule by its ID.
        {
            await _iTrainScheduler.UpdateTrainSchedule(id, schedule);
        }

        [Route("update-schedule-status/{id}/{isPublished}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateScheduleStatus(string id, bool isPublished)/// Updates published status for multiple train schedules.
        {
            // Log a message to the console.
            Console.WriteLine($"Getting train with ID");

            // Get the train from the database.
            var updated = await _iTrainScheduler.UpdateScheduleStatus(id, isPublished);/// Updates published status for a specific train schedule.

            // Return the update status
            if (updated)
            {
                return Ok("Status updated successfully");
            }
            else
            {
                return NotFound("Train not found");
            }
        }

        [Route("update-resevation-status/{id}/{IsCancled}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateReservationBySchedule(string id, bool IsCancled)/// Updates reservation cancellation status for a specific train schedule.
        {
            // Log a message to the console.
            Console.WriteLine($"Getting train with ID");

            // Get the train from the database.
            var updated = await _iTrainScheduler.UpdateReservationsBySchedule(id, IsCancled);

            // Return the update status
            if (updated)
            {
                return Ok("Status updated successfully");
            }
            else
            {
                return NotFound("Train not found");
            }
        }
        [Route("update-train-status/{id}/{status}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateTrainStatus(string id, bool status)/// Updates overall status for a specific train schedule.
        {
            // Log a message to the console.
            Console.WriteLine($"Getting train with ID");

            // Get the train from the database.
            var updated = await _iTrainScheduler.UpdateTrainStatus(id, status);

            // Return the update status
            if (updated)
            {
                return Ok("Status updated successfully");
            }
            else
            {
                return NotFound("Train not found");
            }
        }

        [Route("update-schedules-status/{id}/{IsPublished}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateschedulesStatus(string id, bool IsPublished)/// Updates published status for a specific train schedule by its ID.
        {
            // Log a message to the console.
            Console.WriteLine($"Getting train with ID");

            // Get the train from the database.
            var updated = await _iTrainScheduler.UpdatesOurchedulesStatus(id, IsPublished);

            // Return the update status
            if (updated)
            {
                return Ok("Status updated successfully");
            }
            else
            {
                return NotFound("Train not found");
            }
        }
        [Route("update-schedules-status-cancled/{id}/{IsCancled}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateschedulesIsCancledStatus(string id, bool IsCancled)/// Updates cancellation status for multiple train schedules.
        {
            // Log a message to the console.
            Console.WriteLine($"Getting train with ID");

            // Get the train from the database.
            var updated = await _iTrainScheduler.UpdatesOurchedulesIsCancledStatus(id, IsCancled);

            // Return the update status
            if (updated)
            {
                return Ok("Status updated successfully");
            }
            else
            {
                return NotFound("Train not found");
            }
        }

    }
}

