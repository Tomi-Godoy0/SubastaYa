using Microsoft.AspNetCore.SignalR;
using SubastaYa.API.Hubs;
using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.DTOs;

namespace SubastaYa.API.Services
{
    public class AuctionNotifierService : IAuctionNotifier
    {
        private readonly IHubContext<AuctionHub> _hubContext;

        public AuctionNotifierService(IHubContext<AuctionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyAuctionExtendedAsync(int auctionId, DateTime newEndDate)
        {
            await _hubContext.Clients.Group($"auction-{auctionId}").SendAsync("AuctionExtended", newEndDate);
        }

        public async Task NotifyNewBidAsync(int auctionId, NewBidPayload payload)
        {
            await _hubContext.Clients.Group($"auction-{auctionId}").SendAsync("NewBid", payload);
        }
    }
}
