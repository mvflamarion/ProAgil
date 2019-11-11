using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProAgil.Domain.Identity;
using ProAgil.WebAPI.ViewModels;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserController(IConfiguration config,
                              UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IMapper mapper)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet("getUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            return Ok(new UserVm());
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserVm userVm)
        {
            try
            {
                var user = _mapper.Map<User>(userVm);

                var result = await _userManager.CreateAsync(user, userVm.Password);

                var userToReturn = _mapper.Map<UserVm>(user);

                if (result.Succeeded)
                    return Created("getUser", userToReturn);

                return BadRequest(result.Errors);               
                
            }
            catch (System.Exception ex)
            {
               return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou {ex.Message}");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginVm userlogin)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userlogin.Email);

                var result = await _signInManager.CheckPasswordSignInAsync(user, userlogin.Password, false);

                if (!result.Succeeded)
                    return Unauthorized();

                var appUser = await _userManager.Users
                    .FirstOrDefaultAsync( u => u.NormalizedEmail == userlogin.Email.ToUpper());    

                var userToReturn = _mapper.Map<UserLoginVm>(appUser);

                return Ok(new {
                    token = GenerateJWToken(appUser).Result,
                    user = userToReturn
                });
                
            }
            catch (System.Exception ex)
            {
               return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou {ex.Message}");
            }
        }

        private async Task<string> GenerateJWToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(_config.GetSection("AppSettings:Token").Value));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}