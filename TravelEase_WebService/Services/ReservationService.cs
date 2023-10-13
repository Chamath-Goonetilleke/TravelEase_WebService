﻿
using System;
using Microsoft.AspNetCore.Http;
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
        private readonly IMongoCollection<Reservation> _reservationCollection;

        public ReservationService(IOptions<DatabaseSettings> dbSetting)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);

            _trainCollection = mongoDatabase.GetCollection<Trains>(dbSetting.Value.TrainCollectionName);
            _trainScheduleCollection = mongoDatabase.GetCollection<TrainSchedule>(dbSetting.Value.TrainScheduleCollectionName);
            _reservationCollection = mongoDatabase.GetCollection<Reservation>(dbSetting.Value.ReservationCollection);
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

        public async Task CreateNewReservation(Reservation reservation)
        {
            reservation.IsCancelled = false;
            await _reservationCollection.InsertOneAsync(reservation);
        }

        public async Task<List<Reservation>> GetReservationByTravelerId(string nic)
        {
            var reservations = await _reservationCollection.Find(r => r.TravelerNIC == nic && r.IsCancelled == false ).ToListAsync();

            if(reservations.Count != 0)
            {
                var upCommingReservations = new List<Reservation>();
                foreach(var res in reservations)
                {
                    if (CheckDate(res.Date))
                    {
                        upCommingReservations.Add(res);
                    }
                }

                return upCommingReservations;
            }
            return reservations;
        }

        public async Task<List<Reservation>> GetReservationHistory(string nic)
        {
            var reservations = await _reservationCollection.Find(r => r.TravelerNIC == nic && r.IsCancelled == false).ToListAsync();

            if (reservations.Count != 0)
            {
                var upCommingReservations = new List<Reservation>();
                foreach (var res in reservations)
                {
                    if (!CheckDate(res.Date))
                    {
                        upCommingReservations.Add(res);
                    }
                }

                return upCommingReservations;
            }

            return reservations;
        }



        public async Task<List<Reservation>> GetReservationByTravelAgent(string id)
        {
            var reservations = await _reservationCollection.Find(r => r.IsTravelerCreated == false
            && r.TravelAgentId == id && r.IsCancelled == false).ToListAsync();

            if (reservations.Count != 0)
            {
                var upCommingReservations = new List<Reservation>();
                foreach (var res in reservations)
                {
                    if (CheckDate(res.Date))
                    {
                        upCommingReservations.Add(res);
                    }
                }

                return upCommingReservations;
            }
            return reservations;
        }

        public async Task UpdateReservation(ReservationUpdateDTO updateDTO)
        {
            var reservation = await _reservationCollection.Find(r => r.Id == updateDTO.Id).FirstOrDefaultAsync()
                ?? throw new Exception("Incorrect reservation Id");

            var filter = Builders<Reservation>.Filter.Eq(r => r.Id, updateDTO.Id);

            if(updateDTO.IsCancel == true)
            {
                var update = Builders<Reservation>.Update
                .Set(r => r.IsCancelled, true);
                await _reservationCollection.UpdateOneAsync(filter, update);

            }
            else
            {
                var update = Builders<Reservation>.Update
                .Set(r => r.Date, updateDTO.Date);
                await _reservationCollection.UpdateOneAsync(filter, update);

            }

        }

        public bool CheckDate(string date)
        {
            if (DateTime.TryParse(date, out DateTime dateToCheck))
            {
                DateTime currentDate = DateTime.Today;

                TimeSpan difference = dateToCheck - currentDate;
                int daysDifference = (int)difference.TotalDays;

                if (daysDifference >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new Exception("Invalid date format.");
            }
        }

    }
}

