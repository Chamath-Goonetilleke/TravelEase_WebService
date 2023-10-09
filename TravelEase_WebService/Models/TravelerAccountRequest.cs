using System;
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


