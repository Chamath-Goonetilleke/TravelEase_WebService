using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelEase_WebService.Models
{
    public class Reservation
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public required string TravelerNIC { get; set; }
        public required string ScheduleId { get; set; }
		public required string From { get; set; }
		public required string To { get; set; }
		public required string Time { get; set; }
		public required string Date { get; set; }
		public required string TrainName { get; set; }
		public required string TrainNo { get; set; }
		public required string TrainClass { get; set; }
		public required string ClassPrice { get; set; }
		public required string TotalPrice { get; set; }
		public required List<Passenger> Passengers { get; set; }

        public required bool IsTravelerCreated { get; set; }
        public string? TravelAgentId { get; set; }

        public bool? IsCancelled { get; set; }
    }

    public class Passenger
	{
        public required string Type { get; set; }
        public string? Nic { get; set; }
    }
}

