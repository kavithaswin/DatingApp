using System.Security.Claims;

namespace DatingAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user){
            return  user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}