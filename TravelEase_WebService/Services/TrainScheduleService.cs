/*
 * TrainScheduleService
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
 */



using System;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TravelEase_WebService.Data;
using TravelEase_WebService.DTO;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
	public class TrainScheduleService: ITrainScheduleService
    {
        private readonly IMongoCollection<TrainSchedule> _trainCollection;
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<Trains> _trainNewCollection;
        /// Constructor for TrainScheduleService class.
        /// <param name="dbSetting">Database settings injected via dependency injection.</param>
        public TrainScheduleService(IOptions<DatabaseSettings> dbSetting)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);

            _trainCollection = mongoDatabase.GetCollection<TrainSchedule>(dbSetting.Value.TrainScheduleCollectionName);
            _reservationCollection = mongoDatabase.GetCollection<Reservation>(dbSetting.Value.ReservationCollection);
            _trainNewCollection = mongoDatabase.GetCollection<Trains>(dbSetting.Value.TrainCollectionName);
        }

        /// Retrieves a list of all train schedules from the database.
        /// <returns>A list of TrainSchedule objects representing all train schedules.</returns>
        public async Task<List<TrainSchedule>> GetAllTrainSchedule()
        {
            return await _trainCollection.Find(_ => true).ToListAsync();

        }
        /// Retrieves a specific train schedule by its unique ID.
        /// <param name="trainId">The ID of the train to retrieve the schedule for.</param>
        /// <returns>A TrainSchedule object representing the specified train schedule.</returns>
        async Task<TrainSchedule> ITrainScheduleService.GetTrainsScheduleById(int trainId)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(x => x.TrainNo, trainId);
            var trainSchedule = await _trainCollection.Find(filter).FirstOrDefaultAsync();
            return trainSchedule;
        }
        /// Inserts a new train schedule into the database.
        /// <param name="train">The TrainSchedule object to be inserted.</param>
        /// <returns>The inserted TrainSchedule object.</returns>
        async Task<TrainSchedule> ITrainScheduleService.InsertTrainSchedule(TrainSchedule train)
        {
            await _trainCollection.InsertOneAsync(train);
            return train;
        }
        /// Updates an existing train schedule in the database.
        /// <param name="id">The ID of the train schedule to update.</param>
        /// <param name="schedule">The updated TrainSchedule object.</param>
        public async Task UpdateTrainSchedule(string id, TrainSchedule schedule)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq("_id", new ObjectId(id));
            var update = Builders<TrainSchedule>.Update
                .Set(s => s.TrainNo, schedule.TrainNo)
                .Set(s => s.WeekType, schedule.WeekType)
                .Set(s => s.StartStation, schedule.StartStation)
                .Set(s => s.StartTime, schedule.StartTime)
                .Set(s => s.EndStation, schedule.EndStation)
                .Set(s => s.EndTime, schedule.EndTime)
                .Set(s => s.Stations, schedule.Stations)
                .Set(s => s.Train, schedule.Train)
                .Set(s => s.IsPublished, schedule.IsPublished)
                .Set(s => s.IsCancled, schedule.IsCancled);
            Console.WriteLine($"update value: {update}");

            await _trainCollection.UpdateOneAsync(filter, update);
        }
        /// Placeholder method for updating train status. Not implemented.
        /// <param name="trainId">The ID of the train schedule to update.</param>
        Task ITrainScheduleService.UpdatetrainStatus(int trainId)
        {
            //var filter = Builders<TrainSchedule>.Filter.Eq(x => x.TrainNo, trainId);
            //var trainSchedule = await _trainCollection.Find(filter).FirstOrDefaultAsync();
            //return trainSchedule;
            return null;

        }
        /// Updates the status of a specific train schedule in the database.
        /// <param name="id">The ID of the train schedule to update.</param>
        /// <param name="isPublished">The new status of the train schedule (true for published, false for unpublished).</param>
        /// <returns>Boolean indicating whether the update was successful.</returns>
        public async Task<bool> UpdateScheduleStatus(string id, bool isPublished)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TrainSchedule>.Filter.Eq("_id", objectId);
            Console.WriteLine(filter);
            var update = Builders<TrainSchedule>.Update.Set("IsPublished", isPublished);
            
            Console.WriteLine(update);

            var updateResult = await _trainCollection.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }

        /// Updates reservations associated with a specific train schedule.
        /// <param name="id">The ID of the train schedule to update.</param>
        /// <param name="IsCancled">Boolean indicating whether reservations should be canceled or not.</param>
        /// <returns>Boolean indicating whether the update was successful.</returns>
        public async Task<bool> UpdateReservationsBySchedule(string id, bool IsCancled)
        {
            var ScheduleId = id;
            var filter1 = Builders<Reservation>.Filter.Eq("ScheduleId", ScheduleId);
            var count = _reservationCollection.CountDocuments(filter1);
            Console.WriteLine("update reseation is get count" + count);
            //return (int)count;

            if (count > 0)
            {
                Console.WriteLine("called1");
                var objectId = new ObjectId(id);
                var filter = Builders<TrainSchedule>.Filter.Eq("_id", objectId);
                var update = Builders<TrainSchedule>.Update.Set("IsCancled", IsCancled);

                var updateResult = await _trainCollection.UpdateOneAsync(filter, update);

                return updateResult.ModifiedCount > 0;
            }
            else
            {
                Console.WriteLine("called2");
                return false;

            }
            
        }
        /// Updates the status of a specific train in the database.
        /// <param name="id">The ID of the train to update.</param>
        /// <param name="status">The new status of the train (true for active, false for inactive).</param>
        public async Task<bool> UpdateTrainStatus(string id, bool status)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<Trains>.Filter.Eq("_id", objectId);
            var update = Builders<Trains>.Update.Set("Status", status);

            var updateResult = await _trainNewCollection.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }
        /// Updates the status of an existing train in the database.
        public async Task<bool> UpdateExcistingTrainsStatus(string id, bool status)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<Trains>.Filter.Eq("_id", objectId);
            var update = Builders<Trains>.Update.Set("Status", status);

            var updateResult = await _trainNewCollection.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }
        /// Placeholder method for updating schedule status. Not implemented.
        Task<bool> ITrainScheduleService.UpdateschedulesStatus(string id, bool isPublished)
        {
            throw new NotImplementedException();
        }
        /// Updates the status of a specific train schedule in the database.
        async Task<bool> ITrainScheduleService.UpdatesOurchedulesStatus(string id, bool isPublished)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(x => x.Id, id);
            var trainSchedule = await _trainCollection.Find(filter).FirstOrDefaultAsync();
            Console.WriteLine("trainSchedule" + trainSchedule.IsPublished);
            trainSchedule.IsPublished = isPublished;
            Console.WriteLine("trainSchedule2" + trainSchedule.IsPublished);

            var update = Builders<TrainSchedule>.Update
                .Set(s => s.TrainNo, trainSchedule.TrainNo)
                .Set(s => s.WeekType, trainSchedule.WeekType)
                .Set(s => s.StartStation, trainSchedule.StartStation)
                .Set(s => s.StartTime, trainSchedule.StartTime)
                .Set(s => s.EndStation, trainSchedule.EndStation)
                .Set(s => s.EndTime, trainSchedule.EndTime)
                .Set(s => s.Stations, trainSchedule.Stations)
                .Set(s => s.Train, trainSchedule.Train)
                .Set(s => s.IsPublished, trainSchedule.IsPublished)
                .Set(s => s.IsCancled, trainSchedule.IsCancled);
            Console.WriteLine($"update value: {update}");

            var updateResult = await _trainCollection.UpdateOneAsync(filter, update);
            return true;
        }
        /// Updates the cancellation status of a specific train schedule in the database.
        async Task<bool> ITrainScheduleService.UpdatesOurchedulesIsCancledStatus(string id, bool IsCancled)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(x => x.Id, id);
            var trainSchedule = await _trainCollection.Find(filter).FirstOrDefaultAsync();
            Console.WriteLine("trainSchedule" + trainSchedule.IsPublished);
            trainSchedule.IsCancled = IsCancled;
            Console.WriteLine("trainSchedule2" + trainSchedule.IsPublished);

            var ScheduleId = id;
            var filter1 = Builders<Reservation>.Filter.Eq("ScheduleId", ScheduleId);
            var count = _reservationCollection.CountDocuments(filter1);
            Console.WriteLine("update reseation is get count" + count);
            //return (int)count;

            if (count > 0)
            {
                var update = Builders<TrainSchedule>.Update
                .Set(s => s.TrainNo, trainSchedule.TrainNo)
                .Set(s => s.WeekType, trainSchedule.WeekType)
                .Set(s => s.StartStation, trainSchedule.StartStation)
                .Set(s => s.StartTime, trainSchedule.StartTime)
                .Set(s => s.EndStation, trainSchedule.EndStation)
                .Set(s => s.EndTime, trainSchedule.EndTime)
                .Set(s => s.Stations, trainSchedule.Stations)
                .Set(s => s.Train, trainSchedule.Train)
                .Set(s => s.IsPublished, trainSchedule.IsPublished)
                .Set(s => s.IsCancled, trainSchedule.IsCancled);
                Console.WriteLine($"update value: {update}");

                var updateResult = await _trainCollection.UpdateOneAsync(filter, update);

                return true;
            }
            else
            {
                Console.WriteLine("called2");
                return false;

            }

            
        }
    }
}

