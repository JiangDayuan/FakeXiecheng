using FakeXiecheng.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FakeXiecheng.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult login([FromBody] LoginDto loginDto)
        {
            // 1 验证用户名和密码

            // 2 创建JWT Token
            // header
            var signingAlgrorithm = SecurityAlgorithms.HmacSha256;
            // payload
            var claims = new[]
            {
                // sub
                new Claim(JwtRegisteredClaimNames.Sub, "fake_user_id")
            };
            // signiture
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingCredential = new SigningCredentials(signingKey, signingAlgrorithm);

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audiance"],
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredential
                );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            // 3 return 200ok + jwt
            return Ok(tokenStr);
        }
    }
}
