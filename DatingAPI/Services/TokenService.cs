using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingAPI.Entities;
using DatingAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace DatingAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _userManager = userManager;
        }

        public async Task<string> CreateToken(AppUser appUser)
        {
           var claims = new List<Claim> 
           {
               new Claim(JwtRegisteredClaimNames.NameId,appUser.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.UniqueName,appUser.UserName)
           };
           var roles = await _userManager.GetRolesAsync(appUser);
           claims.AddRange(roles.Select(role=> new Claim(ClaimTypes.Role,role)));
           var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);
           var tokenDescriptor = new SecurityTokenDescriptor
           {
               Subject = new ClaimsIdentity(claims),
               Expires = DateTime.Now.AddDays(10),
               SigningCredentials= creds
           };
           var tokenHandler = new JwtSecurityTokenHandler();
           var token = tokenHandler.CreateToken(tokenDescriptor);
           return tokenHandler.WriteToken(token);
        }
    }
}