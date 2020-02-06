using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class ComponentShould
    {
        private ITestOutputHelper _output;

        public ComponentShould(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void DoStuff_WhenAskedTo()
        {
            _output.WriteLine("Output from test...");
        }
    }
}