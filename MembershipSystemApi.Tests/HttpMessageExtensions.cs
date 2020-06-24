using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MembershipSystemApi.Tests
{
    public static class HttpMessageExtensions
    {
        private const string JsonContentType = "application/json";

        private static readonly Newtonsoft.Json.JsonSerializer _serializer;

        static HttpMessageExtensions()
        {
            _serializer = Newtonsoft.Json.JsonSerializer.Create();
        }

        //public static async Task AssertHasError(
        //    this HttpResponseMessage response,
        //    string expectedErrorCode,
        //    int expectedStatusCode = 400,
        //    Option<object> expectedData = default)
        //{
        //    Assert.Equal(expectedStatusCode, (int)response.StatusCode);

        //    var json = await response.GetResponseJson();

        //    Assert.Equal(expectedErrorCode, json["error"]["code"].ToString());

        //    if (expectedData.HasValue)
        //    {
        //        AssertEx.AssertJsonObjectEquals(expectedData.ValueOrFailure(), json["error"]["data"]);
        //    }
        //}

        public static Task AssertJsonDeepEquals(this HttpResponseMessage response, object expected)
        {
            return AssertJsonDeepEquals(response, "$", expected);
        }

        public static async Task AssertJsonDeepEquals(this HttpResponseMessage response, string path, object expected)
        {
            var expectedJson = JToken.FromObject(expected, _serializer);

            var actualJson = await response.GetResponseJson();
            var actualJsonSelection = actualJson.SelectToken(path);

            Assert.Equal(expectedJson, actualJsonSelection);
        }

        public static async Task<JToken> GetResponseJson(this HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            return JToken.Parse(responseContent);
        }

        public static HttpRequestMessage WithJsonContent(this HttpRequestMessage request, object json)
        {
            var serialized = JsonConvert.SerializeObject(json);

            request.Content = new StringContent(serialized, Encoding.UTF8, JsonContentType);

            return request;
        }
    }
}
