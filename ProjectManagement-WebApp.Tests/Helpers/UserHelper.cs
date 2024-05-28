using System;
using System.Collections.Generic;
using System.Security.Claims;

public static class UserHelper
{
    public static Guid? GetUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : (Guid?)null;
    }

    public static void SetUserId(Guid userId)
    {
        // Testler için geçici bir kullanıcı oluşturma ve ayarlama
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        var claimsPrincipal = new Moq.Mock<ClaimsPrincipal>();
        claimsPrincipal.Setup(c => c.FindFirst(ClaimTypes.NameIdentifier)).Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
        // HttpContext.User property can be set in ControllerContext in the tests
    }
}
