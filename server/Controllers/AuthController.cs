using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;
using TodoApi.Models;

namespace TodoApi.Controllers;

public record SignInRequest(string email, string password);
public record RegisterRequest(string username, string email, string password);
public record Response(bool IsSuccess, string Message);
public record UserClaim(string Type, string Value);

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _Authservice;
  private readonly IJWTManagerRepository _jwtService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public AuthController(IAuthService Authservice, IJWTManagerRepository jwtService, IHttpContextAccessor httpContextAccessor)
  {
    _Authservice = Authservice;
    _jwtService = jwtService;
    _httpContextAccessor = httpContextAccessor;
  }


  [HttpPost("register")]
  public IActionResult Register([FromBody] RegisterRequest body)
  {
    var user = _Authservice.Register(new User
    {
      username = body.username,
      email = body.email,
      password = body.password
    });
    if (user == null)
    {
      return BadRequest(new { status = 400, message = "Email Already Exists" });
    }
    var token = _jwtService.GenerateToken(user.Id.ToString()!);
    return Ok(new { status = 200, token, user, message = "User created successfully" });
  }

  [HttpPost("login")]
  public IActionResult Login([FromBody] SignInRequest body)
  {
    User? valid = _Authservice.Authenticate(body.email, body.password);
    if (valid != null)
    {
      var token = _jwtService.GenerateToken(valid.Id!.ToString()!);
      return Ok(new { token, user = valid, status = 200 });
    }
    return BadRequest(new { message = "Invalid credentials", status = 400 });
  }

  [HttpPost("logout")]
  public IActionResult Logout()
  {
    return Ok(new { status = 200, message = "Logout successfully" });
  }

  [HttpGet("me")]
  [Authorize]
  public IActionResult Me()
  {
    var user = _httpContextAccessor.HttpContext?.Items["User"] as User;
    return Ok(new { user, status = 200 });
  }
}
