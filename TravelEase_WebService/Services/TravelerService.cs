using System;
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
            var traveler = await _travelerCollection.Find(t => t.Email == userDTO.Email || t.Nic == userDTO.Nic).FirstOrDefaultAsync();
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
                City = userDTO.City
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
                t1.MapUser(traveler, "Traveler",null,traveler.City);
                travelersList.Add(t1);
            }

            return travelersList;
        }

    }
}

