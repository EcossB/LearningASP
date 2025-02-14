using CodePulse.API.Models.DTO.AuthDto;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    
    /*Injectamos el UserManager, esta clase nos ayuda a poder crear nuevos usuarios
      Asp.net nos brinda esta clase para hacer la persistencia de los datos de los usuarios
      nuevos que queremos agregar*/
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenRepository _tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }
    
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
    {
        /*nuestra capa modelo de los usuarios sera la clase IdentityUser*/

        var user = new IdentityUser
        {
            UserName = registerRequest.Email?.Trim(),
            Email = registerRequest.Email?.Trim()
        };
        
        //Guardamos el usuario nuevo usando UserManager
        
        var newUser = await _userManager.CreateAsync(user, registerRequest.Password);

        if (newUser.Succeeded)
        {
            //agregando el rol a usuario creado
            newUser = await _userManager.AddToRoleAsync(user, "Reader");

            if (newUser.Succeeded)
            {
                return Ok();
            } else
            {
                if (newUser.Errors.Any())
                {
                    foreach (var error in newUser.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }   
            }
        }
        else
        {
            if (newUser.Errors.Any())
            {
                foreach (var error in newUser.Errors)
                {
                    ModelState.AddModelError("" , error.Description);
                }
            }   
        }
        
        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        
        //buscamos el usuario por el Email
        var identityUser = await _userManager.FindByEmailAsync(loginRequest.Email);

        if (identityUser is not null)
        {
            //validamos que el usuario haya puesto una contraseña correcta
            var identityUserPassword =await _userManager.CheckPasswordAsync(identityUser, loginRequest.Password);

            if (identityUserPassword)
            {
                var roles = await _userManager.GetRolesAsync(identityUser);
                //creamos el JWT
                var token = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                var loginResponse = new LoginResponseDto
                {
                    Email = identityUser.Email,
                    Roles = roles.ToList(),
                    Token = token
                };
                
                return Ok(loginResponse);

            }
        }
        
        //ModelState.AddModelError("400", "Invalid username or password.");
        //return ValidationProblem(ModelState);
        return BadRequest();
    }
    
}