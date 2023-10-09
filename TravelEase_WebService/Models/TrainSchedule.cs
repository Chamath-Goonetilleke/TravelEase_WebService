﻿using System;
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

        [BsonElement("StartTime")]
        public string? StartTime { get; set; }

        [BsonElement("EndStation")]
        public string? EndStation { get; set; }

        [BsonElement("EndTime")]
        public string? EndTime { get; set; }

        [BsonElement("Stations")]
        public List<TrainStation>? Stations { get; set; }
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
        public int Distance { get; set; }
    }
}
