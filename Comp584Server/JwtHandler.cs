using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WorldModel;

namespace Comp584Server
{
    public class JwtHandler(UserManager<WorldModelUser> userManager, IConfiguration configuration)
    {
        public async Task<JwtSecurityToken> GenerateTokenAsync(WorldModelUser user)
        {
            return new JwtSecurityToken
                (
                issuer: configuration["JwtSettings:validIssuer"],
                audience: configuration["JwtSettings:validAudience"],
                claims: await GetClaimsAsync(user),
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwstSettings:ExpiriyMinutes"])),
                signingCredentials: GetSigningCredentials()
                );
        }
        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Convert.FromBase64String(configuration["JwtSettings:SecretKey"]!);
            SymmetricSecurityKey signingKey = new (key);
            return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaimsAsync(WorldModelUser user)
        {

            List<Claim> claims = [new Claim(ClaimTypes.Name, user.UserName!)];
            //claims.AddRange((await userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role)));
            foreach (var role in await userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
