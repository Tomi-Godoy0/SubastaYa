using SubastaYa.Application.Interfaces.DTOs;

namespace SubastaYa.Application.Interfaces
{
    public interface IAuctionNotifier
    {
        Task NotifyNewBidAsync(int auctionId, NewBidPayload payload);
        Task NotifyAuctionExtendedAsync(int auctionId, DateTime newEndDate);
    }
}
