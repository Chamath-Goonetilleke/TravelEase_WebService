using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelEase_WebService.Data;
using TravelEase_WebService.Models;
namespace TravelEase_WebService.Services
{
    public class TrainService: ITrainService
    {
        private readonly IMongoCollection<Trains> _trainCollection;

        public TrainService(IOptions<DatabaseSettings> dbSetting)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);

            _trainCollection = mongoDatabase.GetCollection<Trains>(dbSetting.Value.TrainCollectionName);
        }

        public async Task<Trains> InsertTrain(Trains train)
        {
            Console.WriteLine($">>>>>>>>>>>>>>train name: {train}");
            await _trainCollection.InsertOneAsync(train);
            return train;
        }
    }
}

