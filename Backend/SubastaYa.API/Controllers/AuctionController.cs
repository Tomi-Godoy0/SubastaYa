using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Interfaces.Service.Auctions;
using SubastaYa.Application.UseCases.Auctions.CreateAuction;
using SubastaYa.Application.UseCases.Auctions.GetAuction;
using SubastaYa.Application.UseCases.Auctions.GetAuctions;

namespace SubastaYa.API.Controllers
{
    [Route("api/auctions")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly ICreateAuctionHandler _createAuctionHandler;
        private readonly IGetAuctionHandler _getAuctionHandler;
        private readonly IGetAuctionsHandler _getAuctionsHandler;

        public AuctionController(ICreateAuctionHandler createAuctionHandler, IGetAuctionHandler getAuctionHandler, IGetAuctionsHandler getAuctionsHandler)
        {
            _createAuctionHandler = createAuctionHandler;
            _getAuctionHandler = getAuctionHandler;
            _getAuctionsHandler = getAuctionsHandler;
        }

        [HttpPost]
        public async Task<IActionResult> createAuction(CreateAuctionCommand command)
        {
            var auction = await _createAuctionHandler.HandleAsync(command);

            return CreatedAtAction(nameof(GetAuction), new { id = auction }, new { id = auction });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuction(int id) 
        {
            var auction = await _getAuctionHandler.HandleAsync(new GetAuctionQuery { AuctionId = id });

            return Ok(auction);
        }

        [HttpGet]
        public async Task<IActionResult> GetAuctions([FromQuery] GetAuctionsQuery query)
        {
            var result = await _getAuctionsHandler.HandleAsync(query);

            return Ok(result);
        }
    }
}
