using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MembershipSystem.Api;
using MembershipSystem.Api.Controllers;
using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MembershipSystemApi.Tests
{
    public class GetUser
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GetUser(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        #region Unit Tests

        [Fact]
        public async Task GetUserByCardId_Returns_User_If_User_Exists()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";
            var userRepo = new Mock<IUserRepository>();
            var testUser = new UserDto
            {
                CardId = "ByDJ0lbYcPkzp2Ja",
                Name = "Test User",
            };

            userRepo.Setup(x => x.GetUserByCardId(cardId)).Returns(testUser);

            // Act
            var controller = new UserController(userRepo.Object);
            var result = controller.GetUserByCardId(cardId);

            var resultUser = (OkObjectResult)result.Result;
            
            // Assert
           Assert.Equal(testUser, resultUser.Value);
        }

        [Fact]
        public async Task GetUserByCardId_Returns_Null_If_User_Does_Not_Exists()
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

        [Fact]
        public async Task GetUserByCardId_Is_Called_Once()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";
            var userRepo = new Mock<IUserRepository>();

            userRepo.Setup(x => x.GetUserByCardId(It.IsAny<string>())).Returns(() => null);

            // Act
            var controller = new UserController(userRepo.Object);
            var result = controller.GetUserByCardId(cardId);

            // Assert 
            userRepo.Verify(x => x.GetUserByCardId(cardId), Times.Once);
        }
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

            var cardId = "ByDJ0lbYcPkzp2Ja";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/users/{cardId}");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            await response.AssertJsonDeepEquals(new
            {
                cardId = "ByDJ0lbYcPkzp2Ja",
                name = "Test User",
            });
        }

        [Fact]
        public async Task User_Does_Not_Exist_Returns_NotFound()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IUserRepository, UserRepositoryFake>();
                });
            }).CreateClient();

            var cardId = "ByDJ0lbYcPkzp2Ja";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/users/'{cardId}'");

            // Act
            var response = await client.SendAsync(request);


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion
    }
}

