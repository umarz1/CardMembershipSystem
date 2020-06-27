using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MembershipSystem.Api;
using MembershipSystem.Api.Controllers;
using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Requests;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace MembershipSystemApi.Tests
{
    public class AddAmount
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AddAmount(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        #region Unit Tests

        [Fact]
        public async Task AddAmount_Returns_Latest_Balance_When_Amount_Added()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";

            var newAmount = new AdjustAmount
            {
                CardId = cardId,
                Amount = 10
            };

            var newBalance = new BalanceDto()
            {
                Balance = 10
            };

            var transactionsRepo = new Mock<ITransactionsRepository>();

            transactionsRepo.Setup(x => x.AddAmount(newAmount)).Returns(newBalance);

            // Act
            var controller = new TransactionController(transactionsRepo.Object);
            var result =  controller.AddAmount(newAmount);

            var resultMember = (CreatedAtActionResult)result;

            // Assert
            Assert.Equal(newBalance, resultMember.Value);
        }

        [Fact]
        public async Task AddAmount_Returns_BadRequest_If_Amendment_Fails()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";

            var newAmount = new AdjustAmount
            {
                CardId = cardId,
                Amount = 10
            };

            var newBalance = new BalanceDto()
            {
                Balance = 10
            };

            var transactionsRepo = new Mock<ITransactionsRepository>();

            transactionsRepo.Setup(x => x.AddAmount(It.IsAny<AdjustAmount>())).Returns(() => null);

            // Act
            var controller = new TransactionController(transactionsRepo.Object);
            var result = (StatusCodeResult)controller.AddAmount(newAmount);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task AddMember_Is_Called_Once()
        {
            // Arrange
            var cardId = "ByDJ0lbYcPkzp2Ja";

            var newAmount = new AdjustAmount
            {
                CardId = cardId,
                Amount = 10
            };

            var newBalance = new BalanceDto()
            {
                Balance = 10
            };

            var transactionsRepo = new Mock<ITransactionsRepository>();

            transactionsRepo.Setup(x => x.AddAmount(newAmount)).Returns(newBalance);

            // Act
            var controller = new TransactionController(transactionsRepo.Object);
            var result = controller.AddAmount(newAmount);

            // Assert 
            transactionsRepo.Verify(x => x.AddAmount(newAmount), Times.Once);
        }
        #endregion

        #region Integration Tests

        [Fact]
        public async Task Valid_Request_Returns_Balance()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<ITransactionsRepository, TransactionsRepositoryFake>();
                });
            }).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"transactions/amend-amount")
                .WithJsonContent(new
                {
                    cardId = "VyDJ0lbYcPkzp2Ju",
                    amount = 20,
                });

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            await response.AssertJsonDeepEquals(new
            {
                balance = 50.25,
            });
        }

        [Fact]
        public async Task Negative_Amount_Returns_Bad_Request()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<ITransactionsRepository, TransactionsRepositoryFake>();
                });
            }).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"transactions/amend-amount")
                .WithJsonContent(new
                {
                    cardId = "VyDJ0lbYcPkzp2Ju",
                    amount = -20,
                });


            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        #endregion
    }
}
