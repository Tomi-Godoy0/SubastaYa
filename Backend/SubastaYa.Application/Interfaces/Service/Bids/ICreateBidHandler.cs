using SubastaYa.Application.UseCases.Bids.CreateBid;

namespace SubastaYa.Application.Interfaces.Service.Bids
{
    public interface ICreateBidHandler
    {
        public Task<int> HandleAsync(CreateBidCommand command);
    }
}
