using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OAuth2.Client.HeaderBased.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController
{
    [HttpPost]
    [AllowAnonymous]
    public LoginResponse Login([FromBody] OAuthCode code)
    {
        // TODO: 구현 필요
        return null;
    }
}