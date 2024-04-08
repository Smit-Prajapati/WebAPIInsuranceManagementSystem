using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPIInsuranceManagementSystem.DataAccess.Models;
using WebAPIInsuranceManagementSystem.DataAccess.Repositories.IRepositories;
using WebAPIInsuranceManagementSystem.Services.DTOs;
using WebAPIInsuranceManagementSystem.Services.Services.IServices;

namespace WebAPIInsuranceManagementSystem.Services.Services
{
    public class AuthService : Controller,IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<int> Register(UserDTO userDTO)
        {
            try
            {
                if (await _authRepository.UserExists(userDTO.Email))
                {
                    return 1;
                }

                User user = ConvertToUser(userDTO);

                User registeredUser = await _authRepository.Register(user);

                if (registeredUser != null)
                {        
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RegisterAsync: {ex.Message}");
                return 3;
            }
        }

        public async Task<string> Login(string email, string password)

        {
            try
            {
                User user = await _authRepository.Login(email, password);

                if (user != null)
                {
                    string token = GenerateJwtToken(user.Id);

                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoginAsync: {ex.Message}");
                return null;
            }
        }



        //******************************************************** Generate Token method *************************************************************
        private string GenerateJwtToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("MyTa4tkEkKbI32VgCawxsKc1UQmugAqm7bnPT8P+twQ=");

            var signingKey = new SymmetricSecurityKey(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, userId.ToString())
                 }),

                Expires = DateTime.UtcNow.AddHours(168),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        //******************************************************** Convert Model Methods *************************************************************
        public UserDTO ConvertToUserDTO(User user)
        {
           
            UserDTO userDTO = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
                IsApproved = user.IsApproved,
                IsActive = user.IsActive
            };

            return userDTO;
        }
        public User ConvertToUser(UserDTO userDTO)
        {
            User user = new User
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Password = userDTO.Password, 
                PhoneNumber = userDTO.PhoneNumber,
                RoleId = userDTO.RoleId,
                IsApproved = userDTO.IsApproved,
                IsActive = userDTO.IsActive
            };

            return user;
        }



    }
}
