using SubastaYa.Application.UseCases.Users.UserAuthentication;

namespace SubastaYa.Application.Interfaces.Service.Users
{
    public interface IUserAuthenticationHandler
    {
        public Task<int> HandleAsync(UserAuthenticationQuery query);
    }
}
