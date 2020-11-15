using AutoMapper;
using NotesAPI.Dtos.Account;
using NotesAPI.Models;
using NotesAPI.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NotesAPI.JwtTokenHelper;

namespace NotesAPI.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;

        public AuthService()
        {
            _context = new DataContext();
        }

        public async Task<ServiceResponse<string>> Login(LoginUserDto user)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            var userInDb = _context.Users.SingleOrDefault(u =>
                     u.UserName.ToLower() == user.UserName.ToLower() ||
                     u.Email.ToLower() == user.Email.ToLower()
            );
            if (userInDb == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Wrong Email Or Username";
            }
            else if(!await VerifyPasswordHash(user.Password, userInDb.Password, userInDb.PasswordSalt))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Wrong Password";
            }
            else
            {
                serviceResponse.Data = await CreateToken(Mapper.Map<User,GetUserDto>(userInDb));
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> Register(LoginUserDto newUser)
        {
            ServiceResponse<GetUserDto> serviceResponse = new ServiceResponse<GetUserDto>();

            var requiredFields = RequiredFields(newUser);
            if (!requiredFields.Success)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = requiredFields.Success;
                serviceResponse.Message = requiredFields.Message;

                return serviceResponse;
            }

            if (UserNameExists(newUser.UserName))
            {
                serviceResponse.Data = Mapper.Map<LoginUserDto,GetUserDto>(newUser);
                serviceResponse.Success = false;
                serviceResponse.Message = "UserName Already Exists";

                return serviceResponse;
            }
            
            if (EmailExists(newUser.Email))
            {
                serviceResponse.Data = Mapper.Map<LoginUserDto,GetUserDto>(newUser);
                serviceResponse.Success = false;
                serviceResponse.Message = "Email Already Exists";

                return serviceResponse;
            }

            HashPassword(newUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

            UserDto user = new UserDto()
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };            

            _context.Users.Add(Mapper.Map<UserDto,User>(user));
            await _context.SaveChangesAsync();

            var userInDb = _context.Users.SingleOrDefault(u => 
                    u.Email == user.Email &&
                    u.UserName == user.UserName
            );
            serviceResponse.Data = Mapper.Map<User,GetUserDto>(userInDb);
            return serviceResponse;
        }        

        private ServiceResponse<string> RequiredFields(LoginUserDto user)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            response.Data = "Error";
            if (user.UserName == null || user.UserName == "")
            {
                response.Success = false;
                response.Message = "UserName is required";
            }
            else if (user.Email == null || user.Email == "")
            {
                response.Success = false;
                response.Message = "Email is required";
            }
            else if (user.Password == null || user.Password == "")
            {
                response.Success = false;
                response.Message = "Password is required";
            }
            return response;
        }
        private bool UserNameExists(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName.ToLower() == username.ToLower());
            if (user != null)
            {
                return true;
            }
            return false;
        }
        private bool EmailExists(string email)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                return true;
            }
            return false;
        }

        private void HashPassword(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private async Task<bool> VerifyPasswordHash(string password,byte[] passwordHash,byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for(int i = 0; i < computedHash.Length; i++)
                {
                    if (passwordHash[i] != computedHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        private async Task<string> CreateToken(GetUserDto user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sec));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = GlobalConstants.JwtTokenIssuer,
                Audience = GlobalConstants.JwtTokenIssuer,
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);            
        }
    }
}