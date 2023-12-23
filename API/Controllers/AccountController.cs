using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly DataContext _context;

    public AccountController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
    {
        if (await CheckIfUserExists(registerDto.Username))
            return BadRequest("Username already registered!");

        // "using" to release memory after method finishes
        // hmac == hashing algorithm
        using var hmac = new HMACSHA512();

        var newUser = new AppUser
        {
            Username = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key //randomly generated key when creating hmac class
        };

        _context.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser;

    }

    private async Task<bool> CheckIfUserExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
    }
}
