using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Wallets.DepositWallet;

namespace SubastaYa.Application.Interfaces.Service.Wallets
{
    public interface IDepositHandler
    {
        Task<WalletResponse> HandleAsync(DepositCommand command);
    }
}