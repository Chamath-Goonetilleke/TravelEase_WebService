using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelEase_WebService.Models
{
	public class TrainSchedule
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("TrainNo")]
        public int? TrainNo { get; set; }

        [BsonElement("WeekType")]
        public List<string>? WeekType { get; set; }

        [BsonElement("StartStation")]
        public string? StartStation { get; set; }

        [BsonElement("StartTime")]
        public string? StartTime { get; set; }

        [BsonElement("EndStation")]
        public string? EndStation { get; set; }

        [BsonElement("EndTime")]
        public string? EndTime { get; set; }

        [BsonElement("Stations")]
        public List<TrainStation>? Stations { get; set; }

        [BsonElement("Train")]
        public Trains? Train { get; set; }

        [BsonElement("Status")]
        public int? Status { get; set; }
    }

    public class TrainStation
    {
        [BsonElement("NewStartStation")]
        public string? NewStartStation { get; set; }

        [BsonElement("NewEndStation")]
        public string? NewEndStation { get; set; }

        [BsonElement("NewStartTime")]
        public string? NewStartTime { get; set; }

        [BsonElement("NewEndTime")]
        public string? NewEndTime { get; set; }

        [BsonElement("Distance")]
        public int? Distance { get; set; }
    }
}


//status(0-PUBLISHED, 1-UNPUBLISHED)

