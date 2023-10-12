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
        public async Task<ActionResult<TrainSchedule>> PostTrain(TrainSchedule train)
        {
            var insertedTrainSchedule = await _iTrainScheduler.InsertTrainSchedule(train);
            return insertedTrainSchedule;
        }

        
        [Route("schedules")]
        [HttpGet]
        public async Task<List<TrainSchedule>> GetAllTrainSchedule()
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
        public async Task<TrainSchedule> GetTrain(int trainId)
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
        public async Task UpdateSchedule(string id, TrainSchedule schedule)
        {
            await _iTrainScheduler.UpdateTrainSchedule(id, schedule);
        }

        [Route("update-schedule-status/{id}/{isPublished}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateScheduleStatus(string id, bool isPublished)
        {
            // Log a message to the console.
            Console.WriteLine($"Getting train with ID");

            // Get the train from the database.
            var updated = await _iTrainScheduler.UpdateScheduleStatus(id, isPublished);

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
        public async Task<IActionResult> UpdateReservationBySchedule(string id, bool IsCancled)
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
        public async Task<IActionResult> UpdateTrainStatus(string id, bool status)
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
        public async Task<IActionResult> UpdateschedulesStatus(string id, bool IsPublished)
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
        public async Task<IActionResult> UpdateschedulesIsCancledStatus(string id, bool IsCancled)
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

