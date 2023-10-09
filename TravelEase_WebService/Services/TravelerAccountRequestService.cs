using System;
using Amazon.Runtime.Internal;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelEase_WebService.Data;
using TravelEase_WebService.DTO;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
	public class TravelerAccountRequestService
	{
        private readonly IMongoCollection<TravelerAccountRequest> _requestCollection;

        public TravelerAccountRequestService(IOptions<DatabaseSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);

            _requestCollection = mongoClient.GetDatabase(options.Value.DatabaseName)
                  .GetCollection<TravelerAccountRequest>(options.Value.TravelerAccountRequestCollectionName);
        }

        public async Task CreateNewRequest(TravelerAccountRequest request)
        {
            var req = await _requestCollection.Find(r => r.RequestType == request.RequestType
                                  && r.TravelerNIC == request.TravelerNIC).FirstOrDefaultAsync();

            if (req != null)
            {
                throw new Exception("Request already created.");

            }

            await _requestCollection.InsertOneAsync(request);
        }

        public async Task<List<TravelerAccountRequest>> GetAllRequests()
        {
            return await _requestCollection.Find(r => true).ToListAsync();

        }

    }
}

