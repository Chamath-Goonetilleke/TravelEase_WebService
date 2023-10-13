/*
------------------------------------------------------------------------------
 File: UserController.cs
 Purpose: This file contains the UserController class, which is a controller
 for handling user-related operations in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase_WebService.DTO;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{
    [ApiController]
    [Route("/api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        //------------------------------------------------------------------------------
        // Method: Auth
        // Purpose: Authenticates a user and sets a token cookie upon successful login.
        //------------------------------------------------------------------------------
        [Route("auth")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Auth(AuthUserDTO authUser)
        {
            try
            {
                var token = await userService.Auth(authUser);

                CookieOptions cookieOptions = new()
                {
                    Expires = DateTime.Now.AddDays(7),
                };

                HttpContext.Response.Cookies.Append("token", token, cookieOptions);

                return Ok("Login Success!");
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
        }

        //------------------------------------------------------------------------------
        // Method: CreateNewUser
        // Purpose: Registers a new user.
        //------------------------------------------------------------------------------
        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> CreateNewUser(UserDTO userDTO)
        {
            try
            {
                await userService.CreateUser(userDTO);

                return Ok("Successfully Registered");
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
        }

        //------------------------------------------------------------------------------
        // Method: UpdateUser
        // Purpose: Updates user information.
        //------------------------------------------------------------------------------
        [Route("update")]
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUser(UserDTO userDTO)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;

                await userService.UpdateUser(userDTO, userId);

                return Ok("Successfully Updated");
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
        }

        //------------------------------------------------------------------------------
        // Method: GetCurrentUser
        // Purpose: Retrieves the profile of the currently authenticated user.
        //------------------------------------------------------------------------------
        [Route("profile")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetCurrentUser()
        {
            try
            {
                var userDataClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserData")?.Value;
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                var userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserRole")?.Value;

                var userData = await userService.GetCurrentUser(userRole, userId);

                return Ok(userData);
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
        }
    }
}
