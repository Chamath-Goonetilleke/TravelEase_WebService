using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelEase_WebService.Models
{
	public class Trains
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("trainNo")]
        public int TrainNo { get; set; }

        [BsonElement("classes")]
        public List<TrainClass>? Classes { get; set; }
    }

    public class TrainClass
    {
        [BsonElement("className")]
        public string? ClassName { get; set; }

        [BsonElement("seatCount")]
        public int SeatCount { get; set; }
    }
}


