using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TodosApi.Models;
using TodosApi.Services;
using TodosApi.Data;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TodoDbContext _context;
    private readonly TokenService _tokenService;
    private readonly PasswordHasher<User> _encryptPassword = new PasswordHasher<User>();

    public AuthController(TodoDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        if (_context.Users.Any(u => u.Username == user.Username))
            return BadRequest("Username already exists.");

        user.Password = _encryptPassword.HashPassword(user, user.Password);
        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login(User login)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == login.Username);
        if (user == null)
            return Unauthorized();

        var result = _encryptPassword.VerifyHashedPassword(user, user.Password, login.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized();

        var token = _tokenService.CreateToken(user);
        return Ok(new { token });
    }
}
