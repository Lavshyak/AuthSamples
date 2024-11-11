using System.Security.Claims;
using JWTSample.Db.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWTSample.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<Account> _signInManager;
    private readonly UserManager<Account> _userManager;
    private readonly RoleManager<AccountRole> _roleManager;

    public AuthController(SignInManager<Account> signInManager, UserManager<Account> userManager,
        RoleManager<AccountRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        
        //_signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
    }

    private const string Password = "1234!@#$qwerQWER";

    public record RegisterParameters(string Email);

    [HttpPost]
    public async Task<IActionResult> Register(RegisterParameters registerParameters)
    {
        var account = new Account()
        {
            Email = registerParameters.Email,
            UserName = registerParameters.Email
        };

        var creationResult = await _userManager.CreateAsync(account, Password);

        if (!creationResult.Succeeded)
        {
            return this.BadRequest(creationResult);
        }

        return Ok();
    }

    public record LoginParameters(string Email);
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginParameters loginParameters)
    {
        var account = await _signInManager.UserManager.FindByEmailAsync(loginParameters.Email);

        if (account == null)
            return this.BadRequest("почта не найдена");

        var checkPasswordSignInResult = await _signInManager.CheckPasswordSignInAsync(account, Password, false);
        if (!checkPasswordSignInResult.Succeeded)
        {
            return this.BadRequest(checkPasswordSignInResult);
        }

        
        await _signInManager.SignInAsync(account, true);

        return Ok();
    }

    
    [Authorize]
    [HttpGet]
    public string CheckMyEmail()
    {
        var email = this.User.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
        return email;
    }
}