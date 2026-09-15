using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Interfaces.Service.LedgerTransactions;
using SubastaYa.Application.UseCases.LedgerTransactions.CreateTransaction;
using SubastaYa.Application.UseCases.LedgerTransactions.GetTransaction;
using SubastaYa.Application.UseCases.LedgerTransactions.GetTransactions;

namespace SubastaYa.API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IGetTransactionsHandler _getTransactionsHandler;
        private readonly ICreateTransactionHandler _createTransactionHandler;
        private readonly IGetTransactionHandler _getTransactionHandler;

        public TransactionController(
            IGetTransactionHandler getTransactionHandler,
            IGetTransactionsHandler getTransactionsHandler,
            ICreateTransactionHandler createTransactionHandler)
        {
            _getTransactionHandler = getTransactionHandler;
            _getTransactionsHandler = getTransactionsHandler;
            _createTransactionHandler = createTransactionHandler;
        }

        [HttpGet("wallet/{walletId}")]
        public async Task<IActionResult> GetTransactions(int walletId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var transactions = await _getTransactionsHandler.HandleAsync(
                new GetTransactionsQuery
                {
                    WalletId = walletId,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _getTransactionHandler.HandleAsync(
                new GetTransactionQuery
                {
                    Id = id
                });

            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }
    }
}