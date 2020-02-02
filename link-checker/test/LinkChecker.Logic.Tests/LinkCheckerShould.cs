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
using System.Linq;

using LinkChecker;
using LinkChecker.Domain;

namespace LinkChecker.Logic.Tests
{
    public class LinkCheckerShould
    {
        private static readonly List<Link> _links = new List<Link>
        {
            new Link("http://www.google.com/"),
            new Link("https://www.google.com/"),
            new Link("https://apod.nasa.gov/apod/astropix.html"),
            new Link("xtp://malformed-url"),
            new Link("http://unreachable:2323")
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
            (var httpClient, _) = CreateMockHttpClient((msg, token) => new HttpResponseMessage 
            {
                StatusCode = HttpStatusCode.OK
            });
            var linkChecker = new LinkChecker(httpClient);

            // act            
            var result = linkChecker.Check(input);
            PrintOut(result);

            // assert
            result.LinkStates.Keys.Should().BeEquivalentTo(_links);
        }

        [Fact]
        public void ReportMalformedLinks()
        {
            // arrange
            var malformedUrl = new Link("some-bad-link");
            var input = new LinkCheckerInput(new [] { malformedUrl });
            (var httpClient, var httpHandlerMock) = CreateMockHttpClient((msg, token) => throw new NotImplementedException());
            var linkChecker = new LinkChecker(httpClient);

            // act
            var result = linkChecker.Check(input);
            PrintOut(result);

            // assert
            result.LinkStates.Should().BeEquivalentTo(new Dictionary<Link, LinkStatus> {
                [malformedUrl] = LinkStatus.MALFORMED
            });
            httpHandlerMock.Protected().Verify("SendAsync", Times.Never(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void SkipNonHttpLinks()
        {
            // arrange
            var malformedUrl = new Link("xtp://something.com/");
            var input = new LinkCheckerInput(new [] { malformedUrl });
            (var httpClient, var httpHandlerMock) = CreateMockHttpClient((msg, token) => throw new NotImplementedException());
            var linkChecker = new LinkChecker(httpClient);

            // act
            var result = linkChecker.Check(input);
            PrintOut(result);

            // assert
            result.LinkStates.Should().BeEquivalentTo(new Dictionary<Link, LinkStatus> {
                [malformedUrl] = LinkStatus.SKIPPED
            });
            httpHandlerMock.Protected().Verify("SendAsync", Times.Never(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void ReportLinkAsInvalidIfTheIsErrorSendingRequest()
        {
            // arrange
            var link = new Link("http://www.google.com/");
            var input = new LinkCheckerInput(new [] { link });
            (var httpClient, var httpHandlerMock) = CreateMockHttpClient((msg, token) => throw new HttpRequestException("Some random error"));
            var linkChecker = new LinkChecker(httpClient);

            // act
            var result = linkChecker.Check(input);
            PrintOut(result);

            // assert
            result.LinkStates.Should().BeEquivalentTo(new Dictionary<Link, LinkStatus> {
                [link] = LinkStatus.INVALID
            });
            httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(rm => rm.Method == HttpMethod.Get && rm.RequestUri == new Uri(link.Url)),
                ItExpr.IsAny<CancellationToken>());
        }

        private (HttpClient, Mock<HttpMessageHandler>) CreateMockHttpClient(Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> returns)
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
               .Callback<HttpRequestMessage, CancellationToken>((msg, tkn) => _testOutput.WriteLine($"HttpClient sending {msg.Method} request to {msg.RequestUri}"))
               // prepare the expected response of the mocked http call
               .ReturnsAsync(returns)               
               .Verifiable();
            // use real http client with mocked handler here
            return (new HttpClient(handlerMock.Object), handlerMock);
        }

        private void PrintOut(LinkCheckResult result)
        {
            _testOutput.WriteLine(string.Join(Environment.NewLine, result.LinkStates.Select((kvp) => $"{kvp.Key}: {kvp.Value}")));
        }
    }
}
