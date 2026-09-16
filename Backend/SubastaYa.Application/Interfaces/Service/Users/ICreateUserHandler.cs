using SubastaYa.Application.UseCases.Users.CreateUser;

namespace SubastaYa.Application.Interfaces.Service.Users
{
    public interface ICreateUserHandler
    {
        public Task<int> HandleAsync(CreateUserCommand command);
    }
}
