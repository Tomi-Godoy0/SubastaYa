using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Security;
using SubastaYa.Application.Interfaces.Service.Users;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Exceptions;

namespace SubastaYa.Application.UseCases.Users.CreateUser
{
    public class CreateUserHandler : ICreateUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserHandler(IUserRepository userRepository, IWalletRepository walletRepository, IUnitOfWork unitOfWork ,IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> HandleAsync(CreateUserCommand command)
        {
            if (await _userRepository.ExistsByEmailAsync(command.Email))
                throw new ConflictException("Ya existe un usuario con ese email");

            var newUser = new User
            {
                Email = command.Email,
                Name = command.Name,
                PasswordHash = _passwordHasher.Hash(command.Password)
            };
            await _userRepository.AddAsync(newUser);

            var wallet = new Wallet
            {
                User = newUser,
                TotalBalance = 0,
                HeldBalance = 0
            };
            await _walletRepository.AddAsync(wallet);

            await _unitOfWork.SaveChangesAsync();

            return newUser.Id;
        }
    }
}
