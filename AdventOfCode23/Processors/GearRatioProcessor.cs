namespace AdventOfCode23.Processors
{
    public class GearRatioProcessor
    {
        public int Process(string input)
        {
            var runningTotal = 0;
            var lines = input.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                var nextDigitPos = GetNextDigitPosition(lines[i], 0);
                while (nextDigitPos != -1)
                {
                    var fullNumber = GetFullNumber(lines[i], nextDigitPos);
                    if (NumberIsTouchingValidCharacter(lines, i, nextDigitPos, fullNumber.ToString().Length))
                        runningTotal += fullNumber;

                    nextDigitPos = GetNextDigitPosition(lines[i], nextDigitPos + fullNumber.ToString().Length);
                }
            }
            return runningTotal;
        }

        private int GetNextDigitPosition(string line, int currentPosition)
        {
            for (int i = currentPosition; i < line.Length; i++)
            {
                if (char.IsDigit(line[i]))
                    return i;
            }

            return -1;
        }

        private int GetFullNumber(string line, int digitPosition)
        {
            var digits = "";
            for (int i = digitPosition; i < line.Length; i++)
            {
                if (!char.IsDigit(line[i]) || i == line.Length)
                    return int.Parse(digits);
                digits += line[i];
            }

            return int.Parse(digits);
        }

        private bool NumberIsTouchingValidCharacter(string[] lines, int currentLine, int currentPosition, int numberLength)
        {
            if (currentLine > 0)
            {
                for (int i = 0; i < numberLength + 2; i++)
                {
                    if (currentPosition == 0 && i == 0)
                        continue;

                    if (currentPosition + i - 1 >= lines[currentLine - 1].Length)
                        continue;

                    if (IsValidCharacter(lines[currentLine - 1][i + currentPosition - 1]))
                        return true;
                }
            }

            if (currentPosition != 0 && IsValidCharacter(lines[currentLine][currentPosition - 1]))
                return true;
            if (currentPosition + numberLength + 1 <= lines[currentLine].Length && IsValidCharacter(lines[currentLine][currentPosition + numberLength]))
                return true;

            if (currentLine < lines.Length - 1)
            {
                for (int i = 0; i < numberLength + 2; i++)
                {
                    if (currentPosition == 0 && i == 0)
                        continue;

                    if (currentPosition + i - 1 >= lines[currentLine + 1].Length)
                        continue;

                    if (IsValidCharacter(lines[currentLine + 1][i + currentPosition - 1]))
                        return true;
                }
            }

            return false;
        }

        private readonly List<char> _validChars = new()
        {
            '*','/','-','@','$','=','%','+','#','&'
        };

        private bool IsValidCharacter(char character)
            => _validChars.Contains(character);
    }
}
