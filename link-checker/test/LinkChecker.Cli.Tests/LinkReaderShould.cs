using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

using LinkChecker.Domain;

namespace LinkChecker.Cli.Tests
{
    public class LinkReaderShould
    {
        public LinkReaderShould(ITestOutputHelper output)
        {
            _output = output;
        }

        private ITestOutputHelper _output;

        [Fact]
        public void ReadFromStream()
        {
            // arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.WriteLine("www.google.com");
            writer.WriteLine("http://www.google.com");
            writer.WriteLine("asdfjkl-123");
            writer.WriteLine("www.unreachable.com");
            writer.Flush();
            stream.Position = 0;
            _output.WriteLine("Recorded input links: " + new StreamReader(stream).ReadToEnd());
            stream.Position = 0;
            var linkReader = new LinkReader(stream);

            // act
            var allLinks = linkReader.Read().ToList();
            _output.WriteLine($"Links: {string.Join(", ", allLinks)}");

            // assert
            allLinks.Should().BeEquivalentTo(new[] {
                "www.google.com",
                "http://www.google.com",
                "asdfjkl-123",
                "www.unreachable.com"
            }.Select(x => new Link(x)));
        }
    }
}
