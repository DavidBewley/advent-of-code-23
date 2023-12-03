using AdventOfCode23.Processors;
using FluentAssertions;

namespace AdventOfCode23.Specs
{
    public class _03_GearRatios
    {
        [Theory]
        [InlineData(".467+",467)]
        [InlineData(".467+..1+..",468)]
        [InlineData("+467.",467)]
        [InlineData("+.467.+",0)]
        [InlineData(".467.\n#....",467)]
        [InlineData(".467.\n.#...",467)]
        [InlineData(".467.\n..#..",467)]
        [InlineData(".467.\n...#.",467)]
        [InlineData(".467.\n....#",467)]
        [InlineData("#....\n.467.", 467)]
        [InlineData(".#...\n.467.", 467)]
        [InlineData("..#..\n.467.", 467)]
        [InlineData("...#.\n.467.", 467)]
        [InlineData("....#\n.467.", 467)]
        [InlineData("..#\n467", 467)]
        [InlineData("#..\n467", 467)]
        public void SymbolsTouchingProducesResult(string input, int expectedOutput) 
            => new GearRatioProcessor().Process(input).Should().Be(expectedOutput);

        [Fact]
        public void InputFilePartOne()
        {
            var lines = File.ReadAllText("Inputs/03-GearRatiosInput.txt");
            new GearRatioProcessor().Process(lines).Should().Be(519444);
        }
    }
}