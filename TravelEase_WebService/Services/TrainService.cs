/*
 * TrainService
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
 */using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TravelEase_WebService.Data;
using TravelEase_WebService.Models;
namespace TravelEase_WebService.Services
{
    
    /// Service class implementing ITrainService for managing train data in the database.
    public class TrainService : ITrainService
    {
        private readonly IMongoCollection<Trains> _trainCollection;


        /// Constructor for TrainService class.
        /// <param name="dbSetting">Database settings injected via dependency injection.</param>
        public TrainService(IOptions<DatabaseSettings> dbSetting)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);

            _trainCollection = mongoDatabase.GetCollection<Trains>(dbSetting.Value.TrainCollectionName);
        }

        /// Inserts a new train into the database.
        /// <param name="train">The Trains object to be inserted.</param>
        /// <returns>The inserted Trains object.</returns>
        public async Task<Trains> InsertTrain(Trains train)
        {
            Console.WriteLine($">>>>>>>>>>>>>>train name: {train}");
            await _trainCollection.InsertOneAsync(train);
            return train;
        }


        /// Retrieves a list of all trains from the database.
        /// <returns>A list of Trains objects representing all trains.</returns>
        public async Task<List<Trains>> GetAllTrains()
        {
            return await _trainCollection.Find(_ => true).ToListAsync();
        }


        /// Retrieves a specific train by its unique ID.
        /// <param name="id">The ID of the train to retrieve.</param>
        /// <returns>A Trains object representing the specified train.</returns>
        public async Task<Trains> GetTrainsById(string id)
        {
            return await _trainCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        /// Updates the status of a specific train in the database.
        /// <param name="id">The ID of the train to update.</param>
        /// <param name="status">The new status of the train (true for active, false for inactive).</param>
        /// <returns>Boolean indicating whether the update was successful.</returns>
        public async Task<bool> UpdateTrainStatus(string id, bool status)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<Trains>.Filter.Eq("_id", objectId);
            var update = Builders<Trains>.Update.Set("Status", status);

            var updateResult = await _trainCollection.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }
    }
}

