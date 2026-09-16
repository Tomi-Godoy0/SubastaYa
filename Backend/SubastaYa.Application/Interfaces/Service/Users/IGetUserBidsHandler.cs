using SubastaYa.Application.Common;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Users.GetMyBids;

namespace SubastaYa.Application.Interfaces.Service.Users
{
    public interface IGetUserBidsHandler
    {
        public Task<PagedResult<UserBidsResponse>> HandleAsync(GetUserBidsQuery query);
    }
}
