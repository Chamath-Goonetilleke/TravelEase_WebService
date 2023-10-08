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

        [BsonElement("trainNo")]
        public int TrainNo { get; set; }

        [BsonElement("weekType")]
        public string? WeekType { get; set; }

        [BsonElement("startStation")]
        public string? StartStation { get; set; }

        [BsonElement("startTime")]
        public string? StartTime { get; set; }

        [BsonElement("endStation")]
        public string? EndStation { get; set; }

        [BsonElement("endTime")]
        public string? EndTime { get; set; }

        [BsonElement("stations")]
        public List<TrainStation>? Stations { get; set; }
    }

    public class TrainStation
    {
        [BsonElement("newStartStation")]
        public string? NewStartStation { get; set; }

        [BsonElement("newEndStation")]
        public string? NewEndStation { get; set; }

        [BsonElement("newStartTime")]
        public string? NewStartTime { get; set; }

        [BsonElement("newEndTime")]
        public string? NewEndTime { get; set; }

        [BsonElement("distance")]
        public int Distance { get; set; }
    }
}

