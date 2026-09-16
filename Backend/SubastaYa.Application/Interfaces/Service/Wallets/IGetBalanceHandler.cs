using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Wallets.BalanceWallet;

namespace SubastaYa.Application.Interfaces.Service.Wallets
{
    public interface IGetBalanceHandler
    {
        public Task<WalletResponse> HandleAsync(GetBalanceQuery query);
    }
}
