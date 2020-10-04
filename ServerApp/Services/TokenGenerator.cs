using System;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ServerApp.Services
{
    public class TokenGenerator
    {
        public bool ValidateIssuer { get; }
        public string ValidIssuer { get; }
        public bool ValidateAudience { get; }
        public string ValidAudience { get; }
        public bool ValidateIssuerSigningKey => true;
        public SymmetricSecurityKey IssuerSigningKey { get; }
        public bool ValidateLifetime { get; }
        public TimeSpan ClockSkew { get; }

        private readonly JwtSecurityTokenHandler tokenHandler;

        public TokenGenerator(
            string issuerSigningKey,
            bool validateIssuer = true,
            string validIssuer = "Issuer",
            bool validateAudience = true,
            string validAudience = "Audience",
            int clockSkewMinutes = 4320,
            bool validateLifetime = true)
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(issuerSigningKey));
            ValidateIssuer = validateIssuer;
            ValidIssuer = validIssuer;
            ValidateAudience = validateAudience;
            ValidAudience = validAudience;
            ValidateLifetime = validateLifetime;
            ClockSkew = TimeSpan.FromMinutes(clockSkewMinutes);
            tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateToken(string userName, IEnumerable<string> userRoles)
        {
            var claims = new ClaimsIdentity("Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            claims.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, userName));

            foreach (var role in userRoles)
            {
                claims.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = ValidIssuer,
                Audience = ValidAudience,
                SigningCredentials = new SigningCredentials(IssuerSigningKey, SecurityAlgorithms.HmacSha256Signature),
                Subject = claims,
                Expires = DateTime.UtcNow.Add(ClockSkew)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(string userName, params string[] userRoles)
        {
            return GenerateToken(userName, userRoles.AsEnumerable());
        }
    }

    public static class ServiceProviderExtensions
    {
        public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("JwtBearerTokenSettings");
            
            var tokenGenerator = new TokenGenerator(
                issuerSigningKey: jwtSection.GetValue<string>("IssuerSigningKey"),
                validIssuer: jwtSection.GetValue<string>("Issuer"),
                validAudience: jwtSection.GetValue<string>("Audience"),
                clockSkewMinutes: jwtSection.GetValue<int>("ClockSkewMinutes"));
            
            services.AddSingleton(tokenGenerator);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = tokenGenerator.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = tokenGenerator.ValidAudience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = tokenGenerator.IssuerSigningKey,
                    ValidateLifetime = true,
                    ClockSkew = tokenGenerator.ClockSkew
                };
            });
        }
    }
}
