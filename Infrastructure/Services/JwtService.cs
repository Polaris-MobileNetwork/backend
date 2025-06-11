using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models.Authentication;
using Application.Interfaces.IServices;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSetting jwtSetting;
        private readonly SymmetricSecurityKey key;
        public JwtService(IOptions<JwtSetting> options)
        {
            jwtSetting = options.Value;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SigninKey));
        }
        public string GenerateToken(User user)
        {
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSetting.ExpirationMinutes),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
