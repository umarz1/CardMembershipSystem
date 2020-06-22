using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MembershipSystem.Api;
using MembershipSystem.Api.Controllers;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MembershipSystemApi.Tests
{
    public class GetUsers
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GetUsers(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetUserByCardId_Returns_User_If_User_Exists()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";
            var userRepo = new Mock<IUserRepository>();
            var testUser = new User
            {
                CardId = "ByDJ0lbYcPkzp2Ja",
                EmployeeId = "12334",
                Name = "test",
                Email = "test email",
                Mobile = "0799797297"
            };

            userRepo.Setup(x => x.GetUserByCardId(cardId)).Returns(testUser);

            // Act
            var controller = new CardUserController(userRepo.Object);
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
            var controller = new CardUserController(userRepo.Object);
            var result = controller.GetUserByCardId(cardId);

            // Assert
            Assert.Equal(null, result);
        }


        //[Fact]
        //public async Task Valid_Request_Returns_Users()
        //{
        //    // Arrange
        //    var client = _factory.CreateClient();

        //    var cardId = "ByDJ0lbYcPkzp2Ja";

        //    var userRepo = new Mock<IUserRepository>();

        //    var user = new User
        //    {
        //        CardId = "ByDJ0lbYcPkzp2Ja",
        //        EmployeeId = "12334",
        //        Name = "test",
        //        Email = "test email",
        //        Mobile = "0799797297"
        //    };

        //    userRepo.Setup(x => x.GetUserByCardId(cardId)).Returns(user);

        //    //var request = new HttpRequestMessage(HttpMethod.Get, $"/users/'{cardId}'");

        //    // Act
        //    //var response = await client.SendAsync(request);
        //    var controller = new CardUserController(userRepo.Object);

        //    var response = controller.GetUserByCardId(cardId);

        //    // Assert

        //    //Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //    //var responseContent = await response.Content.ReadAsStringAsync();
        //    //var json = JToken.Parse(responseContent);
        //}
    }
}

