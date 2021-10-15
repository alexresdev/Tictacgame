using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services
{
    public class UserService : IUserService
    {
        public Task<bool> RegisterUser(UserModel userModel)
        {
            return Task.FromResult(true);
        }
        public Task<bool> IsOnline(string name)
        {
            return Task.FromResult(true);
        }
    }
}
