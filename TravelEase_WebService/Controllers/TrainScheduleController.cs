using System;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.Models;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{
    [Route("api/schedule")]
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

    }
}

