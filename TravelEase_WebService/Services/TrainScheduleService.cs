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
        public TrainScheduleService(IOptions<DatabaseSettings> dbSetting)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);

            _trainCollection = mongoDatabase.GetCollection<TrainSchedule>(dbSetting.Value.TrainScheduleCollectionName);
            _reservationCollection = mongoDatabase.GetCollection<Reservation>(dbSetting.Value.ReservationCollection);
            _trainNewCollection = mongoDatabase.GetCollection<Trains>(dbSetting.Value.TrainCollectionName);
        }

        public async Task<List<TrainSchedule>> GetAllTrainSchedule()
        {
            return await _trainCollection.Find(_ => true).ToListAsync();

        }

        async Task<TrainSchedule> ITrainScheduleService.GetTrainsScheduleById(int trainId)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(x => x.TrainNo, trainId);
            var trainSchedule = await _trainCollection.Find(filter).FirstOrDefaultAsync();
            return trainSchedule;
        }

        async Task<TrainSchedule> ITrainScheduleService.InsertTrainSchedule(TrainSchedule train)
        {
            await _trainCollection.InsertOneAsync(train);
            return train;
        }

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

        Task ITrainScheduleService.UpdatetrainStatus(int trainId)
        {
            //var filter = Builders<TrainSchedule>.Filter.Eq(x => x.TrainNo, trainId);
            //var trainSchedule = await _trainCollection.Find(filter).FirstOrDefaultAsync();
            //return trainSchedule;
            return null;

        }

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

        public async Task<bool> UpdateTrainStatus(string id, bool status)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<Trains>.Filter.Eq("_id", objectId);
            var update = Builders<Trains>.Update.Set("Status", status);

            var updateResult = await _trainNewCollection.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdateExcistingTrainsStatus(string id, bool status)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<Trains>.Filter.Eq("_id", objectId);
            var update = Builders<Trains>.Update.Set("Status", status);

            var updateResult = await _trainNewCollection.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }

        Task<bool> ITrainScheduleService.UpdateschedulesStatus(string id, bool isPublished)
        {
            throw new NotImplementedException();
        }

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

