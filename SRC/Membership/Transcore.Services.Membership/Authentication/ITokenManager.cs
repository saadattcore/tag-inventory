using System.Collections.Generic;

namespace Transcore.Services.Membership.Authentication
{
    public interface ITokenManager
    {
        string GenerateToken();
        string GenerateToken(Dictionary<string, string> claims);
    }
}
