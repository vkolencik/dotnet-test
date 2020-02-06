using System;
using System.Linq;
using Domain;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class SortingHatShould
    {
        private ITestOutputHelper _output;

        public SortingHatShould(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void PutDracoToSlytherin()
        {
            // arrange
            var sortingHat = new SortingHat();
            var draco = new Wizard("Draco Malfoy");
            var wizards = new[] {draco};
            
            _output.WriteLine("Wizards to be sorted:");
            Array.ForEach(wizards, w => _output.WriteLine($"* {w}"));

            // act
            var houseMemberships = sortingHat.Sort(wizards);
            
            _output.WriteLine("Assigned house memberships:");
            houseMemberships.ToList().ForEach(kvp => _output.WriteLine($"* {kvp.Key} in {kvp.Value}"));

            _output.WriteLine(JsonConvert.SerializeObject(houseMemberships));
            // assert
            Assert.Equal(House.Slytherin, houseMemberships[draco]);
        }
    }
}
