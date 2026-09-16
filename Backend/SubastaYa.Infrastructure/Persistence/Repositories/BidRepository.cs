using Microsoft.EntityFrameworkCore;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Infrastructure.Persistence.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly AppDbContext _context;

        public BidRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Bid> AddAsync(Bid bid)
        {
            await _context.Bids.AddAsync(bid);

            return bid;
        }
        public async Task<bool> ExistsByAuctionIdAsync(int auctionId)
        {
            return await _context.Bids.AnyAsync(b => b.AuctionId == auctionId);
        }
        public async Task<List<Bid>> GetByAuctionIdAsync(int auctionId)
        {
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }
        public async Task<Bid?> GetHighestBidByAuctionIdAsync(int auctionId)
        {
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.Amount)
                .FirstOrDefaultAsync();
        }
    }
}
