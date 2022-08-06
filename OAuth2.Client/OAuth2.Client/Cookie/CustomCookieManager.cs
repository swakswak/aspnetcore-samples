using Microsoft.AspNetCore.Authentication.Cookies;

namespace OAuth2.Client.Cookie;

public class CustomCookieManager : ICookieManager
{
    public string? GetRequestCookie(HttpContext context, string key)
    {
        Console.WriteLine($"[GetRequestCookie] key={key}, {key}={context.Request.Cookies[key]}");
        return context.Request.Cookies[key];
    }

    public void AppendResponseCookie(HttpContext context, string key, string? value, CookieOptions options)
    {
        Console.WriteLine($"[AppendResponseCookie] context={context}, key={key}, value={value}, options={options}");
        context.Request.Cookies.Append(new KeyValuePair<string, string>("accessToken", "a"));
    }

    public void DeleteCookie(HttpContext context, string key, CookieOptions options)
    {
        Console.WriteLine($"[DeleteCookie] context={context} key={key}, options={options}");
        context.Response.Cookies.Delete(key);
    }
}