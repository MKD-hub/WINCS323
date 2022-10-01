using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Model;
using WebProject.Repository;

namespace WebProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromForm] SignUpModel signUpModel)
        {
            var result = await _accountRepository.SignUpAsync(signUpModel);

            if (result.Succeeded)
            {
                return Ok();
            }

            else { return Unauthorized(result.Errors.ToList()); }
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromForm] SignInModel signInModel)
        {
            var result = await _accountRepository.LoginAsync(signInModel);

            if (String.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
