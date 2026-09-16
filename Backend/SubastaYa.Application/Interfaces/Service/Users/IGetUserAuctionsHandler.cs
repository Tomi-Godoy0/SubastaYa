using SubastaYa.Application.Common;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Users.GetMyAuctions;

namespace SubastaYa.Application.Interfaces.Service.Users
{
    public interface IGetUserAuctionsHandler
    {
        public Task<PagedResult<UserAuctionsResponse>> HandleAsync(GetUserAuctionsQuery query);
    }
}
