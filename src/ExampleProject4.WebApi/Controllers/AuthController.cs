using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExampleProject4.WebApi.Controllers;

public record LoginModel(string Username, string Password);
public record GetTokenResponse(string Token);

[ApiController]
[Route("/api/auth")]
public class AuthController: ControllerBase
{
    private readonly IConfiguration _configuration;
    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("token")]
    public ActionResult<GetTokenResponse> GetToken(LoginModel model)
    {
        if (model.Username == "admin" && model.Password == "admin") 
        {
            var secret = _configuration.GetValue<string>("Jwt:Secret");
            var audience = _configuration.GetValue<string>("Jwt:Audience");
            var issuer = _configuration.GetValue<string>("Jwt:Issuer");
            byte[] jwtSecret = Encoding.UTF8.GetBytes(secret);
            var key = new SymmetricSecurityKey(jwtSecret);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "admin"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Audience = audience,
                Issuer = issuer,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(tokenDescriptor);
            GetTokenResponse response = new GetTokenResponse(handler.WriteToken(token));

            CookieOptions developmentOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Domain = "localhost"
            };

            HttpContext.Response.Cookies.Append("Authorization", response.Token, developmentOptions);

            return Ok(response);
        }

        return BadRequest();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe()
    {
        return Ok(HttpContext.User.Claims.Select(c => new { c.Type, c.Value }).ToList());
    }
}
