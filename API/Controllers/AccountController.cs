using System.Security.Claims;
using API.DTOs;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : BaseController
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationIdentityDbContext _dbContext;
    private readonly ITokenGenerationService _tokenGenerationService;

    public AccountController(
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationIdentityDbContext dbContext,
        ITokenGenerationService tokenGenerationService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
        _tokenGenerationService = tokenGenerationService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDTO>> LoadUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }

        // Generate a JWT token with user claims
        var tokenClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),
                // Add more claims as needed
            };

        // Generate the JWT token
        var token = _tokenGenerationService.GenerateToken(tokenClaims);

        var userDTO = new UserDTO
        {
            Email = user.Email,
            DisplayName = user.DisplayName,
            Token = token // Set the Token property with the generated token
        };

        return Ok(new
        {
            Message = "Registration successful.",
            Data = userDTO
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid registration data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        var user = _mapper.Map<ApplicationUser>(model);

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);

            // Generate token after successful registration
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.DisplayName),
                    new Claim(ClaimTypes.Email, user.Email),
                    // Add additional claims as needed
                };

            var token = _tokenGenerationService.GenerateToken(claims);

            return Ok(new { Message = "Registration successful.", Data = new UserDTO
            {
                Email = user.Email,
                Token = token,
                DisplayName = user.DisplayName
            }
            });
        }

        return BadRequest(new { Message = "Registration failed.", Error = result.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid login data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            // Generate token after successful login
            var user = await _userManager.FindByEmailAsync(model.Email);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.DisplayName),
                    new Claim(ClaimTypes.Email, user.Email),
                    // Add additional claims as needed
                };

            var token = _tokenGenerationService.GenerateToken(claims);

            return Ok(new
            {
                Message = "Login successful.",
                Data = new UserDTO
                {
                    Email = user.Email,
                    Token = token,
                    DisplayName = user.DisplayName
                }
            });
        }

        if (result.IsLockedOut)
        {
            return BadRequest(new { Message = "Account is locked out." });
        }


        if (result.RequiresTwoFactor)
        {
            return BadRequest(new { Message = "Two-factor authentication required" });
        }

        return BadRequest(new { Message = "Invalid login attempt." });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { Message = "Logout successful." });
    }


    [HttpGet("user-address")]
    [Authorize]
    public async Task<IActionResult> GetUserAddress()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (user.Address == null)
        {
            return NotFound("User address not found.");
        }

        // Return user's address or any other profile information
        return Ok(new
        {
            user.Address.Street,
            user.Address.City,
            user.Address.State,
            user.Address.ZipCode
        });
    }


    [HttpPut("user-address")]
    [Authorize]
    public async Task<IActionResult> UpdateUserAddress([FromBody] AddressDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { Message = "User not found." });
        }

        var address = _mapper.Map<Address>(model);
        user.Address = address;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return Ok(new { Message = "Address updated successfully." });
        }

        return BadRequest(result.Errors);
    }

    [HttpGet("check-email")]
    public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest(new { Message = "Email cannot be empty." });
        }

        var user = await _userManager.FindByEmailAsync(email);
        bool emailExists = user != null;

        return Ok(emailExists);
    }

}