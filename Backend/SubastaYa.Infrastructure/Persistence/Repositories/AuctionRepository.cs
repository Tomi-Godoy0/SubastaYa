using Microsoft.EntityFrameworkCore;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Constants;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Infrastructure.Persistence.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AppDbContext _context;

        public AuctionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Auction> AddAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);

            return auction;
        }

        public async Task<(List<Auction> Items, int TotalCount)> GetBySellerIdAsync(int sellerId, int pageNumber, int pageSize)
        {
            IQueryable<Auction> query = _context.Auctions
                .AsNoTracking()
                .Where(a => a.SellerId == sellerId)
                .Include(a => a.Category)
                .Include(a => a.Bids);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Auction?> GetByIdTrackingAsync(int id)
        {
            return await _context.Auctions.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Auction>> GetReadyToStartAsync(DateTime now)
        {
            return await _context.Auctions
                .Where(a => a.Status == AuctionConstants.Scheduled && a.StartDate <= now)
                .ToListAsync();
        }
        public async Task<List<Auction>> GetExpiredAsync(DateTime now)
        {
            return await _context.Auctions
                .Where(a => a.Status == AuctionConstants.Active && a.EndDate <= now)
                .ToListAsync();
        }

        public async Task<Auction?> GetByIdWithBidsAsync(int id)
        {
            return await _context.Auctions
                .Include(a => a.Bids)
                .Include(a => a.Category)
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(List<Auction> Items, int TotalCount)> GetFilterAsync(AuctionFilter f ,int pageNumber, int pageSize)
        {
            IQueryable<Auction> query = _context.Auctions
                .AsNoTracking()
                .Include(a => a.Category)
                .Include(a => a.Bids);

            if (!string.IsNullOrEmpty(f.Title))
                query = query.Where(a => a.Title.Contains(f.Title));

            if (f.CategoryId.HasValue)
                query = query.Where(a => a.CategoryId == f.CategoryId.Value);

            if (!string.IsNullOrEmpty(f.Status))
                query = query.Where(a => a.Status == f.Status);

            if (f.MinPrice.HasValue)
                query = query.Where(a => a.CurrentBidAmount >= f.MinPrice);

            if (f.MaxPrice.HasValue)
                query = query.Where(a => a.CurrentBidAmount <= f.MaxPrice);

            //Ordeno
            query = f.OrderBy switch
            {
                "timeRemaining" => query.OrderBy(a => a.EndDate),
                "highestBid" => query.OrderByDescending(a => a.CurrentBidAmount),
                _ => query.OrderBy(a => a.Id)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            return (items, totalCount);
        }
    }
}
