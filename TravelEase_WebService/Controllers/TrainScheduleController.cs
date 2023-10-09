using System;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.Models;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{
    [Route("api/v1/schedule")]
    [ApiController]
    public class TrainScheduleController
	{
        private readonly ITrainScheduleService _iTrainScheduler;
        public TrainScheduleController(ITrainScheduleService iTrainScheduler)
		{
            _iTrainScheduler = iTrainScheduler;

        }
        [HttpPost("/new-schedule")]
        public async Task<ActionResult<TrainSchedule>> PostTrain(TrainSchedule train)
        {
            var insertedTrainSchedule = await _iTrainScheduler.InsertTrainSchedule(train);
            return CreatedAtAction("GetTrain", new { id = insertedTrainSchedule.Id });
        }

        private ActionResult<TrainSchedule> CreatedAtAction(string v, object value)
        {
            throw new NotImplementedException();
        }
        [HttpGet("/schedules")]
        public async Task<List<TrainSchedule>> GetAllTrainSchedule()
        {
            // Log a message to the console.
            Console.WriteLine($"Getting train with ID");

            // Get the train from the database.
            var train = await _iTrainScheduler.GetAllTrainSchedule();

            // Return the train.
            return train;
        }

    }
}

