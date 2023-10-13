/*
------------------------------------------------------------------------------
 File: IUserService.cs
 Purpose: This file defines the IUserService interface, which contains
 methods for handling user-related operations in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/
using TravelEase_WebService.DTO;

namespace TravelEase_WebService.Services
{
    public interface IUserService
	{
        Task<string> Auth(AuthUserDTO authUser);
        Task CreateUser(UserDTO userDTO);
        Task UpdateUser(UserDTO userDTO, string uId);
        Task<CurrentUserDTO> GetCurrentUser(string role, string id);
    }
}

