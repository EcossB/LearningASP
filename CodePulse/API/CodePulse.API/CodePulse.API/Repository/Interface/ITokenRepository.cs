using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Repository.Interface;

public interface ITokenRepository
{
    string CreateJwtToken(IdentityUser user, List<string> roles);
}