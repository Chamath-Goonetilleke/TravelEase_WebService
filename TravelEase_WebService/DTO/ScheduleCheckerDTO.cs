using System;
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

