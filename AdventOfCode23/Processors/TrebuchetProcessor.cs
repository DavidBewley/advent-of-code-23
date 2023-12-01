using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode23.Processors
{
    public class TrebuchetProcessor
    {
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

        public int Process(string input)
        {
            var lines = input.Split('\n');
            return lines.Sum(line => GetCombinedDigits(line, false));
        }

        public int ProcessWithWords(string input)
        {
            var lines = input.Split('\n');
            return lines.Sum(line => GetCombinedDigits(line, true));
        }

        private int GetCombinedDigits(string line, bool includeWords)
        {
            if (string.IsNullOrEmpty(line))
                return 0;

            var first = FindDigit(line, includeWords, false);
            var last = FindDigit(line, includeWords, true);

            return int.Parse($"{first}{last}");
        }

        private int FindDigit(string line, bool includeWords, bool rightToLeftSearch)
        {
            var patternWithoutWordsNumbers = new Regex("(1|2|3|4|5|6|7|8|9)");
            var patternWithoutWordsNumbersRightLeft = new Regex("(1|2|3|4|5|6|7|8|9)", RegexOptions.RightToLeft);
            var patternWithWordsNumbers = new Regex("(one|two|three|four|five|six|seven|eight|nine|1|2|3|4|5|6|7|8|9)");
            var patternWithWordsNumbersRightLeft = new Regex("(one|two|three|four|five|six|seven|eight|nine|1|2|3|4|5|6|7|8|9)", RegexOptions.RightToLeft);

            Match? result;
            if (includeWords)
            {
                result = rightToLeftSearch 
                    ? patternWithWordsNumbersRightLeft.Match(line) 
                    : patternWithWordsNumbers.Match(line);
            }
            else
            {
                result = rightToLeftSearch 
                    ? patternWithoutWordsNumbersRightLeft.Match(line) 
                    : patternWithoutWordsNumbers.Match(line);
            }

            if (result.Success)
                return _digitConversions.TryGetValue(result.Value, out var digit) ? digit : int.Parse(result.Value);
            return 0;
        }
    }
}
