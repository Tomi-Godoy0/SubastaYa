using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Interfaces.Service.Bids;
using SubastaYa.Application.UseCases.Bids.CreateBid;
using SubastaYa.Application.UseCases.Bids.GetBidHistory;
using System.Net;

namespace SubastaYa.API.Controllers
{
    [Route("api/auctions/{auctionId}/bids")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly ICreateBidHandler _createBidHandler;
        private readonly IGetBidHistoryHandler _getBidHistoryHandler;

        public BidController(ICreateBidHandler createBidHandler, IGetBidHistoryHandler getBidHistoryHandler) 
        {
            _createBidHandler = createBidHandler;
            _getBidHistoryHandler = getBidHistoryHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBid(int auctionId, CreateBidCommand command)
        {
            command.AuctionId = auctionId;
            var bidId = await _createBidHandler.HandleAsync(command);

            return StatusCode(201, new { id = bidId });
        }

        [HttpGet]
        public async Task<IActionResult> GetBidHistory(int auctionId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetBidHistoryQuery
            {
                AuctionId = auctionId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var bids = await _getBidHistoryHandler.HandleAsync(query);

            return Ok(bids);
        }
    }
}
