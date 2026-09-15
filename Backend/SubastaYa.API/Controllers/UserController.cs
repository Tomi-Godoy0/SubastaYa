using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Interfaces.Service.Users;
using SubastaYa.Application.UseCases.Users.CreateUser;
using SubastaYa.Application.UseCases.Users.GetUser;

namespace SubastaYa.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICreateUserHandler _createUserHandler;
        private readonly IGetUserHandler _getUserHandler;

        public UserController(ICreateUserHandler createUserHandler, IGetUserHandler getUserHandler)
        {
            _createUserHandler = createUserHandler;
            _getUserHandler = getUserHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _getUserHandler.HandleAsync(new GetUserQuery { UserId = id });

            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            var userId = await _createUserHandler.HandleAsync(command);

            return CreatedAtAction(nameof(GetUser), new { id = userId }, new { id = userId });
        }
    }
}
