using FlightManagementSystem.Entities;
using FlightManagementSystem.Middleware;
using FlightManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightManagementSystem.Services
{
    public interface IAuthService
    {
        void RegisterUser(RegisterUserDto dto);
        string LoginUser(LoginUserDto dto);
        User Account(int id);
    }

    public class AuthService : IAuthService
    {
        private readonly FlightManagementDbContext _context;
        private readonly AuthenticationSettings _authenticationSettings;

        public AuthService(FlightManagementDbContext context, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                throw new BadRequestException("Passwords are different");
            }

            var newUser = new User()
            {
                Email = dto.Email,
                FullName = dto.FullName,
                Password = dto.Password,
            };

            //var hashedPasword = _passwordHasher.HashPassword(newUser, dto.Password);

            //newUser.PasswordHash = hashedPasword;


            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public string LoginUser(LoginUserDto dto)
        {
            var user = _context
                .Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null || user.Password != dto.Password)
            {
                throw new BadRequestException("Invalid email or password");
            }

            //var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            //if (result == PasswordVerificationResult.Failed)
            //{
            //    throw new BadRequestException("Invalid email or password");
            //}

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FullName}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpiredays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public User Account(int id)
        {
            Console.WriteLine("Account "+ id);
            var user = _context
                .Users
                .FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                throw new BadRequestException("Invalid id");
            }
            return user;
        }
    }
}

