﻿using Application.Models;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserService
    {
        Task<bool> IsOnline(string name);
        Task<bool> RegisterUser(UserModel userModel);
    }
}