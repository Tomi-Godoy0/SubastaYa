using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Users.GetUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Users
{
    public interface IGetUserHandler
    {
        public Task<UserResponse> HandleAsync(GetUserQuery query);
    }
}
