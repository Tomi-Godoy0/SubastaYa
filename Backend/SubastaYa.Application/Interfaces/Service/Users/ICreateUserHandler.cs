using SubastaYa.Application.UseCases.Users.CreateUser;
using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Users
{
    public interface ICreateUserHandler
    {
        public Task<int> HandleAsync(CreateUserCommand command);
    }
}
