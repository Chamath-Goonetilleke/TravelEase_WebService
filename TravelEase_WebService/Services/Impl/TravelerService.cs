/*
------------------------------------------------------------------------------
 File: TravelerService.cs
 Purpose: This file contains the TravelerService class, which is responsible
 for handling traveler-related operations in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using TravelEase_WebService.Data;
using TravelEase_WebService.DTO;
using TravelEase_WebService.Models;
using TravelEase_WebService.Utils;

namespace TravelEase_WebService.Services
{
    public class TravelerService : ITravelerService
    {
        private readonly IMongoCollection<Traveler> _travelerCollection;
        private readonly PasswordEncryptionUtil _passwordEncryptionUtil;
        private readonly IConfiguration _configuration;

        public TravelerService(IOptions<DatabaseSettings> options, IConfiguration configuration, PasswordEncryptionUtil passwordEncryptionUtil)
        {

            var mongoClient = new MongoClient(options.Value.ConnectionString);

            _travelerCollection = mongoClient.GetDatabase(options.Value.DatabaseName)
                  .GetCollection<Traveler>(options.Value.TravelerCollectionName);
            _configuration = configuration;
            _passwordEncryptionUtil = passwordEncryptionUtil;
        }

        //------------------------------------------------------------------------------
        // Method: GenerateJWTToken
        // Purpose: Generates a JWT token for authentication.
        //------------------------------------------------------------------------------
        public string GenerateJWTToken(string role, string id)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSettings:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserRole", role),
                        new Claim("UserID", id),

                    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //------------------------------------------------------------------------------
        // Method: TravelerAuth
        // Purpose: Authenticates a traveler.
        //------------------------------------------------------------------------------
        public async Task<TravelerDTO> TravelerAuth(AuthUserDTO authUserDTO)
        {
            var traveler = await _travelerCollection.Find(t => t.Email == authUserDTO.Email).FirstOrDefaultAsync()
                ?? throw new Exception("No user found with the provided email.");

            if (!_passwordEncryptionUtil.VerifyPassword(authUserDTO.Password, traveler.Password))
            {
                throw new Exception("Wrong password.");
            }
            TravelerDTO traveler1 = new();
            traveler1.MapTraveler(traveler, GenerateJWTToken(traveler.Role, traveler.Id));
            return traveler1;
        }

        //------------------------------------------------------------------------------
        // Method: CreateNewTraveler
        // Purpose: Creates a new traveler account.
        //------------------------------------------------------------------------------
        public async Task CreateNewTraveler(UserDTO userDTO)
        {
            var traveler = await _travelerCollection.Find(t => t.Email == userDTO.Email
                                     || t.Nic == userDTO.Nic).FirstOrDefaultAsync();
            if (traveler != null)
            {
                throw new Exception("User already registered.");

            }

            Traveler newTravelr = new()
            {
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

        //------------------------------------------------------------------------------
        // Method: GetAllTravelers
        // Purpose: Retrieves a list of all travelers.
        //------------------------------------------------------------------------------
        public async Task<List<CurrentUserDTO>> GetAllTravelers()
        {
            var travelers = await _travelerCollection.Find(t => t.Role == "Traveler").ToListAsync();
            var travelersList = new List<CurrentUserDTO>();
            foreach (var traveler in travelers)
            {
                var t1 = new CurrentUserDTO();
                t1.MapUser(traveler, "Traveler", null, traveler.City, traveler.State);
                travelersList.Add(t1);
            }

            return travelersList;
        }

        //------------------------------------------------------------------------------
        // Method: GetTravelerByNIC
        // Purpose: Retrieves traveler information by NIC.
        //------------------------------------------------------------------------------
        public async Task<CurrentUserDTO> GetTravelerByNIC(string nic)
        {
            var traveler = await _travelerCollection.Find(t => t.Nic == nic).
                FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

            var t1 = new CurrentUserDTO();
            t1.MapUser(traveler, "Traveler", null, traveler.City, traveler.State);

            return t1;
        }

        //------------------------------------------------------------------------------
        // Method: UpdateTraveler
        // Purpose: Updates traveler information.
        //------------------------------------------------------------------------------
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

        //------------------------------------------------------------------------------
        // Method: ActivateTravelerAccount
        // Purpose: Activates a traveler account by NIC.
        //------------------------------------------------------------------------------
        public async Task ActivateTravelerAccount(string nic)
        {
            var traveler = await _travelerCollection.Find(t => t.Nic == nic).
                FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

            var filter = Builders<Traveler>.Filter.Eq(u => u.Nic, nic);
            var update = Builders<Traveler>.Update
                .Set(t => t.State, Traveler.TravelerAccountState.ACTIVE);



            await _travelerCollection.UpdateOneAsync(filter, update);
        }

        //------------------------------------------------------------------------------
        // Method: DeactivateTravelerAccount
        // Purpose: Deactivates a traveler account by NIC.
        //------------------------------------------------------------------------------
        public async Task DeactivateTravelerAccount(string nic)
        {
            var traveler = await _travelerCollection.Find(t => t.Nic == nic).
                FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

            var filter = Builders<Traveler>.Filter.Eq(u => u.Nic, nic);
            var update = Builders<Traveler>.Update
                .Set(t => t.State, Traveler.TravelerAccountState.INACTIVE);

            await _travelerCollection.UpdateOneAsync(filter, update);
        }

        //------------------------------------------------------------------------------
        // Method: DeleteTraveler
        // Purpose: Deletes a traveler account by NIC.
        //------------------------------------------------------------------------------
        public async Task DeleteTraveler(string nic)
        {
            var traveler = await _travelerCollection.Find(t => t.Nic == nic).
                FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

            await _travelerCollection.DeleteOneAsync(t => t.Nic == nic);

        }

    }
}

