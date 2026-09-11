using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Wallets.DepositWallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Wallets
{
    public interface IDepositHandler
    {
        Task<WalletResponse> HandleAsync(DepositCommand command);
    }
}
