﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIInsuranceManagementSystem.Services.DTOs;
using WebAPIInsuranceManagementSystem.Services.Services.IServices;

namespace WebAPIInsuranceManagementSystem.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            try
            {

                int result = await _authService.Register(userDTO);

                if (result == 1)
                {
                    return BadRequest("Email already exists.");
                }

                if (result == 2)
                {
                    return Ok("Registration successful");
                }
                else
                {
                    return BadRequest("Registration failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Register action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
     
                string token = await _authService.Login(loginDTO.Email, loginDTO.Password);

           
                if (token != null)
                { 
                    return Ok(new { token });
                }
                else
                {
                    return Unauthorized("Invalid email or password");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Login action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
