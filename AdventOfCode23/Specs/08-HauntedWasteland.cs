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

        [Fact]
        public void GhostsAllHitEndIn6Moves()
        {
            var input = "LR\n\n11A = (11B, XXX)\n11B = (XXX, 11Z)\n11Z = (11B, XXX)\n22A = (22B, XXX)\n22B = (22C, 22C)\n22C = (22Z, 22Z)\n22Z = (22B, 22B)\nXXX = (XXX, XXX)";
            new HauntedWastelandProcessor(input).CalculateNumberOfTurnsForGhostsToReachEnd().Should().Be(6);
        }

        [Fact]
        public void InputFileProducesResultPart2()
        {
            var input = File.ReadAllText("Inputs/08-HauntedWasteland.txt");
            new HauntedWastelandProcessor(input).CalculateNumberOfTurnsForGhostsToReachEnd().Should().Be(0);
        }

    }
}
