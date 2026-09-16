using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface IBidRepository
    {
        Task<Bid> AddAsync(Bid bid);
        Task<(List<Bid> Items, int TotalCount)> GetByAuctionIdAsync(int auctionId, int pageNumber, int pageSize);
        Task<int> CountByAuctionIdAsync(int auctionId);
        Task<(List<Bid> Items, int TotalCount)> GetByBuyerIdAsync(int buyerId, int pageNumber, int pageSize);
        Task<Bid?> GetHighestBidByAuctionIdAsync(int auctionId);
        Task<bool> ExistsByAuctionIdAsync(int auctionId);
    }
}
