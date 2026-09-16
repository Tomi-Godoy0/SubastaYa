namespace SubastaYa.Application.Interfaces.Service.Worker
{
    public interface IAuctionStartingService
    {
        public Task<int> HandleAsync();
    }
}
