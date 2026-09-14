using Microsoft.AspNetCore.SignalR;

namespace SubastaYa.API.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task JoinAuctionGroup(int auctionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"auction-{auctionId}");
        }

        public async Task LeaveAuctionGroup(int auctionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"auction-{auctionId}"); 
        }
    }
}
