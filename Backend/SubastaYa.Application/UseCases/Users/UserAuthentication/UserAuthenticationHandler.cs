using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Security;
using SubastaYa.Application.Interfaces.Service.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Users.UserAuthentication
{
    public class UserAuthenticationHandler : IUserAuthenticationHandler 
    {

        private readonly IUserRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserAuthenticationHandler(IUserRepository usuarioRepository, IPasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> HandleAsync(UserAuthenticationQuery query)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(query.Email)
            ??throw new UnauthorizedAccessException("Credenciales inválidas");

            bool valido = _passwordHasher.Verify(query.Password, usuario.PasswordHash);

            if (!valido) { throw new UnauthorizedAccessException("Credenciales inválidas"); }

            return usuario.Id;
        }
    }
}
