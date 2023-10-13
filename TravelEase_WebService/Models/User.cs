/*
------------------------------------------------------------------------------
 File: User.cs
 Purpose: This file contains the User class, which represents a user in the
 TravelEase_WebService project. It includes various user-related properties.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelEase_WebService.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public required string Role { get; set; }
        public required string Title { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Nic { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public string? ImageUrl { get; set; }
    }
}
