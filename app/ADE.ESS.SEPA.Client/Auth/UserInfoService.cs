using ADE.ESS.SEPA.Application.Interfaces;

namespace ADE.ESS.SEPA.Client.Auth
{

    public class UserInfoService : IUserInfoService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IESSUserSecurityManager _securityManager;
        public UserInfoService(IHttpContextAccessor context, IESSUserSecurityManager securityManager)
        {
            _context = context;
            _securityManager = securityManager;
        }

        public string GetUser()
        {
            var httpContext = _context.HttpContext;
            if (httpContext == null || httpContext.User == null)
                return "Anonymous";

            //var loggedInUserId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var loggedInUser = await _userManager.FindByIdAsync(loggedInUserId);

            var userId = _securityManager.GetEmail();

            if (string.IsNullOrEmpty(userId))
                return "Anonymous";

            // If you need to fetch the complete user object, uncomment the following line
            // var loggedInUser = await _userManager.FindByIdAsync(userId);

            //if (user.Identity?.IsAuthenticated == true)
            //    return user.FindFirstValue(ClaimTypes.Email) ?? user.Identity.Name;

            return userId ?? "Anonymous";
        }
    }
}


