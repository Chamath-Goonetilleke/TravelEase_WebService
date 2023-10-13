/*
------------------------------------------------------------------------------
 File: TravelerAccountRequestService.cs
 Purpose: This file contains the TravelerAccountRequestService class, which is responsible
 for handling traveler account requests in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelEase_WebService.Data;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
    public class TravelerAccountRequestService: ITravelerAccountRequestService
    {
        private readonly IMongoCollection<TravelerAccountRequest> _requestCollection;

        public TravelerAccountRequestService(IOptions<DatabaseSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);

            _requestCollection = mongoClient.GetDatabase(options.Value.DatabaseName)
                  .GetCollection<TravelerAccountRequest>(options.Value.TravelerAccountRequestCollectionName);
        }

        //------------------------------------------------------------------------------
        // Method: CreateNewRequest
        // Purpose: Creates a new traveler account request.
        //------------------------------------------------------------------------------
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

        //------------------------------------------------------------------------------
        // Method: DeleteRequest
        // Purpose: Deletes a traveler account request by NIC.
        //------------------------------------------------------------------------------
        public async Task DeleteRequest(string nic)
        {
            var req = await _requestCollection.Find(r => r.TravelerNIC == nic).FirstOrDefaultAsync()
                ?? throw new Exception("No Req Found.");
            await _requestCollection.DeleteOneAsync(r => r.TravelerNIC == nic);
        }

        //------------------------------------------------------------------------------
        // Method: GetAllRequests
        // Purpose: Get all activation requests.
        //------------------------------------------------------------------------------
        public async Task<List<TravelerAccountRequest>> GetAllRequests()
        {
            return await _requestCollection.Find(r => true).ToListAsync();

        }

    }
}

