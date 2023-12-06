using AdventOfCode23.Processors;
using FluentAssertions;

namespace AdventOfCode23.Specs
{
    public class _05_PlantGrowthSpec
    {
        readonly string input = "seeds: 79 14 55 13\n\nseed-to-soil map:\n50 98 2\n52 50 48\n\nsoil-to-fertilizer map:\n0 15 37\n37 52 2\n39 0 15\n\nfertilizer-to-water map:\n49 53 8\n0 11 42\n42 0 7\n57 7 4\n\nwater-to-light map:\n88 18 7\n18 25 70\n\nlight-to-temperature map:\n45 77 23\n81 45 19\n68 64 13\n\ntemperature-to-humidity map:\n0 69 1\n1 0 69\n\nhumidity-to-location map:\n60 56 37\n56 93 4";

        [Theory]
        [InlineData(79,82)]
        [InlineData(14,43)]
        [InlineData(55,86)]
        [InlineData(13,35)]
        public void ClosestLocationIsFound(int seed, int expectedLocation) 
            => new PlantGrowthProcessor(input).FindClosetSeedLocation(seed).Should().Be(expectedLocation);

        [Fact]
        public void InputFileProducesResultPart1()
        {
            var input = File.ReadAllText("Inputs/05-PlantGrowth.txt");
            new PlantGrowthProcessor(input).FindClosetSeedLocation().Should().Be(107430936);
        }


        [Theory]
        [InlineData(46)]
        public void ClosestLocationWithSeedRangesIsFound(int expectedLocation)
            => new PlantGrowthProcessor(input).FindClosetSeedLocationWithRange().Should().Be(expectedLocation);

        [Fact]
        public void InputFileProducesResultPart2()
        {
            var input = File.ReadAllText("Inputs/05-PlantGrowth.txt");
            new PlantGrowthProcessor(input).FindClosetSeedLocationWithRange().Should().Be(0);
        }
    }
}
