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
        public TrainScheduleService(IOptions<DatabaseSettings> dbSetting)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);

            _trainCollection = mongoDatabase.GetCollection<TrainSchedule>(dbSetting.Value.TrainScheduleCollectionName);
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
                .Set(s => s.Status, schedule.Status);
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
    }
}

