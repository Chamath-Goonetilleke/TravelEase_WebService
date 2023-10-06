using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TravelEase_WebService.Services;

namespace TravelEase_WebService.Controllers
{

    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [Route("auth")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Auth(DTO.AuthUserDTO authUser)
        {
            try
            {
                var token = await userService.Auth(authUser);

                CookieOptions cookieOptions = new()
                {
                    Expires = DateTime.Now.AddDays(7)
                };

                HttpContext.Response.Cookies.Append("token", token, cookieOptions);

                return Ok("Login Success!");
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e);
            }
        }

        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> CreateNewUser(DTO.UserDTO userDTO)
        {
            try
            {
                await userService.CreateUser(userDTO);

                return Ok("Successfully Created");
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
        }

        [Route("profile")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetCurrentUser()
        {
            try
            {
                var userDataClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserData")?.Value;
                var userData = JsonConvert.DeserializeObject<DTO.CurrentUserDTO>(userDataClaim);

                return Ok(userData);
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
        }
    }
}
