/*
 * Trains Model
 * 
 * Description:
 * This file contains the definition of the Trains and TrainClass classes used in the TravelEase_WebService project. The Trains class represents train entities, including train details and availability status, while the TrainClass class represents different classes of seats available on the train.
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
 * - These classes can be utilized to model train data within the TravelEase_WebService project.
 * - Modify the classes and their properties as needed to suit the requirements of the project.
 * - Ensure that the necessary MongoDB driver and other dependencies are installed and configured in your project.
 * 
 * Dependencies:
 * - .NET Framework or .NET Core
 * - MongoDB.Driver (install via NuGet Package Manager)
 * - [List any other external libraries or packages required]
 *
 * References:
 * - 
 * 
 * Note:
 * 
 */
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

        [BsonElement("Name")]
        public string? Name { get; set; }

        [BsonElement("TrainNo")]
        public int TrainNo { get; set; }

        [BsonElement("Classes")]
        public List<TrainClass>? Classes { get; set; }

        [BsonElement("Status")]
        public bool? Status { get; set; }

    }

    public class TrainClass
    {
        [BsonElement("ClassName")]
        public string? ClassName { get; set; }

        [BsonElement("SeatCount")]
        public int? SeatCount { get; set; }

        [BsonElement("availableCount")]
        public int? AvailableCount { get; set; }
    }
}


//status(0-CANCLED, 1-PUBLISHED)