using AdventOfCode23.Processors;
using FluentAssertions;

namespace AdventOfCode23.Specs
{
    public class _04_ScratchCards
    {
        [Theory]
        [InlineData("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53", 8)]
        [InlineData("Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19", 2)]
        [InlineData("Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1", 2)]
        [InlineData("Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83", 1)]
        [InlineData("Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36", 0)]
        [InlineData("Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", 0)]
        [InlineData("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\n", 10)]
        public void ScratchCardWinningPointsReturnsCorrectPointValuesPart1(string input, int expectedPoints) 
            => new ScratchCardProcessor().ProcessScratchCardsPart1(input).Should().Be(expectedPoints);

        [Fact]
        public void InputFileProducesResultPart1()
        {
            var lines = File.ReadAllText("Inputs/04-ScratchCardsInput.txt");
            new ScratchCardProcessor().ProcessScratchCardsPart1(lines).Should().Be(23750);
        }

        [Theory]
        [InlineData(
            "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\n" + 
            "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\n" +
            "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\n" +
            "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\n" +
            "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\n" +
            "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11",
            30)]
        public void ScratchCardWinningPointsReturnsCorrectPointValuesPart2(string input, int expectedPoints)
            => new ScratchCardProcessor().ProcessScratchCardsPart2(input).Should().Be(expectedPoints);

        [Fact]
        public void InputFileProducesResultPart2()
        {
            var lines = File.ReadAllText("Inputs/04-ScratchCardsInput.txt");
            new ScratchCardProcessor().ProcessScratchCardsPart2(lines).Should().Be(13261850);
        }
    }
}
