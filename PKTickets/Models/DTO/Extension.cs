using System.Security.Claims;
using System.Security.Principal;

namespace PKTickets.Models
{
    public static class Extension
    {
        public static string GetClaimRole(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim role = claimsIdentity.FindFirst(ClaimTypes.Role);
            return role.Value;
        }
        public static string GetClaimName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim name = claimsIdentity.FindFirst(ClaimTypes.Name);
            return name.Value;
        }
        public static string GetClaimId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return id.Value;
        }
        public static string GetClaimEmail(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim email = claimsIdentity.FindFirst(ClaimTypes.Email);
            return email.Value;
        }
    }
}
