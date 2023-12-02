using AdventOfCode23.Processors;
using FluentAssertions;

namespace AdventOfCode23.Specs
{
    public class _02_Cubes
    {
        [Theory]
        [InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", 12, 13, 14, 1)]
        [InlineData("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", 12, 13, 14, 2)]
        [InlineData("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", 12, 13, 14, 0)]
        [InlineData("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", 12, 13, 14, 0)]
        [InlineData("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 12, 13, 14, 5)]
        public void CubeGameIsValid(string input, int red, int green, int blue, int validNumberOfGames) 
            => new CubeProcessor(red, green, blue).TotalOfGamesValid(input).Should().Be(validNumberOfGames);

        [Fact]
        public void MultiLineInputCubeGameIsValid()
            => new CubeProcessor(12, 13, 14)
                .TotalOfGamesValid(
                    "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\n" +
                    "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\n" +
                    "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\n" +
                    "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\n" +
                    "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green")
                .Should().Be(8);

        [Fact]
        public void Part1Solution()
        {
            var games = File.ReadAllText("Inputs/02-CubesInput.txt");
            new CubeProcessor(12, 13, 14)
                .TotalOfGamesValid(games).Should().Be(2006);
        }
    }
}
