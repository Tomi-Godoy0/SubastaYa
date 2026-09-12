using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Wallets;
using SubastaYa.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Wallets.BalanceWallet
{
    public class GetBalanceHandler : IGetBalanceHandler
    {
        private readonly IWalletRepository _walletRepository;

        public GetBalanceHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletResponse> HandleAsync(GetBalanceQuery query)
        {
            var wallet = await _walletRepository.GetByUserIdAsync(query.UserId)
             ?? throw new NotFoundException("Billetera no encontrada");

            return new WalletResponse
            {
                TotalBalance = wallet.TotalBalance,
                HeldBalance = wallet.HeldBalance,
                AvailableBalance = wallet.AvailableBalance
            };
        }
    }
}
