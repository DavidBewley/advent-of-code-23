using System.Text.RegularExpressions;

namespace AdventOfCode23.Processors
{
    public class TrebuchetProcessor
    {
        private const string PatternWithoutWords = "(1|2|3|4|5|6|7|8|9)";
        private const string PatternWithWords = "(one|two|three|four|five|six|seven|eight|nine|1|2|3|4|5|6|7|8|9)";

        private readonly Dictionary<string, int> _digitConversions = new()
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
        };

        public int ProcessWithoutWords(string input)
        {
            var lines = input.Split('\n');
            return lines.Sum(line => GetCombinedDigits(line, PatternWithoutWords));
        }

        public int ProcessWithWords(string input)
        {
            var lines = input.Split('\n');
            return lines.Sum(line => GetCombinedDigits(line, PatternWithWords));
        }

        private int GetCombinedDigits(string line, string pattern)
        {
            if (string.IsNullOrEmpty(line))
                return 0;

            var first = FindDigit(line, pattern, RegexOptions.None);
            var last = FindDigit(line, pattern, RegexOptions.RightToLeft);

            return int.Parse($"{first}{last}");
        }

        private int FindDigit(string line, string pattern, RegexOptions options)
        {
            var result = new Regex(pattern, options).Match(line);
            if (result.Success)
                return _digitConversions.TryGetValue(result.Value, out var digit) ? digit : int.Parse(result.Value);
            return 0;
        }
    }
}
