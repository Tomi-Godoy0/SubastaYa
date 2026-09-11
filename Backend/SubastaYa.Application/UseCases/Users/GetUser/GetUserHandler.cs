using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Users;
using SubastaYa.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var user = await _userRepository.GetByIdAsync(query.UserId);

            if (user == null)
                throw new NotFoundException($"Usuario {query.UserId} no encontrado");

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name
            };
        }
    }
}
