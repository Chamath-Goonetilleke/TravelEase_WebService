using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TavelEase_WebService.Data;
using TravelEase_WebService.DTO;
using TravelEase_WebService.Models;
using TravelEase_WebService.Utils;

namespace TravelEase_WebService.Services
{
	public class TravelerService
	{
        private readonly IMongoCollection<Traveler> _travelerCollection;
        private readonly PasswordEncryptionUtil _passwordEncryptionUtil;

        public TravelerService(IOptions<DatabaseSettings> options, IConfiguration configuration, PasswordEncryptionUtil passwordEncryptionUtil)
        {

            var mongoClient = new MongoClient(options.Value.ConnectionString);

            _travelerCollection = mongoClient.GetDatabase(options.Value.DatabaseName)
                  .GetCollection<Traveler>(options.Value.TravelerCollectionName);

            _passwordEncryptionUtil = passwordEncryptionUtil;
        }

        public async Task CreateNewTraveler(UserDTO userDTO)
        {
            var traveler = await _travelerCollection.Find(t => t.Email == userDTO.Email
                                     || t.Nic == userDTO.Nic).FirstOrDefaultAsync();
            if(traveler != null)
            {
                throw new Exception("User already registered.");

            }

            Traveler newTravelr = new() {
                Role = userDTO.Role,
                Title = userDTO.Title,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Nic = userDTO.Nic,
                Email = userDTO.Email,
                PhoneNumber = userDTO.PhoneNumber,
                Password = _passwordEncryptionUtil.HashPassword(userDTO.Password),
                ImageUrl = userDTO.ImageUrl,
                City = userDTO.City,
                State = Traveler.TravelerAccountState.INACTIVE
                
            };

            await _travelerCollection.InsertOneAsync(newTravelr);

        }

        public async Task<List<CurrentUserDTO>> GetAllTravelers()
        {
            var travelers =  await _travelerCollection.Find(t => t.Role == "Traveler").ToListAsync();
            var travelersList = new List<CurrentUserDTO>();
            foreach (var traveler in travelers)
            {
                var t1 = new CurrentUserDTO();
                t1.MapUser(traveler, "Traveler",null,traveler.City, traveler.State);
                travelersList.Add(t1);
            }

            return travelersList;
        }

        public async Task<CurrentUserDTO> GetTravelerByNIC(string nic)
        {
            var traveler = await _travelerCollection.Find(t => t.Nic == nic).
                FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

            var t1 = new CurrentUserDTO();
            t1.MapUser(traveler, "Traveler", null, traveler.City, traveler.State);

            return t1;
        }

        public async Task UpdateTraveler(UserDTO userDTO)
        {
            var traveler = await _travelerCollection.Find(t => t.Nic == userDTO.Nic).
                FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

            var filter = Builders<Traveler>.Filter.Eq(u => u.Nic, userDTO.Nic);
            var update = Builders<Traveler>.Update
                .Set(u => u.Title, userDTO.Title)
                .Set(u => u.FirstName, userDTO.FirstName)
                .Set(u => u.LastName, userDTO.LastName)
                .Set(u => u.PhoneNumber, userDTO.PhoneNumber)
                .Set(u => u.ImageUrl, userDTO.ImageUrl)
                .Set(u => u.City, userDTO.City);

            await _travelerCollection.UpdateOneAsync(filter, update);
        }

        public async Task ActivateTravelerAccount(string nic)
        {
            var traveler = await _travelerCollection.Find(t => t.Nic == nic).
                FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

            var filter = Builders<Traveler>.Filter.Eq(u => u.Nic, nic);
            var update = Builders<Traveler>.Update
                .Set(t => t.State, Traveler.TravelerAccountState.ACTIVE);

            await _travelerCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeactivateTravelerAccount(string nic)
        {
            var traveler = await _travelerCollection.Find(t => t.Nic == nic).
                FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

            var filter = Builders<Traveler>.Filter.Eq(u => u.Nic, nic);
            var update = Builders<Traveler>.Update
                .Set(t => t.State, Traveler.TravelerAccountState.INACTIVE);

            await _travelerCollection.UpdateOneAsync(filter, update);
        }



        public async Task DeleteTraveler(string nic)
        {
            var traveler = await _travelerCollection.Find(t => t.Nic == nic).
                FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

            await _travelerCollection.DeleteOneAsync(t => t.Nic == nic);

        }

    }
}

