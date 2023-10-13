/*
 * TrainSchedule Model
 * 
 * Description:
 * This file contains the definition of the TrainSchedule and TrainStation classes used in the TravelEase_WebService project. The TrainSchedule class represents the schedule of a train, including its route, timings, and related information. The TrainStation class represents specific station details within the train schedule.
 * 
 * Author:
 * Madushan S H K
 * IT20122614
 * SLIIT
 * it20122614@my.sliit.lk
 * 
 * Date:
 * 06/10/2023
 * 
 * Usage:
 * - These classes can be utilized to model train schedules within the TravelEase_WebService project.
 * - Modify the classes and their properties as needed to suit the requirements of the project.
 * - Ensure that the necessary MongoDB driver and other dependencies are installed and configured in your project.
 * 
 * Dependencies:
 * - .NET Framework or .NET Core
 * - MongoDB.Driver (install via NuGet Package Manager: Install-Package MongoDB.Driver)
 * 
 */

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

        [BsonElement("IsPublished")]
        public bool? IsPublished { get; set; }

        [BsonElement("IsCancled")]
        public bool? IsCancled { get; set; }
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

