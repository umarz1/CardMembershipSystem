using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MembershipSystem.Api;
using MembershipSystem.Api.Controllers;
using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace MembershipSystemApi.Tests
{
    public class AddUser
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AddUser(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        #region Unit Tests

        [Fact]
        public async Task AddUser_Returns_User_When_Created()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";

            var newUser = new User
            {
                CardId = cardId,
                Name = "Test User",
                EmployeeId = "1234",
                Email = "test.user@hotmail.com",
                Mobile = "0774564154",
                Pin = "3433"
            };

            var createUser = new UserDto()
            {
                CardId = cardId,
                Name = "Test User"
            };

            var userRepo = new Mock<IUserRepository>();

            userRepo.Setup(x => x.AddUser(newUser)).Returns(createUser);

            // Act
            var controller = new UserController(userRepo.Object);
            var result = controller.AddUser(newUser);

            //var resultUser = (OkObjectResult)result.Result;

            // Assert
            //Assert.Equal(testUser, resultUser.Value);
        }

        [Fact]
        public async Task AddUser_Returns_Null_If_User_Does_Not_Exists()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";
            var userRepo = new Mock<IUserRepository>();

            userRepo.Setup(x => x.GetUserByCardId(It.IsAny<string>())).Returns(() => null);

            // Act
            var controller = new UserController(userRepo.Object);
            var result = controller.GetUserByCardId(cardId);

            // Assert
            Assert.Equal(null, result.Value);
        }

        //[Fact]
        //public async Task AddUser_Is_Called_Once()
        //{
        //    // Arrange
        //    var cardId = "ByDJ0lbYcPkzp2Ja";
        //    var userRepo = new Mock<IUserRepository>();

        //    userRepo.Setup(x => x.GetUserByCardId(It.IsAny<string>())).Returns(() => null);

        //    // Act
        //    var controller = new UserController(userRepo.Object);
        //    var result = controller.GetUserByCardId(cardId);

        //    // Assert 
        //    userRepo.Verify(x => x.GetUserByCardId(cardId), Times.Once);
        //}
        #endregion

        #region Integration Tests

        [Fact]
        public async Task Valid_Request_Returns_User()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IUserRepository, UserRepositoryFake>();
                });
            }).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"/users")
                .WithJsonContent(new
                {
                    cardId = "ByDJ0lbYcPkzp2Jx",
                    employeeId = "1234",
                    name = "Test User",
                    email = "test.user@hotmail.co.uk",
                    mobile = "07746312234",
                    pin = "7727"
                });

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            await response.AssertJsonDeepEquals(new
            {
                cardId = "ByDJ0lbYcPkzp2Jx",
                name = "Test User",
            });
        }

        [Fact]
        public async Task User_Already_Exists_Returns_Bad_Request()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IUserRepository, UserRepositoryFake>();
                });
            }).CreateClient();

            var existingCardId = "ByDJ0lbYcPkzp2Ja";

            var request = new HttpRequestMessage(HttpMethod.Post, $"/users")
                .WithJsonContent(new
                {
                    cardId = existingCardId,
                    employeeId = "1234",
                    name = "Test User",
                    email = "test.user@hotmail.co.uk",
                    mobile = "07746312234",
                    pin = "7727"
                });

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion
    }
}
