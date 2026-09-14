using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.LedgerTransactions;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.LedgerTransactions.RegisterTransaction
{
    public class RegisterTransactionHandler : IRegisterTransactionHandler
    {
        private readonly ITransactionLedgerRepository _transactionRepository;

        public RegisterTransactionHandler(
            ITransactionLedgerRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionLedger> HandleAsync(
            RegisterTransactionCommand command)
        {
            if (command.WalletId <= 0)
                throw new ArgumentException(
                    "La billetera no es válida.");

            if (string.IsNullOrWhiteSpace(command.Type))
                throw new ArgumentException(
                    "El tipo de transacción es obligatorio.");

            if (command.Amount <= 0)
                throw new ArgumentException(
                    "El monto debe ser mayor a cero.");

            if (command.CreatedAt == default)
                command.CreatedAt = DateTime.UtcNow;

            var transaction = new TransactionLedger
            {
                WalletId = command.WalletId,
                Type = command.Type.Trim(),
                Amount = command.Amount,
                CreatedAt = command.CreatedAt,
                AuctionId = command.AuctionId
            };

            return await _transactionRepository.AddAsync(transaction);
        }
    }
}