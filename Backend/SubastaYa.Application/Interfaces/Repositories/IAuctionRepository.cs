using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface IAuctionRepository
    {
        //Commands
        Task<Auction> AddAsync(Auction auction);

        //Queries
        Task<Auction?> GetByIdWithBidsAsync(int id);
        Task<(List<Auction> Items, int TotalCount)> GetBySellerIdAsync(int sellerId, int pageNumber, int pageSize);

        // Concurrencia y Tracking
        Task<Auction?> GetByIdTrackingAsync(int id);


        //Filtros y Worker
        Task<(List<Auction> Items, int TotalCount)> GetFilterAsync(AuctionFilter filter, int pageNumber, int pageSize);
        Task<List<Auction>> GetExpiredAsync(DateTime now);
        Task<List<Auction>> GetReadyToStartAsync(DateTime now);
    }
}
