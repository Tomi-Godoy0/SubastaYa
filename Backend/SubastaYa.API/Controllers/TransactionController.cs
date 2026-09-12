using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Interfaces.Service.LedgerTransactions;
using SubastaYa.Application.UseCases.LedgerTransactions.GetTransactions;
using SubastaYa.Application.UseCases.LedgerTransactions.RegisterTransaction;

namespace SubastaYa.API.Controllers
{
    [Route("api/v1/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IGetTransactionsHandler _getTransactionsHandler;
        private readonly IRegisterTransactionHandler _registerTransactionHandler;

        public TransactionController(
            IGetTransactionsHandler getTransactionsHandler,
            IRegisterTransactionHandler registerTransactionHandler)
        {
            _getTransactionsHandler = getTransactionsHandler;
            _registerTransactionHandler = registerTransactionHandler;
        }

        [HttpGet("wallet/{walletId}")]
        public async Task<IActionResult> GetTransactions(int walletId)
        {
            var transactions = await _getTransactionsHandler.HandleAsync(
                new GetTransactionsQuery
                {
                    WalletId = walletId
                });

            return Ok(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTransaction(
            RegisterTransactionCommand command)
        {
            var transaction = await _registerTransactionHandler
                .HandleAsync(command);

            return CreatedAtAction(
                nameof(GetTransaction),
                new { id = transaction.Id },
                transaction);
        }

        [HttpGet("{id}")]
        public IActionResult GetTransaction(int id)
        {
            return Ok();
        }
    }
}