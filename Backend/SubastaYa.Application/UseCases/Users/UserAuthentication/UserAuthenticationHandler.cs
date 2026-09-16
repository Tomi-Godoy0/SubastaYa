using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Security;
using SubastaYa.Application.Interfaces.Service.Users;

namespace SubastaYa.Application.UseCases.Users.UserAuthentication
{
    public class UserAuthenticationHandler : IUserAuthenticationHandler 
    {

        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserAuthenticationHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> HandleAsync(UserAuthenticationQuery query)
        {
            var user = await _userRepository.GetByEmailAsync(query.Email)
                ??throw new UnauthorizedAccessException("Credenciales inválidas");

            bool valid = _passwordHasher.Verify(query.Password, user.PasswordHash);

            if (!valid)
                throw new UnauthorizedAccessException("Credenciales inválidas");

            return user.Id;
        }
    }
}