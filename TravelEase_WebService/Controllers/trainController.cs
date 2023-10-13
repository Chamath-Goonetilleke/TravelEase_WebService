/*
 * TrainController
 * 
 * Description:
 * This class defines the API endpoints related to train operations for the TravelEase_WebService project. It includes methods for creating a new train, retrieving a train by ID, and fetching all trains.
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
 * - This controller handles incoming HTTP requests related to train operations.
 * - Ensure that the necessary dependencies (MongoDB driver, services, etc.) are injected and configured properly in your project.
 * - Customize the routes and endpoint methods as per the project requirements.
 * 
 * Endpoints:
 * - POST /api/v1/train/new-train: Creates a new train. Expects a JSON object representing the train in the request body.
 * - GET /api/v1/train/trains/{id}: Retrieves a specific train by its ID.
 * - GET /api/v1/train/trains: Retrieves a list of all trains.
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
        public async Task<ActionResult<Trains>> PostTrain(Trains train)/// Inserts a new train into the database.
        {
            try
            {
                var insertedTrain = await _trainService.InsertTrain(train);
                return Ok(insertedTrain); // HTTP 200 OK response with the inserted train object
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error occurred: {ex.Message}");

                // You can customize the error response based on the exception type or message
                return StatusCode(500, "Internal Server Error"); // HTTP 500 Internal Server Error response
            }



        }
        [Route("trains/{id}")]
        [HttpGet]
        public async Task<ActionResult<Trains>> GetTrain(string id)/// Retrieves a specific train by its unique ID.
        {
            try
            {
                var train = await _trainService.GetTrainsById(id);

                if (train != null)
                {
                    return Ok(train); // HTTP 200 OK response with the train object
                }
                else
                {
                    return NotFound($"Train with ID {id} not found"); // HTTP 404 Not Found response
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error occurred: {ex.Message}");

                return StatusCode(500, "Internal Server Error"); // HTTP 500 Internal Server Error response
            }
        }
        [Route("trains")]
        [HttpGet]
        public async Task<ActionResult<List<Trains>>> GetAllTrains() /// Retrieves a list of all trains from the database.
        {
            try
            {
                // Log a message to the console.
                Console.WriteLine($"Getting all trains.");

                // Get the trains from the database.
                var trains = await _trainService.GetAllTrains();

                if (trains != null && trains.Count > 0)
                {
                    return Ok(trains); // HTTP 200 OK response with the list of trains
                }
                else
                {
                    return NotFound("No trains found"); // HTTP 404 Not Found response with an appropriate error message
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error occurred: {ex.Message}");

                return StatusCode(500, "Internal Server Error"); // HTTP 500 Internal Server Error response with a generic error message
            }
        }
    }
}

