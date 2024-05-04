using System.Security.Claims;

namespace ProjectManagament_WebApp.Helpers
{
    public class UserHelper
    {
        public static Guid GetUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
