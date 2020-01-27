using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

using UrlChecker;
using UrlChecker.Domain;

namespace UrlChecker.Tests
{
    public class UrlCheckerShould
    {
        private UrlChecker _urlChecker;

        public UrlCheckerShould() 
        {
            _urlChecker = new UrlChecker();
        }

        [Fact]
        public void CheckAllInputUrlsWhenNoFilterIsSpecified()
        {
            // arrange
            var urls = new List<Uri> {
                new Uri("http://www.google.com/"),
                new Uri("https://www.google.com/"),
                new Uri("https://apod.nasa.gov/apod/astropix.html")
            };
            var input = new UrlCheckerInput(urls);

            // act
            var result = _urlChecker.Check(input);

            // assert
            result.UrlStates.Keys.Should().BeEquivalentTo(urls);
        }
    }
}
