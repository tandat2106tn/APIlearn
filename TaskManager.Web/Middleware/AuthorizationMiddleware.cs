using TaskManager.Web.Services;

namespace TaskManager.Web.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtService _jwtService;

        public AuthorizationMiddleware(RequestDelegate next, JwtService jwtService)
        {
            _next = next;
            _jwtService = jwtService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = _jwtService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                var principal = _jwtService.GetPrincipalFromToken(token);
                if (principal != null)
                {
                    context.User = principal;
                }
            }

            await _next(context);
        }
    }
}