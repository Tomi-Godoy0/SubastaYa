using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Interfaces.Service.Users;
using SubastaYa.Application.UseCases.Users.UserAuthentication;

namespace SubastaYa.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthenticationHandler _userAuthenticationHandler;

        public AuthController(IUserAuthenticationHandler userAuthenticationHandler)
        {
            _userAuthenticationHandler = userAuthenticationHandler;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserAuthenticationQuery query)
        {
            var userId = await _userAuthenticationHandler.HandleAsync(query);

            return Ok(new
            {
                id = userId
            });
        }
    }
}