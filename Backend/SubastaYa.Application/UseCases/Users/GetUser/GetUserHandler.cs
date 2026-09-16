using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Users;
using SubastaYa.Domain.Exceptions;

namespace SubastaYa.Application.UseCases.Users.GetUser
{
    public class GetUserHandler : IGetUserHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> HandleAsync(GetUserQuery query)
        {
            var user = await _userRepository.GetByIdAsync(query.UserId)
            ?? throw new NotFoundException($"Usuario {query.UserId} no encontrado");

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name
            };
        }
    }
}
