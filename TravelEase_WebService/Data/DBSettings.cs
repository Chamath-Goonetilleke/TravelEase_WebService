/*
------------------------------------------------------------------------------
 File: DatabaseSettings.cs
 Purpose: This file contains the DatabaseSettings class, which defines the
 database connection settings for the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

namespace TravelEase_WebService.Data
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string UserCollectionName { get; set; } = string.Empty;
        public string TrainCollectionName { get; set; } = string.Empty;
        public string TrainScheduleCollectionName { get; set; } = string.Empty;
        public string BackOfficeUserCollectionName { get; set; } = string.Empty;
        public string TravelAgentCollectionName { get; set; } = string.Empty;
        public string TravelerCollectionName { get; set; } = string.Empty;
        public string TravelerAccountRequestCollectionName { get; set; } = string.Empty;
        public string ReservationCollection { get; set; } = string.Empty;
    }
}
