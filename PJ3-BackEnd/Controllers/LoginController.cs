using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PJ3_BackEnd.Dtos;
using PJ3_BackEnd.Models;
using PJ3_BackEnd.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PJ3_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly profileContext _profileContext;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;

        public LoginController(profileContext profileContext, IConfiguration configuration, IPasswordService passwordService)
        {
            _profileContext = profileContext;
            _configuration = configuration;
            _passwordService = passwordService;

        }
        // POST api/<LoginController>
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto value)
        {
            
            var user = await (from a in _profileContext.auth
                        where a.email == value.email
                        select a).SingleOrDefaultAsync();
            if (user == null) 
            {
                return BadRequest("帳密錯誤");
            }

            else 
            {
                var verifyPassword = _passwordService.VerifyPassword(value.password, user.password);

                if (!verifyPassword)
                {
                    return BadRequest("帳密錯誤");
                }
                else 
                {
                    var claim = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,user.name),
                        new Claim(JwtRegisteredClaimNames.Email, user.email),
                        new Claim(ClaimTypes.Role,user.role),
                    };

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:KEY"]));
                    var jwt = new JwtSecurityToken
                        (
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claim,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                        );
                    var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                    return Ok(new
                    {
                        Token = token,
                        User = new
                        {
                            Id = user.id,   // 假設你有 id 欄位
                            Name = user.name,
                            Email = user.email,
                            Role = user.role
                        }
                    });
                }               
            }
        }
    }
}
