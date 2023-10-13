/*
 * ITrainService Interface
 * 
 * Description:
 * This interface defines the contract for managing train data in the TravelEase_WebService project.
 * It provides methods for retrieving, inserting, and updating train information.
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
 * Methods:
 */

using System;
using TravelEase_WebService.Models;

namespace TravelEase_WebService.Services
{
	public interface ITrainService
	{
        Task<List<Trains>> GetAllTrains();/// Retrieves a list of all trains from the database.
        Task<Trains> GetTrainsById(string id);/// Retrieves a specific train by its unique ID.
        Task<Trains> InsertTrain(Trains train);/// Inserts a new train into the database.
        Task<bool> UpdateTrainStatus(string id, bool status);/// Updates the status of a specific train in the database.
    }
}

