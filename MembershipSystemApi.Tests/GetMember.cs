using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MembershipSystem.Api;
using MembershipSystem.Api.Controllers;
using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace MembershipSystemApi.Tests
{
    public class GetMember
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GetMember(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        #region Unit Tests

        [Fact]
        public async Task GetMember_Controller_Action_Returns_Member_If_Member_Exists()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";
            var memberService = new Mock<IMemberService>();
            var testMember = new MemberDto
            {
                Name = "Test User",
            };

            memberService.Setup(x => x.GetMember(cardId)).Returns(testMember);

            // Act
            var controller = new MemberController(memberService.Object);
            var result = controller.GetMember(cardId);

            var resultMember = (OkObjectResult)result.Result;

            // Assert
            Assert.Equal(testMember, resultMember.Value);
        }

        [Fact]
        public async Task GetMember_Controller_Action_Returns_Null_If_Member_Does_Not_Exists()
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
        public async Task GetMember_Database_Call_Is_Made_Once()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";
            var memberRepo = new Mock<IMembershipRepository>();

            memberRepo.Setup(x => x.GetMember(It.IsAny<string>())).Returns(() => null);

            // Act
            var memberService = new MemberService(memberRepo.Object);
            var result = memberService.GetMember(cardId);

            // Assert 
            memberRepo.Verify(x => x.GetMember(cardId), Times.Once);
        }

        [Fact]
        public async Task GetMember_Returns_Null_When_Card_Id_More_Than_Sixteen_Digits()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Jaaa";
            var memberRepo = new Mock<IMembershipRepository>();

            memberRepo.Setup(m => m.GetMember(cardId)).Returns(() => null);

            // Act
            var memberService = new MemberService(memberRepo.Object);
            var result = memberService.GetMember(cardId);

            // Assert 
            Assert.Equal(null, result);
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

            var cardId = "VyDJ0lbYcPkzp2Ju";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/members/{cardId}");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            await response.AssertJsonDeepEquals(new
            {
                name = "Test User1",
            });
        }

        [Fact]
        public async Task Member_Does_Not_Exist_Returns_NotFound()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IMembershipRepository, MemberRepositoryFake>();
                });
            }).CreateClient();

            var cardId = "ByDJ0lbYcPkzp2Ja";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/members/{cardId}");

            // Act
            var response = await client.SendAsync(request);


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion
    }
}

