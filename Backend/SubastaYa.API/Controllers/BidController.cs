using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Interfaces.Service.Bids;
using SubastaYa.Application.UseCases.Bids.CreateBid;

namespace SubastaYa.API.Controllers
{
    [Route("api/auctions/{auctionId}/bids")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly ICreateBidHandler _createBidHandler;

        public BidController(ICreateBidHandler createBidHandler) 
        {
            _createBidHandler = createBidHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBid(int auctionId, CreateBidCommand command)
        {
            command.AuctionId = auctionId;
            var bidId = await _createBidHandler.HandleAsync(command);

            return StatusCode(201, new { id = bidId });
        }
    }
}
