using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TravelEase_WebService.Data;
using TravelEase_WebService.Models;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{
    [Route("api/v1/train")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ITrainService _trainService;

        public TrainController(ITrainService trainService)
        {
            _trainService = trainService;
        }

        [Route("new-train")]
        [HttpPost]
        public async Task<ActionResult<Trains>> PostTrain(Trains train)
        {
            var insertedTrain = await _trainService.InsertTrain(train);
            return insertedTrain;



        }
        [Route("trains/{id}")]
        [HttpGet]
        public async Task<Trains> GetTrain(string id)
        {
            var train = await _trainService.GetTrainsById(id);

            // Return the train.
            return train;
        }
        [Route("trains")]
        [HttpGet]
        public async Task<List<Trains>> GetAllTrains()
        {
            // Log a message to the console.
            Console.WriteLine($"Getting train with ID");

            // Get the train from the database.
            var train = await _trainService.GetAllTrains();

            // Return the train.
            return train;
        }
    }
}

