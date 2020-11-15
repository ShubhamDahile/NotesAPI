using NotesAPI.Dtos.Account;
using NotesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotesAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<GetUserDto>> Register(LoginUserDto newUser);
        Task<ServiceResponse<string>> Login(LoginUserDto user);
    }
}