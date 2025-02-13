﻿using Library.Application.Interfaces.Security;
using Library.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Library.Infrastructure.Security
{
    public class JWTProvider : IJWTProvider
    {
        private readonly JWTOptions _options;

        public JWTProvider(IOptions<JWTOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateAccessToken(User user)
        {
            Claim[] claims =
            [
                new("userId", user.Id.ToString()),
                new("Role", user.Role.ToString())
            ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                expires: DateTime.Now.AddHours(_options.ExpiresHours)
            );

            var token = new JwtSecurityTokenHandler().WriteToken(accessToken);

            return token;
        }

        public string GenerateRefreshToken()
        {
            Claim[] claims =
            [
                new("body", Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))),
            ];

            var refreshToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(_options.RefreshExpiresDays)
            );

            var token = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            return token;
        }
    }
}
