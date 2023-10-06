
using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelEase_WebService.Models;
using TavelEase_WebService.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelEase_WebService.DTO;
using Newtonsoft.Json;

namespace TravelEase_WebService.Services
{
    public class UserService
    {
        private readonly IMongoCollection<BackOfficeUser> _bouCollection;
        private readonly IMongoCollection<TravelAgent> _travelAgentCollection;
        private readonly IConfiguration _configuration;

        public UserService(IOptions<DatabaseSettings> options, IConfiguration configuration)
        {

            var mongoClient = new MongoClient(options.Value.ConnectionString);
            _configuration = configuration;

            _bouCollection = mongoClient.GetDatabase(options.Value.DatabaseName)
                   .GetCollection<BackOfficeUser>(options.Value.BackOfficeUserCollectionName);

            _travelAgentCollection = mongoClient.GetDatabase(options.Value.DatabaseName)
                  .GetCollection<TravelAgent>(options.Value.TravelAgentCollectionName);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }

        public string GenerateJWTToken(string data, string role, string id)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSettings:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserData", data),
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


        public async Task CreateUser(UserDTO userDTO)
        {
            var userRole = userDTO.Role;

            if (userRole == null)
            {
                throw new Exception("User role cannot be null.");
            }

            if (userRole == "TravelAgent")
            {
                TravelAgent travelAgent = new()
                {
                    Role = userDTO.Role,
                    Title = userDTO.Title,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Nic = userDTO.Nic,
                    Email = userDTO.Email,
                    PhoneNumber = userDTO.PhoneNumber,
                    Password = HashPassword(userDTO.Password),
                    TravelAgentId = userDTO.TravelAgentId

                };

                await _travelAgentCollection.InsertOneAsync(travelAgent);
            }
            else if (userRole == "BackOfficeUser")
            {
                BackOfficeUser backOfficeUser = new()
                {
                    Role = userDTO.Role,
                    Title = userDTO.Title,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Nic = userDTO.Nic,
                    Email = userDTO.Email,
                    PhoneNumber = userDTO.PhoneNumber,
                    Password = HashPassword(userDTO.Password),
                    EmployeeId = userDTO.EmployeeId

                };

                await _bouCollection.InsertOneAsync(backOfficeUser);
            }
        }

        public async Task<string> Auth(AuthUserDTO authUser)
        {
            var userRole = authUser.Role;

            var currentUser = new CurrentUserDTO();

            if (userRole == "TravelAgent")
            {
                var user = await _travelAgentCollection.Find(u => u.Email == authUser.Email).
                    FirstOrDefaultAsync() ?? throw new Exception("No user found with the provided email.");

                currentUser.MapUser(user, "TravelAgent", user.TravelAgentId);
                string jsonUserData = JsonConvert.SerializeObject(currentUser);

                if (!VerifyPassword(authUser.Password, user.Password))
                {
                    throw new Exception("Wrong password.");
                }

                return GenerateJWTToken(jsonUserData, user.Role, user.Id);
            }
            else if (userRole == "BackOfficeUser")
            {
                var user = await _bouCollection.Find(u => u.Email == authUser.Email).
                    FirstOrDefaultAsync() ?? throw new Exception("No user found with the provided email.");

                currentUser.MapUser(user, "BackOfficeUser", user.EmployeeId);
                string jsonUserData = JsonConvert.SerializeObject(currentUser);

                if (!VerifyPassword(authUser.Password, user.Password))
                {
                    throw new Exception("Wrong password.");
                }

                return GenerateJWTToken(jsonUserData, user.Role, user.Id);
            }
            throw new Exception("In Valid User Role");
        }

    }
}

