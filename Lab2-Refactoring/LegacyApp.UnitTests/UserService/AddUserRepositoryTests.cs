using LegacyApp.Enums;
using Moq;

namespace LegacyApp.UnitTests.UserService;

public class AddUserRepositoryTests
{
        private readonly LegacyApp.UserService _userService;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IUserCreditService> _userCreditServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public AddUserRepositoryTests()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _userCreditServiceMock = new Mock<IUserCreditService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            
            _userService = new LegacyApp.UserService(_clientRepositoryMock.Object, _userCreditServiceMock.Object, _userRepositoryMock.Object);
        }
        
        [Fact]
        public void DefaultConstructor_InitializesDependenciesCorrectly()
        {
            // Arrange
            var userService = new LegacyApp.UserService();

            // Act
            var result = userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(1980, 1, 1), 1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddUser_WhenCalledWithValidParameters_ReturnsTrue()
        {
            // Arrange
            var client = new Client { Type = ClientType.NormalClient };
            _clientRepositoryMock.Setup(c => c.GetById(It.IsAny<int>())).Returns(client);
            _userCreditServiceMock.Setup(u => u.GetCreditLimit(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(600);

            // Act
            var result = _userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(1980, 1, 1), 1);

            // Assert
            Assert.True(result);
            _userRepositoryMock.Verify(u => u.AddUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void AddUser_WhenCalledWithCreditLimitLessThan500_ReturnsFalse()
        {
            // Arrange
            var client = new Client { Type = ClientType.NormalClient };
            _clientRepositoryMock.Setup(c => c.GetById(It.IsAny<int>())).Returns(client);
            _userCreditServiceMock.Setup(u => u.GetCreditLimit(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(400);

            // Act
            var result = _userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(1980, 1, 1), 1);

            // Assert
            Assert.False(result);
            _userRepositoryMock.Verify(u => u.AddUser(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void AddUser_WhenClientIsVeryImportantClient_UserHasNoCreditLimit()
        {
            // Arrange
            var client = new Client { Type = ClientType.VeryImportantClient };
            _clientRepositoryMock.Setup(c => c.GetById(It.IsAny<int>())).Returns(client);

            // Act
            var result = _userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(1980, 1, 1), 1);

            // Assert
            Assert.True(result);
            _userRepositoryMock.Verify(u => u.AddUser(It.Is<User>(user => !user.HasCreditLimit)), Times.Once);
        }

        [Fact]
        public void AddUser_WhenClientIsImportantClient_UserCreditLimitIsTwiceTheCalculatedValue()
        {
            // Arrange
            var client = new Client { Type = ClientType.ImportantClient };
            _clientRepositoryMock.Setup(c => c.GetById(It.IsAny<int>())).Returns(client);
            _userCreditServiceMock.Setup(u => u.GetCreditLimit(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(500);

            // Act
            var result = _userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(1980, 1, 1), 1);

            // Assert
            Assert.True(result);
            _userRepositoryMock.Verify(u => u.AddUser(It.Is<User>(user => user.CreditLimit == 1000)), Times.Once);
        }

        [Fact]
        public void AddUser_WhenClientIsNormalClient_UserCreditLimitIsTheCalculatedValue()
        {
            // Arrange
            var client = new Client { Type = ClientType.NormalClient };
            _clientRepositoryMock.Setup(c => c.GetById(It.IsAny<int>())).Returns(client);
            _userCreditServiceMock.Setup(u => u.GetCreditLimit(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(500);

            // Act
            var result = _userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(1980, 1, 1), 1);

            // Assert
            Assert.True(result);
            _userRepositoryMock.Verify(u => u.AddUser(It.Is<User>(user => user.CreditLimit == 500)), Times.Once);
        }
}