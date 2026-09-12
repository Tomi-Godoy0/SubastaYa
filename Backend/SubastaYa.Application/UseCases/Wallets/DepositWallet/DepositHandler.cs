using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Wallets;
using SubastaYa.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Wallets.DepositWallet
{
    public class DepositHandler : IDepositHandler
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepositHandler(IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletResponse> HandleAsync(DepositCommand command)
        {
            var wallet = await _walletRepository.GetByUserIdTrackedAsync(command.UserId)
            ?? throw new NotFoundException("Usuario no encontrado");

            wallet.TotalBalance += command.Amount;

            await _walletRepository.UpdateAsync(wallet);

            //Historial de transacciones

            await _unitOfWork.SaveChangesAsync();

            return new WalletResponse
            {
                TotalBalance = wallet.TotalBalance,
                HeldBalance = wallet.HeldBalance,
                AvailableBalance = wallet.TotalBalance - wallet.HeldBalance
            };
        }
    }
}
