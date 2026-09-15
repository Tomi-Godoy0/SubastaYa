using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.LedgerTransactions;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.LedgerTransactions.CreateTransaction
{
    public class CreateTransactionHandler : ICreateTransactionHandler
    {
        private readonly ITransactionLedgerRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTransactionHandler(ITransactionLedgerRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionLedger> HandleAsync(CreateTransactionCommand command)
        {
            var transaction = new TransactionLedger
            {
                WalletId = command.WalletId,
                Type = command.Type.Trim(),
                Amount = command.Amount,
                CreatedAt = DateTime.UtcNow,
                AuctionId = command.AuctionId
            };

            await _transactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            return transaction;
        }
    }
}