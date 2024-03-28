using CSharpFunctionalExtensions;
using LegacyApp.Enums;
using Moq;

namespace LegacyApp.UnitTests.UserService;

public class AddUserValidationTests
{
    private readonly LegacyApp.UserService _userService;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<IUserCreditService> _userCreditServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public AddUserValidationTests()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _userCreditServiceMock = new Mock<IUserCreditService>();
        _userRepositoryMock = new Mock<IUserRepository>();
            
        _userService = new LegacyApp.UserService(_clientRepositoryMock.Object, _userCreditServiceMock.Object, _userRepositoryMock.Object);
    }
    
    [Fact]
    public void Create_WhenCalledWithValidParameters_ReturnsUser()
    {
        // Act
        var (_, isFailure, user) = User.Create("John", "Doe", "john.doe@example.com", new DateTime(1980, 1, 1));

        // Assert
        Assert.False(isFailure);
        Assert.NotNull(user);
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal("john.doe@example.com", user.Email);
        Assert.Equal(new DateTime(1980, 1, 1), user.DateOfBirth);
    }

    [Fact]
    public void Create_WhenCalledWithInvalidEmail_ReturnsFailure()
    {
        // Act
        var (_, isFailure, user, error) = User.Create("John", "Doe", "invalid email", new DateTime(1980, 1, 1));

        // Assert
        Assert.True(isFailure);
        Assert.Equal("Email is not valid", error.Message);
    }

    [Fact]
    public void Create_WhenCalledWithFutureDateOfBirth_ReturnsFailure()
    {
        // Act
        var (_, isFailure, user, error) = User.Create("John", "Doe", "john.doe@example.com", DateTime.Now.AddYears(1));

        // Assert
        Assert.True(isFailure);
        Assert.Equal("User must be at least 21 years old", error.Message);
    }
    
    [Fact]
    public void AddUser_WhenClientIsNull_ReturnsTrue()
    {
        // Arrange
        _clientRepositoryMock.Setup(c => c.GetById(It.IsAny<int>())).Returns((Client)null);

        // Act
        var result = _userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(1980, 1, 1), 1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AddUser_WhenUserIsUnder21_ReturnsFalse()
    {
        // Arrange
        var client = new Client { Type = ClientType.NormalClient };
        _clientRepositoryMock.Setup(c => c.GetById(It.IsAny<int>())).Returns(client);

        // Act
        var result = _userService.AddUser("John", "Doe", "john.doe@example.com", DateTime.Now.AddYears(-20), 1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_WhenEmailIsAlreadyInUse_ReturnsFalse()
    {
        // Arrange
        var client = new Client { Type = ClientType.NormalClient };
        _clientRepositoryMock.Setup(c => c.GetById(It.IsAny<int>())).Returns(client);

        // Act
        var result = _userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(1980, 1, 1), 1);

        // Assert
        Assert.False(result);
    }
}