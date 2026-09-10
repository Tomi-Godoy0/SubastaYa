using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Bids.CreateBid
{
    public class CreateBidHandler
    {
        public CreateBidHandler()
        {
        }

        public async Task<Bid> HandleAsync(CreateBidCommand command)
        {
            if (command.AuctionId <= 0)
                throw new ArgumentException(
                    "La subasta no es válida.");

            if (command.BuyerId <= 0)
                throw new ArgumentException(
                    "El comprador no es válido.");

            if (command.Amount <= 0)
                throw new ArgumentException(
                    "El monto de la puja debe ser mayor a cero.");

            if (command.CreatedAt == default)
                command.CreatedAt = DateTime.UtcNow;

            var bid = new Bid
            {
                AuctionId = command.AuctionId,
                BuyerId = command.BuyerId,
                Amount = command.Amount,
                CreatedAt = command.CreatedAt
            };

            return await Task.FromResult(bid);
        }
    }
}