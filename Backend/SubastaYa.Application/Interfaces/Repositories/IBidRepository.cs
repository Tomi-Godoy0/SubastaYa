using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface IBidRepository
    {
        public Task<Bid> AddAsync(Bid bid);
        public Task<List<Bid>> GetByAuctionIdAsync(int auctionId); // Acá hago el historial de ofertas en una subasta
        public Task<Bid?> GetHighestBidByAuctionIdAsync(int auctionId);
        public Task<bool> ExistsByAuctionIdAsync(int auctionId);
    }
}
