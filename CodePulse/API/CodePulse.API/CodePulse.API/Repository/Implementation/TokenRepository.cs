using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CodePulse.API.Repository.Implementation;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration _configuration;

    public TokenRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string CreateJwtToken(IdentityUser user, List<string> roles)
    {
        
        //Creamos los Claims (declaraciones que contienen informaciones, como los roles)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        //creamos los parametros de seguridad del JWT
        var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // creando las credenciales del token
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}