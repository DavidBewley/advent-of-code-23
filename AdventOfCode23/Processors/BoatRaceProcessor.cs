using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Processors
{
    public class BoatRaceProcessor
    {
        public long GetNumberOfWaysToWin(string input)
        {
            var times = input.Split("Time:      ")[1].Split("\n")[0].Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToList();
            var distances = input.Split("Distance:  ")[1].Split("\n")[0].Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToList();
            long result = 1;

            for (int i = 0; i < times.Count; i++)
            {
                result *= new BoatRace(long.Parse(times[i]), long.Parse(distances[i])).NumberOfWaysToWin;
            }

            return result;
        }

        public long GetNumberOfWaysToWinBadKerning(string input)
        {
            var time = input.Split("Time:      ")[1].Split("\n")[0].Replace(" ", "");
            var distance = input.Split("Distance:  ")[1].Split("\n")[0].Replace(" ", "");
            long result = 1;

            result *= new BoatRace(long.Parse(time), long.Parse(distance)).NumberOfWaysToWin;

            return result;
        }
    }

    public class BoatRace
    {
        public long Time { get; set; }
        public long RecordDistance { get; set; }
        public long NumberOfWaysToWin { get; set; }

        public BoatRace(long time, long recordDistance)
        {
            Time = time;
            RecordDistance = recordDistance;

            var firstWin = 0;

            if (time < 1000)
            {
                for (int i = 0; i < Time; i++)
                {
                    if (CalculateIfWon(i, Time - i))
                    {
                        firstWin = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Time; i+=100000)
                {
                    if (CalculateIfWon(i, Time - i))
                    {
                        firstWin = i;
                        break;
                    }
                }

                var firstLoss = 0;
                for (int i = firstWin; i < firstWin - 100000; i--)
                {
                    if (!CalculateIfWon(i, Time - i))
                    {
                        firstLoss = i;
                        break;
                    }
                }

                firstWin = firstLoss + 1;
            }

            NumberOfWaysToWin = Time - firstWin - firstWin + 1;

        }

        private bool CalculateIfWon(long ticksToAccelerate, long ticksToCoast)
        {
            var distanceTraveled = 0;
            var speed = 0;
            for (int i = 0; i < ticksToAccelerate; i++)
            {
                speed++;
            }

            for (int i = 0; i < ticksToCoast; i++)
            {
                distanceTraveled += speed;
            }

            return distanceTraveled > RecordDistance;
        }
    }
}
