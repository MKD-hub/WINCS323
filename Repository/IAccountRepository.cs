﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Model;

namespace WebProject.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);
        Task<List<string>> LoginAsync(SignInModel signInModel);

        Task<string> GetUserId(string UserName);
        Task<ApplicationUser> GetUser(string UserName);
    }
}
