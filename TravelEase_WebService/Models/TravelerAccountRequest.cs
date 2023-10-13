/*
------------------------------------------------------------------------------
 File: TravelerAccountRequest.cs
 Purpose: This file contains the TravelerAccountRequest class, which represents
 a request for a traveler account in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelEase_WebService.Models
{
    public class TravelerAccountRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public required string TravelerNIC { get; set; }
        public required int RequestType { get; set; }
        public required DateTime Time { get; set; }
    }
}
