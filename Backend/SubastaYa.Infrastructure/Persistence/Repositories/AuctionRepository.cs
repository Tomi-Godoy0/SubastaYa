using Microsoft.EntityFrameworkCore;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Constants;
using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<Auction?> GetByIdAsync(int id)
        {
            return await _context.Auctions.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Auction?> GetByIdTrackingAsync(int id)
        {
            return await _context.Auctions.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Auctions.AnyAsync(a => a.Id == id);
        }

        public async Task<List<Auction>> GetActiveAsync(DateTime now)
        {
            return await _context.Auctions
                .Where(a => a.Status == AuctionConstants.Active)
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

        public async Task<List<Auction>> GetBySellerIdAsync(int sellerId)
        {
            return await _context.Auctions.Where(a => a.SellerId == sellerId).ToListAsync();
        }
        public Task<Auction> UpdateAsync(Auction auction)
        {
            _context.Auctions.Update(auction);

            return Task.FromResult(auction);
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
                query = query.Where(a => a.BasePrice >= f.MinPrice);

            if (f.MaxPrice.HasValue)
                query = query.Where(a => a.BasePrice <= f.MaxPrice);

            //Ordeno
            query = f.OrderBy switch
            {
                "timeRemaining" => query.OrderBy(a => a.EndDate),
                "highestBid" => query.OrderByDescending(a => a.BasePrice), //Todavia no tenemos un campo donde tengamos la puja mayor
                _ => query.OrderBy(a => a.Id)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                        .Skip((pageNumber - 1) * pageSize) // Salta a otra página
                        .Take(pageSize) // Limita
                        .ToListAsync();

            return (items, totalCount);
        }
    }
}
