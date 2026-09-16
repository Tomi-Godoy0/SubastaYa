using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.UseCases.Users.GetMyBids;

namespace SubastaYa.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICreateUserHandler _createUserHandler;
        private readonly IGetUserHandler _getUserHandler;
        private readonly IGetUserAuctionsHandler _getUserAuctionsHandler;
        private readonly IGetUserBidsHandler _getUserBidsHandler;

        public UserController(
            ICreateUserHandler createUserHandler, 
            IGetUserHandler getUserHandler, 
            IGetUserAuctionsHandler getUserAuctionsHandler, 
            IGetUserBidsHandler getUserBidsHandler)
        {
            _createUserHandler = createUserHandler;
            _getUserHandler = getUserHandler;
            _getUserAuctionsHandler = getUserAuctionsHandler;
            _getUserBidsHandler = getUserBidsHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            var userId = await _createUserHandler.HandleAsync(command);

            return CreatedAtAction(nameof(GetUser), new { id = userId }, new { id = userId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _getUserHandler.HandleAsync(new GetUserQuery { UserId = id });

            return Ok(user);
        }

        [HttpGet("{sellerId}/auctions")]
        public async Task<IActionResult> GetUserAuctions(int sellerId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetUserAuctionsQuery
            {
                SellerId = sellerId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var auctions = await _getUserAuctionsHandler.HandleAsync(query);

            return Ok(auctions);
        }

        [HttpGet("{buyerId}/bids")]
        public async Task<IActionResult> GetUserBids(int buyerId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetUserBidsQuery
            {
                BuyerId = buyerId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var auctions = await _getUserBidsHandler.HandleAsync(query);

            return Ok(auctions);
        }
    }
}
