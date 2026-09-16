using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Wallets;
using SubastaYa.Domain.Constants;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Exceptions;

namespace SubastaYa.Application.UseCases.Wallets.DepositWallet
{
    public class DepositHandler : IDepositHandler
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionLedgerRepository _transactionLedgerRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepositHandler(
            IWalletRepository walletRepository, 
            IUnitOfWork unitOfWork, 
            ITransactionLedgerRepository transactionLedgerRepository, 
            IAuditLogRepository auditLogRepository)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _transactionLedgerRepository = transactionLedgerRepository;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<WalletResponse> HandleAsync(DepositCommand command)
        {
            var wallet = await _walletRepository.GetByUserIdTrackedAsync(command.UserId)
            ?? throw new NotFoundException("Usuario no encontrado");

            var previousBalance = wallet.TotalBalance;

            wallet.TotalBalance += command.Amount;

            var auditEntry = new AuditLog
            {
                Entity = "BILLETERA",
                EntityId = wallet.Id,
                Action = "depositar",
                UserId = command.UserId,
                DetailJson = System.Text.Json.JsonSerializer.Serialize(new { lastTotalBalance = previousBalance, newTotalBalance = wallet.TotalBalance, amount = command.Amount }),
                CreatedAt = DateTime.UtcNow
            };
            await _auditLogRepository.AddAsync(auditEntry);

            var ledgerEntry = new TransactionLedger
            {
                WalletId = wallet.Id,
                Type = TransactionConstants.Deposit,
                Amount = command.Amount,
                CreatedAt = DateTime.UtcNow
            };
            await _transactionLedgerRepository.AddAsync(ledgerEntry);

            await _unitOfWork.SaveChangesAsync();

            return new WalletResponse
            {
                TotalBalance = wallet.TotalBalance,
                HeldBalance = wallet.HeldBalance,
                AvailableBalance = wallet.AvailableBalance
            };
        }
    }
}
