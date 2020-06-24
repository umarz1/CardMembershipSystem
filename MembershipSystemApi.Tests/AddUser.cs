using MembershipSystem.Api;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
                    cardId = "ByDJ0lbYcPkzp2Ja",
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
                cardId = "ByDJ0lbYcPkzp2Ja",
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

            var request = new HttpRequestMessage(HttpMethod.Post, $"/users")
                .WithJsonContent(new
                {
                    cardId = "ByDJ0lbYcPkzp2Ja",
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

            await response.AssertJsonDeepEquals(new
            {
                cardId = "ByDJ0lbYcPkzp2Ja",
                name = "Test User",
            });
        }
    }
}
