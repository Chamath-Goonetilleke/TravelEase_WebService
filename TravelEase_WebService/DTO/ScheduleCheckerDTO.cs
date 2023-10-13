/*
------------------------------------------------------------------------------
 File: ScheduleCheckerDTO.cs
 Purpose: This file contains the ScheduleCheckerDTO class, which defines a data transfer
 object for checking train schedule and related details in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

using TravelEase_WebService.Models;

namespace TravelEase_WebService.DTO
{
    public class ScheduleCheckerDTO
    {
        public TrainSchedule? TrainSchedule { get; set; }
        public List<string>? Stations { get; set; }
        public Trains? Train { get; set; }
    }
}
