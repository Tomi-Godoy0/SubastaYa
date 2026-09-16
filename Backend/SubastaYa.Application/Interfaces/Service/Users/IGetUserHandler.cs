using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Users.GetUser;

namespace SubastaYa.Application.Interfaces.Service.Users
{
    public interface IGetUserHandler
    {
        public Task<UserResponse> HandleAsync(GetUserQuery query);
    }
}
