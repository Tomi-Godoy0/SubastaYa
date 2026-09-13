using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface IBidRepository
    {
        public Task<Bid> AddAsync(Bid bid);
        public Task<Bid?> GetByIdAsync(int id);
        public Task<List<Bid>> GetByAuctionIdAsync(int auctionId); // Acá hago el historial de ofertas en una subasta
        public Task<Bid?> GetHighestBidByAuctionIdAsync(int auctionId); // La puja lider actual
        public Task<int> CountByAuctionIdAsync(int auctionId); // Cantidad de ofertas
    }
}
