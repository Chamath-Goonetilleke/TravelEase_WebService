using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelEase_WebService.Data;
using TravelEase_WebService.DTO;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
	public class ReservationService
	{
        private readonly IMongoCollection<Trains> _trainCollection;
        private readonly IMongoCollection<TrainSchedule> _trainScheduleCollection;

        public ReservationService(IOptions<DatabaseSettings> dbSetting)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);

            _trainCollection = mongoDatabase.GetCollection<Trains>(dbSetting.Value.TrainCollectionName);
            _trainScheduleCollection = mongoDatabase.GetCollection<TrainSchedule>(dbSetting.Value.TrainScheduleCollectionName);
        }

        public async Task<List<ScheduleCheckerDTO>> GetTrainsSchedules()
        {
            var trainsSchedules = await _trainScheduleCollection.Find(ts => true).ToListAsync();

            var scheduleList = new List<ScheduleCheckerDTO>();

            foreach(var schedule in trainsSchedules){
                var stationList = new List<string>();
                foreach(var station in schedule.Stations)
                {
                    stationList.Add(station.NewStartStation);
                }             

                var train = await _trainCollection.Find(t => t.TrainNo == schedule.TrainNo).FirstOrDefaultAsync();
                var scheduleChecker = new ScheduleCheckerDTO();
                scheduleChecker.Stations = stationList;
                scheduleChecker.Train = train;
                scheduleChecker.TrainSchedule = schedule;

                scheduleList.Add(scheduleChecker);
            }
            return scheduleList;
        }

    }
}

