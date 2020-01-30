using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using LinkChecker;
using LinkChecker.Domain;

namespace LinkChecker.Logic.Tests
{
    public class LinkCheckerShould
    {
        private static readonly List<Uri> _links = new List<Uri>
        {
            new Uri("http://www.google.com/"),
            new Uri("https://www.google.com/"),
            new Uri("https://apod.nasa.gov/apod/astropix.html"),
            new Uri("xtp://malformed-url"),
            new Uri("http://unreachable:2323")
        };

        private ITestOutputHelper _testOutput;

        public LinkCheckerShould(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public void CheckAllInputLinksWhenNoFilterIsSpecified()
        {
            // arrange
            var input = new LinkCheckerInput(_links);
            var httpClient = CreateMockHttpClient((msg, token) => new HttpResponseMessage 
            {
                StatusCode = HttpStatusCode.OK
            });
            var linkChecker = new LinkChecker(httpClient);

            // act
            var result = linkChecker.Check(input);

            // assert
            result.LinkStates.Keys.Should().BeEquivalentTo(_links);
            result.LinkStates.Values.Should().OnlyContain(s => s == LinkStatus.OK);
        }

        private HttpClient CreateMockHttpClient(Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> returns)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(returns)
               .Verifiable();
            // use real http client with mocked handler here
            return new HttpClient(handlerMock.Object);
        }
    }
}
