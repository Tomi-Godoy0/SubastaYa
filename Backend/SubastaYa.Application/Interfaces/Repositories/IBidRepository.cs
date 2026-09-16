using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface IBidRepository
    {
        Task<Bid> AddAsync(Bid bid);
        Task<List<Bid>> GetByAuctionIdAsync(int auctionId); // Acá hago el historial de ofertas en una subasta
        Task<(List<Bid> Items, int TotalCount)> GetByBuyerIdAsync(int buyerId, int pageNumber, int pageSize);
        Task<Bid?> GetHighestBidByAuctionIdAsync(int auctionId);
        Task<bool> ExistsByAuctionIdAsync(int auctionId);
    }
}
