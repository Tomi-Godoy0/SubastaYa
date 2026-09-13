using Microsoft.EntityFrameworkCore;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<Bid?> GetByIdAsync(int id)
        {
            return await _context.Bids.FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<int> CountByAuctionIdAsync(int auctionId)
        {
            return await _context.Bids.CountAsync(b => b.AuctionId == auctionId);
        }
        public async Task<List<Bid>> GetByAuctionIdAsync(int auctionId)
        {
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.CreatedAt) // Voy a querer el más reciente primero
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
