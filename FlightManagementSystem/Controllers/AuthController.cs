﻿using FlightManagementSystem.Models;
using FlightManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightManagementSystem.Controllers
{
    [Route("auth")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public ActionResult RegisterUser([FromBody] UserSignupDto dto)
        {
            _authService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] UserLoginDto dto)
        {
            string token = _authService.LoginUser(dto);
            return Ok(token);
        }

        [HttpGet("account")]
        public ActionResult Account()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            int.TryParse(currentUserName, out int id);

            var response = _authService.Account(id);
            return Ok(response);
        }
    }
}
