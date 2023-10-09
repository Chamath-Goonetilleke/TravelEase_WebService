using System;
using Microsoft.Extensions.Options;
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

        async Task<TrainSchedule> ITrainScheduleService.InsertTrainSchedule(TrainSchedule train)
        {
            await _trainCollection.InsertOneAsync(train);
            return train;
        }
    }
}

