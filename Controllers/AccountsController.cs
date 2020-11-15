using NotesAPI.Dtos.Account;
using NotesAPI.Models;
using NotesAPI.Services.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NotesAPI.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        private readonly IAuthService _authService;
        public AccountsController(IAuthService authService)
        {
            _authService = authService;
        }

        // api/Accounts/Register
        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(LoginUserDto newUser)
        {
            ServiceResponse<GetUserDto> response = await _authService.Register(newUser);
            return Ok(response);
        }


        //api/Accounts/Login
        [Route("login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginUserDto user)
        {
            ServiceResponse<string> response = await _authService.Login(user);
            return Ok(response);
        }
    }
}
