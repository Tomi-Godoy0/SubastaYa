using SubastaYa.Application.Interfaces.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces
{
    public interface IAuctionNotifier
    {
        Task NotifyNewBidAsync(int auctionId, NewBidPayload payload);
        Task NotifyAuctionExtendedAsync(int auctionId, DateTime newEndDate);
    }
}
