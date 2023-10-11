using Microsoft.Extensions.Options;
using MongoDB.Bson;
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

        public async Task<List<Trains>> GetAllTrains()
        {
            return await _trainCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Trains> GetTrainsById(string id)
        {
            return await _trainCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

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

