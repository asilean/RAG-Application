using System.Security.Claims;

namespace ProjectManagament_WebApp.Helpers
{
    public class UserHelper
    {
        public static Guid? GetUserId(ClaimsPrincipal user)
        {
            string userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(userIdValue, out Guid userId))
            {
                return userId;
            }

            return null;
        }
    }
}

