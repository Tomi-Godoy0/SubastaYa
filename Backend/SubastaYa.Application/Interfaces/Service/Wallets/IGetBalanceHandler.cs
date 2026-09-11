using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Wallets.BalanceWallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Wallets
{
    public interface IGetBalanceHandler
    {
        public Task<WalletResponse> HandleAsync(GetBalanceQuery query);
    }
}
