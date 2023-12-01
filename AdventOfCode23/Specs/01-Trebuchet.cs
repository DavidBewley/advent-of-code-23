using AdventOfCode23.Processors;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace AdventOfCode23.Specs
{
    public class _01_Trebuchet
    {
        [Theory]
        [InlineData("1abc2", 12)]
        [InlineData("pqr3stu8vwx", 38)]
        [InlineData("a1b2c3d4e5f", 15)]
        [InlineData("treb7uchet", 77)]
        [InlineData("1abc2\npqr3stu8vwx\na1b2c3d4e5f\ntreb7uchet", 142)]
        public void CorrectCalibrationIsReturned(string input, int output) 
            => new TrebuchetProcessor().Process(input).Should().Be(output);

        [Fact]
        public void InputFileProducesResult()
        {
            var lines = File.ReadAllText("Inputs/01-TrebuchetInput.txt");
            new TrebuchetProcessor().Process(lines).Should().Be(56465);
        }

        [Theory]
        [InlineData("two1nine", 29)]
        [InlineData("eightwothree", 83)]
        [InlineData("abcone2threexyz", 13)]
        [InlineData("xtwone3four", 24)]
        [InlineData("4nineeightseven2", 42)]
        [InlineData("zoneight234", 14)]
        [InlineData("7pqrstsixteen", 76)]
        [InlineData("two1nine\neightwothree\nabcone2threexyz\nxtwone3four\n4nineeightseven2\nzoneight234\n7pqrstsixteen", 281)]
        public void CorrectCalibrationWithWordsIsReturned(string input, int output)
            => new TrebuchetProcessor().ProcessWithWords(input).Should().Be(output);

        [Fact]
        public void InputFileWithWordsProducesResult()
        {
            var lines = File.ReadAllText("Inputs/01-TrebuchetInput.txt");
            new TrebuchetProcessor().ProcessWithWords(lines).Should().Be(55902);
        }
    }
}
