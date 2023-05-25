using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APIzza.Security
{
    public class JwtGenerator : ITokenGenerator
    {
        private readonly byte[] JwtSecret;

        public JwtGenerator(string secret)
        {
            JwtSecret = GenerateKey(secret);
        }

        public string GenerateToken(int userId, string username)
        {
            return GenerateToken(userId, username, string.Empty);
        }

        public string GenerateToken(int userId, string username, string role)
        {
            var claims = new List<Claim>()
            {
                new Claim("sub", userId.ToString()),
                new Claim("name", username),
            };

            if (!string.IsNullOrWhiteSpace(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(JwtSecret), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = signingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private byte[] GenerateKey(string secret)
        {
            using var sha256 = SHA256.Create();
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            return sha256.ComputeHash(secretBytes);
        }
    }
}
