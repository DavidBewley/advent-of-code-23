using AdventOfCode23.Processors;
using FluentAssertions;

namespace AdventOfCode23.Specs
{
    public class _06_BoatRace
    {
        [Theory]
        [InlineData(7, 9, 4)]
        [InlineData(15, 40, 8)]
        [InlineData(30, 200, 9)]
        public void BoatRaceCanBeWonNumberOfWays(int raceLength, int recordDistance, int numberOfWaysToWin) 
            => new BoatRaceProcessor().GetNumberOfWaysToWin($"Time:      {raceLength}\nDistance:  {recordDistance}").Should().Be(numberOfWaysToWin);

        [Fact]
        public void InputFileProducesResultPart1()
        {
            var input = File.ReadAllText("Inputs/06-BoatRace.txt");
            new BoatRaceProcessor().GetNumberOfWaysToWin(input).Should().Be(2449062);
        }

        [Fact]
        public void BoatRaceBadKerningCanBeWonNumberOfWays()
            => new BoatRaceProcessor().GetNumberOfWaysToWinBadKerning($"Time:      7  15   30\nDistance:  9  40  200").Should().Be(71503);

        [Fact]
        public void InputFileProducesResultPart2()
        {
            var input = File.ReadAllText("Inputs/06-BoatRace.txt");
            new BoatRaceProcessor().GetNumberOfWaysToWinBadKerning(input).Should().Be(33149631);
        }
    }
}
