using Microsoft.AspNetCore.Identity;

namespace APIPractice.Repository.IRepository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, string role);
    }
}
