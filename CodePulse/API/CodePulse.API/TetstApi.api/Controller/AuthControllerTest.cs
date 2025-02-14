namespace TetstApi.api.Controller;
using CodePulse.API.Controllers;
using CodePulse.API.Models.DTO.AuthDto;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class AuthControllerTest
{
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<ITokenRepository> _tokenRepositoryMock;
    private readonly AuthController _authController;
    
    public AuthControllerTest()
    {
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            new Mock<IUserStore<IdentityUser>>().Object, null, null, null, null, null, null, null, null);

        _tokenRepositoryMock = new Mock<ITokenRepository>();

        _authController = new AuthController(_userManagerMock.Object, _tokenRepositoryMock.Object);
    }
    
    [Fact]
    public async Task Register_ReturnsOk_WhenUserIsCreatedSuccessfully()
    {
        // Arrange
        var registerDto = new RegisterRequestDto { Email = "test@example.com", Password = "Test123!" };
        var identityUser = new IdentityUser { UserName = registerDto.Email, Email = registerDto.Email };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Reader"))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authController.Register(registerDto);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public async Task Register_ReturnsBadRequest_WhenUserCreationFails()
    {
        // Arrange
        var registerDto = new RegisterRequestDto { Email = "test@example.com", Password = "Test123!" };
        
        var identityResult = IdentityResult.Failed(new IdentityError { Description = "Error creating user" });

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _authController.Register(registerDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Register_ReturnsBadRequest_WhenRoleAssignmentFails()
    {
        // Arrange
        var registerDto = new RegisterRequestDto { Email = "test@example.com", Password = "Test123!" };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Reader"))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Role assignment failed" }));

        // Act
        var result = await _authController.Register(registerDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Login_ReturnsOk_WithValidCredentials()
    {
        // Arrange
        var loginDto = new LoginRequestDto { Email = "test@example.com", Password = "Test123!" };
        var identityUser = new IdentityUser { Email = loginDto.Email };

        _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(identityUser);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new List<string> { "Reader" });

        _tokenRepositoryMock.Setup(x => x.CreateJwtToken(It.IsAny<IdentityUser>(), It.IsAny<List<string>>()))
            .Returns("fake-jwt-token");

        // Act
        var result = await _authController.Login(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<LoginResponseDto>(okResult.Value);
        Assert.Equal("fake-jwt-token", response.Token);
        Assert.Equal("test@example.com", response.Email);
        Assert.Contains("Reader", response.Roles);
    }
    
    [Fact]
    public async Task Login_ReturnsValidationProblem_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginDto = new LoginRequestDto { Email = "test@example.com", Password = "WrongPassword!" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null);

        // Act
        var result = await _authController.Login(loginDto);

        // Assert
        var validationProblem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(400, validationProblem.StatusCode);
    }

}