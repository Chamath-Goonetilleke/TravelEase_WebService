using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelEase_WebService.Models
{
	public class Trains
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string? Id { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }

        [BsonElement("TrainNo")]
        public int TrainNo { get; set; }

        [BsonElement("Classes")]
        public List<TrainClass>? Classes { get; set; }
    }

    public class TrainClass
    {
        [BsonElement("ClassName")]
        public string? ClassName { get; set; }

        [BsonElement("SeatCount")]
        public int SeatCount { get; set; }
    }
}


