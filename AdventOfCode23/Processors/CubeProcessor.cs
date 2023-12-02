using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace AdventOfCode23.Processors
{
    public class CubeProcessor
    {
        private readonly int _redCubesForValidGame;
        private readonly int _greenCubesForValidGame;
        private readonly int _blueCubesForValidGame;
        public CubeProcessor(int red, int green, int blue)
        {
            _redCubesForValidGame = red;
            _greenCubesForValidGame = green;
            _blueCubesForValidGame = blue;
        }

        public CubeProcessor()
        {
            _redCubesForValidGame = 0;
            _greenCubesForValidGame = 0;
            _blueCubesForValidGame = 0;
        }

        public int TotalOfGamesValid(string input)
        {
            var runningTotal = 0;
            var games = input.Split('\n');
            foreach (var game in games)
            {
                if(string.IsNullOrEmpty(game))
                    continue;
                var cubeGame = new CubeGame(game);
                if(cubeGame.IsValid(_redCubesForValidGame,_blueCubesForValidGame,_greenCubesForValidGame))
                    runningTotal+= cubeGame.GameId;
            }
            return runningTotal;
        }

        public int GetPowerTotal(string input)
        {
            var runningTotal = 0;
            var games = input.Split('\n');
            foreach (var game in games)
            {
                if (string.IsNullOrEmpty(game))
                    continue;
                var cubeGame = new CubeGame(game);
                runningTotal += cubeGame.GetPower();
            }
            return runningTotal;
        }
    }

    public class CubeGame
    {
        public int GameId { get; private set; }
        public int RedCubesMaxShown { get; private set; }
        public int BlueCubesMaxShown { get; private set; }
        public int GreenCubesMaxShown { get; private set; }


        public bool IsValid(int validRed, int validBlue, int validGreen) 
            => validRed >= RedCubesMaxShown && validBlue>= BlueCubesMaxShown && validGreen >= GreenCubesMaxShown;

        public int GetPower()
            => RedCubesMaxShown * BlueCubesMaxShown * GreenCubesMaxShown;

        public CubeGame(string inputString)
        {
            GameId = int.Parse(inputString.Split(":")[0].Split("Game ")[1]);
            var sets = inputString.Split(":")[1].Split(";");
            foreach (var set in sets)
            {
                foreach (var die in set.Split(","))
                {
                    _ = die switch
                    {
                        not null when die.Contains("red") => RedCubesMaxShown =
                            int.Parse(die.Split(" red")[0]) > RedCubesMaxShown
                                ? int.Parse(die.Split(" red")[0])
                                : RedCubesMaxShown,
                        not null when die.Contains("blue") => BlueCubesMaxShown =
                            int.Parse(die.Split(" blue")[0]) > BlueCubesMaxShown
                                ? int.Parse(die.Split(" blue")[0])
                                : BlueCubesMaxShown,
                        not null when die.Contains("green") => GreenCubesMaxShown =
                            int.Parse(die.Split(" green")[0]) > GreenCubesMaxShown
                                ? int.Parse(die.Split(" green")[0])
                                : GreenCubesMaxShown,
                        _ => 0,
                    };
                }
            }
        }
    }
}
