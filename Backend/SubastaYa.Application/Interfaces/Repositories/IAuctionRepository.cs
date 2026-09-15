using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface IAuctionRepository
    {
        //Commands
        Task<Auction> AddAsync(Auction auction);
        Task<Auction> UpdateAsync(Auction auction);

        //Queries
        Task<Auction?> GetByIdAsync(int id); //no-tracking
        Task<Auction?> GetByIdWithBidsAsync(int id);
        Task<List<Auction>> GetBySellerIdAsync(int sellerId);

        // Concurrencia y Tracking
        Task<Auction?> GetByIdTrackingAsync(int id); // Tracking para modificar y guardar.


        //Filtros y Worker
        Task<(List<Auction> Items, int TotalCount)> GetFilterAsync(AuctionFilter filter, int pageNumber, int pageSize);
        Task<List<Auction>> GetExpiredAsync(DateTime now); // Esto lo usamos para el worker que cierra subastas vencidas
        Task<List<Auction>> GetReadyToStartAsync(DateTime now);
        
        Task<bool> ExistsAsync(int id);
    }
}
