using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TravelEase_WebService.Data;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
	public class TrainScheduleService: ITrainScheduleService
    {
        private readonly IMongoCollection<TrainSchedule> _trainCollection;
        private readonly IMongoCollection<Reservation> _reservationCollection;
        public TrainScheduleService(IOptions<DatabaseSettings> dbSetting)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);

            _trainCollection = mongoDatabase.GetCollection<TrainSchedule>(dbSetting.Value.TrainScheduleCollectionName);
            _reservationCollection = mongoDatabase.GetCollection<Reservation>(dbSetting.Value.ReservationCollection);
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

        public async Task<bool> UpdateScheduleStatus(string id, bool status)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TrainSchedule>.Filter.Eq("_id", objectId);
            var update = Builders<TrainSchedule>.Update.Set("IsPublished", status);

            var updateResult = await _trainCollection.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }


        public async Task<bool> UpdateReservationsByScheduleNoAsync(string scheduleId, bool status)
        {
            var filter = Builders<Reservation>.Filter.Eq("ScheduleId", scheduleId);
            var count = _reservationCollection.CountDocuments(filter);
            Console.WriteLine("update reseation is get count" + count);
            //return (int)count;

            if (count > 0)
            {
                Console.WriteLine("called1");
                var objectId = new ObjectId(scheduleId);
                var filter1 = Builders<TrainSchedule>.Filter.Eq("_id", objectId);
                var update = Builders<TrainSchedule>.Update.Set("IsCancled", status);

                var updateResult = await _trainCollection.UpdateOneAsync(filter1, update);

                return updateResult.ModifiedCount > 0;
            }
            else
            {
                Console.WriteLine("called2");
                return false;

            }
            
        }
    }
}

