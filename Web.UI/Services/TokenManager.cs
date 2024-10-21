public interface ITokenManager
{
    string GetToken();
    void SetToken(string token);
}

public class TokenManager : ITokenManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string TokenCookieName = "JWTToken";

    public TokenManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetToken()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies[TokenCookieName];
    }

    public void SetToken(string token)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            if (string.IsNullOrEmpty(token))
            {
                httpContext.Response.Cookies.Delete(TokenCookieName);
            }
            else
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1) // Set expiration time
                };
                httpContext.Response.Cookies.Append(TokenCookieName, token, cookieOptions);
            }
        }
    }
}