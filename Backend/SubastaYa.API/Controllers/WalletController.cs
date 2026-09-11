using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Interfaces.Service.Wallets;
using SubastaYa.Application.UseCases.Wallets.BalanceWallet;
using SubastaYa.Application.UseCases.Wallets.DepositWallet;

namespace SubastaYa.API.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IGetBalanceHandler _getBalanceHandler;
        private readonly IDepositHandler _depositHandler;

        public WalletController(IGetBalanceHandler getBalanceHandler, IDepositHandler depositHandler)
        {
            _getBalanceHandler = getBalanceHandler;
            _depositHandler = depositHandler;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance(int id)
        {
            var balance = await _getBalanceHandler.HandleAsync(new GetBalanceQuery { UserId = id });

            return Ok(balance);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> CreateDeposit(DepositCommand command)
        {
            var newBalance = await _depositHandler.HandleAsync(command);

            return Ok(newBalance);
        }
    }
}
