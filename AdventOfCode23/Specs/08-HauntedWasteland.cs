using AdventOfCode23.Processors;
using FluentAssertions;

namespace AdventOfCode23.Specs
{
    public class _08_HauntedWasteland
    {
        private const string _exampleOne = "RL\n\nAAA = (BBB, CCC)\nBBB = (DDD, EEE)\nCCC = (ZZZ, GGG)\nDDD = (DDD, DDD)\nEEE = (EEE, EEE)\nGGG = (GGG, GGG)\nZZZ = (ZZZ, ZZZ)";
        private const string _exampleTwo = "LLR\n\nAAA = (BBB, BBB)\nBBB = (AAA, ZZZ)\nZZZ = (ZZZ, ZZZ)";

        [Theory]
        [InlineData(_exampleOne,2)]
        [InlineData(_exampleTwo, 6)]
        public void ExampleInputReachesDestinationInExpectedMoves(string input, int expectedMoves) 
            => new HauntedWastelandProcessor(input).CalculateNumberOfTurnsToReachDestination().Should().Be(expectedMoves);

        [Fact]
        public void InputFileProducesResultPart1()
        {
            var input = File.ReadAllText("Inputs/08-HauntedWasteland.txt");
            new HauntedWastelandProcessor(input).CalculateNumberOfTurnsToReachDestination().Should().Be(13771);
        }

    }
}
