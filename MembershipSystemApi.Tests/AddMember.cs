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
using Xunit;

namespace MembershipSystemApi.Tests
{
    public class AddMember
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AddMember(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        #region Unit Tests

        [Fact]
        public async Task AddMember_Controller_Action_Returns_Member_When_Created()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";

            var newMember = new NewMember
            {
                CardId = cardId,
                Name = "Test User",
                EmployeeId = "1234",
                Email = "test.user@hotmail.com",
                Mobile = "0774564154",
                Pin = "3433"
            };

            var createdMember = new MemberDto()
            {
                Name = "Test User"
            };

            var memberService = new Mock<IMemberService>();

            memberService.Setup(x => x.AddMember(newMember)).Returns(createdMember);

            // Act
            var controller = new MemberController(memberService.Object);
            var result = controller.AddMember(newMember);

            var resultMember = (CreatedAtActionResult)result;

            // Assert
            Assert.Equal(createdMember, resultMember.Value);
        }

        [Fact]
        public async Task AddMember_Controller_Action_Returns_Null_If_Fails_To_Create()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";
            var memberService = new Mock<IMemberService>();

            memberService.Setup(x => x.GetMember(It.IsAny<string>())).Returns(() => null);

            // Act
            var controller = new MemberController(memberService.Object);
            var result = controller.GetMember(cardId);

            // Assert
            Assert.Equal(null, result.Value);
        }

        [Fact]
        public async Task AddMember_Database_Call_Is_Made_Once()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";
            var memberRepo = new Mock<IMembershipRepository>();

            var newMember = new NewMember
            {
                CardId = cardId,
                Name = "Test User",
                EmployeeId = "1234",
                Email = "test.user@hotmail.com",
                Mobile = "0774564154",
                Pin = "3433"
            };

            var createdMember = new MemberDto()
            {
                Name = "Test User"
            };

            memberRepo.Setup(x => x.AddMember(It.IsAny<NewMember>())).Returns(() => null);

            // Act
            var memberService = new MemberService(memberRepo.Object);
            var result = memberService.AddMember(newMember);

            // Assert 
            memberRepo.Verify(x => x.AddMember(It.IsAny<NewMember>()), Times.Once);
        }
        #endregion

        #region Integration Tests

        [Fact]
        public async Task Valid_Request_Returns_Member()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IMembershipRepository, MemberRepositoryFake>();
                });
            }).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"/members")
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
                name = "Test User",
            });
        }

        [Fact]
        public async Task Member_Already_Exists_Returns_Bad_Request()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IMembershipRepository, MemberRepositoryFake>();
                });
            }).CreateClient();

            var existingCardId = "VyDJ0lbYcPkzp2Ju";

            var request = new HttpRequestMessage(HttpMethod.Post, $"/members")
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


        [Fact]
        public async Task Invalid_Request_Returns_Bad_Request()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IMembershipRepository, MemberRepositoryFake>();
                });
            }).CreateClient();

            var existingCardId = "VyDJ0lbYcPkzp2Ju";
            var invalidEmail = "test.user";

            var request = new HttpRequestMessage(HttpMethod.Post, $"/members")
                .WithJsonContent(new
                {
                    cardId = existingCardId,
                    employeeId = "1234",
                    name = "Test User",
                    email = invalidEmail,
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
