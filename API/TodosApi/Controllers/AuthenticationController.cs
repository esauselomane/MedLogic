using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodosApi.Data;
using TodosApi.Models;
using TodosApi.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TodoDbContext _context;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;
    private readonly PasswordHasher<User> _encryptPassword = new PasswordHasher<User>();

    public AuthController(TodoDbContext context, TokenService tokenService, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public IActionResult Register(UserViewModel user)
    {
        if (_context.Users.Any(u => u.Username == user.Username))
            return BadRequest("Username already exists.");

        var dbUser = _mapper.Map<User>(user);

        dbUser.Password = _encryptPassword.HashPassword(dbUser, user.Password);
        _context.Users.Add(dbUser);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login(UserViewModel login)
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
