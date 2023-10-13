/*
------------------------------------------------------------------------------
 File: UserService.cs
 Purpose: This file contains the UserService class, which is responsible
 for handling user-related operations in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelEase_WebService.Models;
using TravelEase_WebService.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelEase_WebService.DTO;
using TravelEase_WebService.Utils;

namespace TravelEase_WebService.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<BackOfficeUser> _bouCollection;
        private readonly IMongoCollection<TravelAgent> _travelAgentCollection;
        private readonly IConfiguration _configuration;
        private readonly PasswordEncryptionUtil _passwordEncryptionUtil;

        public UserService(IOptions<DatabaseSettings> options, IConfiguration configuration, PasswordEncryptionUtil passwordEncryptionUtil)
        {

            var mongoClient = new MongoClient(options.Value.ConnectionString);
            _configuration = configuration;

            _bouCollection = mongoClient.GetDatabase(options.Value.DatabaseName)
                   .GetCollection<BackOfficeUser>(options.Value.BackOfficeUserCollectionName);

            _travelAgentCollection = mongoClient.GetDatabase(options.Value.DatabaseName)
                  .GetCollection<TravelAgent>(options.Value.TravelAgentCollectionName);

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
        // Method: Auth
        // Purpose: Authenticates a user.
        //------------------------------------------------------------------------------
        public async Task<string> Auth(AuthUserDTO authUser)
        {
            var userRole = authUser.Role;

            if (userRole == "TravelAgent")
            {
                var user = await _travelAgentCollection.Find(u => u.Email == authUser.Email).
                    FirstOrDefaultAsync() ?? throw new Exception("No user found with the provided email.");

                if (!_passwordEncryptionUtil.VerifyPassword(authUser.Password, user.Password))
                {
                    throw new Exception("Wrong password.");
                }

                return GenerateJWTToken(user.Role, user.Id);
            }
            else if (userRole == "BackOfficeUser")
            {
                var user = await _bouCollection.Find(u => u.Email == authUser.Email).
                    FirstOrDefaultAsync() ?? throw new Exception("No user found with the provided email.");

                if (!_passwordEncryptionUtil.VerifyPassword(authUser.Password, user.Password))
                {
                    throw new Exception("Wrong password.");
                }

                return GenerateJWTToken(user.Role, user.Id);
            }
            throw new Exception("In Valid User Role");
        }

        //------------------------------------------------------------------------------
        // Method: CreateUser
        // Purpose: Creates a new user account.
        //------------------------------------------------------------------------------
        public async Task CreateUser(UserDTO userDTO)
        {
            var userRole = userDTO.Role ?? throw new Exception("User role cannot be null.");

            if (userRole == "TravelAgent")
            {
                var user = await _travelAgentCollection.Find(u => u.Email == userDTO.Email || u.Nic == userDTO.Nic).FirstOrDefaultAsync();

                if (user != null)
                {
                    throw new Exception("User is already registered.");
                }

                TravelAgent travelAgent = new()
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
                    TravelAgentId = userDTO.TravelAgentId
                };

                await _travelAgentCollection.InsertOneAsync(travelAgent);
            }
            else if (userRole == "BackOfficeUser")
            {
                var user = await _bouCollection.Find(u => u.Email == userDTO.Email).FirstOrDefaultAsync();

                if (user != null)
                {
                    throw new Exception("Email is already registered.");
                }

                BackOfficeUser backOfficeUser = new()
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
                    EmployeeId = userDTO.EmployeeId

                };

                await _bouCollection.InsertOneAsync(backOfficeUser);
            }
        }

        //------------------------------------------------------------------------------
        // Method: UpdateUser
        // Purpose: Updates user information.
        //------------------------------------------------------------------------------
        public async Task UpdateUser(UserDTO userDTO, string uId)
        {
            var userRole = userDTO.Role ?? throw new Exception("User role cannot be null.");

            if (userRole == "TravelAgent")
            {
                var user = await _travelAgentCollection.Find(u => u.Id == uId).
                    FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

                var filter = Builders<TravelAgent>.Filter.Eq(u => u.Id, uId);
                var update = Builders<TravelAgent>.Update
                    .Set(u => u.Title, userDTO.Title)
                    .Set(u => u.FirstName, userDTO.FirstName)
                    .Set(u => u.LastName, userDTO.LastName)
                    .Set(u => u.PhoneNumber, userDTO.PhoneNumber)
                    .Set(u => u.ImageUrl, userDTO.ImageUrl)
                    .Set(u => u.TravelAgentId, userDTO.TravelAgentId);

                await _travelAgentCollection.UpdateOneAsync(filter, update);
            }
            else if (userRole == "BackOfficeUser")
            {
                var user = await _bouCollection.Find(u => u.Id == uId).
                    FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

                var filter = Builders<BackOfficeUser>.Filter.Eq(u => u.Id, uId);
                var update = Builders<BackOfficeUser>.Update
                    .Set(u => u.Title, userDTO.Title)
                    .Set(u => u.FirstName, userDTO.FirstName)
                    .Set(u => u.LastName, userDTO.LastName)
                    .Set(u => u.PhoneNumber, userDTO.PhoneNumber)
                    .Set(u => u.ImageUrl, userDTO.ImageUrl)
                    .Set(u => u.EmployeeId, userDTO.EmployeeId);

                await _bouCollection.UpdateOneAsync(filter, update);
            }
        }

        //------------------------------------------------------------------------------
        // Method: GetCurrentUser
        // Purpose: Retrieves current user information.
        //------------------------------------------------------------------------------
        public async Task<CurrentUserDTO> GetCurrentUser(string role, string id)
        {
            var currentUser = new CurrentUserDTO();

            if (role == "TravelAgent")
            {
                var user = await _travelAgentCollection.Find(u => u.Id == id).
                    FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

                currentUser.MapUser(user, "TravelAgent", user.TravelAgentId);

            }
            else if (role == "BackOfficeUser")
            {
                var user = await _bouCollection.Find(u => u.Id == id).
                    FirstOrDefaultAsync() ?? throw new Exception("No User Found.");

                currentUser.MapUser(user, "BackOfficeUser", user.EmployeeId);

            }
            return currentUser;

        }
    }
}

